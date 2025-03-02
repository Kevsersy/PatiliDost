using PatiliDost.Data.DTOs;
using PatiliDost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using static PatiliDost.Models.Authorization;

namespace PatiliDost.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<string> AddRoleAsync(AddRoleDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email); 

        if (user is null)
            return $"No accounts registered with {model.Email}";

        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            bool roleExists = Enum.GetNames(typeof(Roles)) 
                                 .Any(r => r.ToLower() == model.Role.ToLower());

            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Roles))
                                    .Cast<Roles>()
                                    .SingleOrDefault(r => r.ToString().ToLower() == model.Role.ToLower());

                await _userManager.AddToRoleAsync(user, validRole.ToString());

                return $"Added {model.Role} to user {model.Email}.";
            }
            return $"Role {model.Role} not found.";
        }
        return $"Incorrect credentials for user.";
    }

    public async Task<AuthenticationDTO> GetTokenAsync(TokenRequestDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return new AuthenticationDTO { IsAuthenticated = false, Message = $"No accounts registered with {model.Email}" };

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return new AuthenticationDTO { IsAuthenticated = false, Message = "Incorrect credentials for user." };

        JwtSecurityToken jwt = await CreateJwtToken(user);
        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

        return new AuthenticationDTO
        {
            IsAuthenticated = true,
            Message = "Authentication successful.",
            UserName = user.UserName!,
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            Email = user.Email!,
            Roles = roles.ToList()
        };
    }

    private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        var roleClaims = userRoles.Select(role => new Claim("roles", role)).ToList();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

     
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecureKey");
        var symmetricSecurityKey = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "SecureApi",
            audience: _configuration["Jwt:Audience"] ?? "SecureApiUser",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: signingCredentials
        );
    }

    public async Task<string> RegisterAsync(RegisterDTO model)
    {
        var user = new AppUser
        {
            UserName = model.UserName,
            Email = model.Email,
            CreatedAt = DateTime.Now
        };

        var userWithSameEmail = await _userManager.FindByEmailAsync(user.Email);

        if (userWithSameEmail is null)
        {
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
            return $"User Registered {user.UserName}";
        }
        else
        {
            return $"Email {user.Email} is already registered.";
        }
    }
}
