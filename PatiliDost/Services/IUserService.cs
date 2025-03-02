using PatiliDost.Data.DTOs;



    namespace PatiliDost.Services
    {
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDTO model);
        Task<AuthenticationDTO> GetTokenAsync(TokenRequestDTO model);
        Task<string> AddRoleAsync(AddRoleDTO model);
    }

}


