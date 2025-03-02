using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PatiliDost.Data;
using PatiliDost.Models;

namespace PatiliDost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly PatiAPIDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "ContactsCache";

        public ContactsController(PatiAPIDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<Contact> cacheData))
            {
                return Ok(cacheData); 
            }

            var contacts = await _context.Contacts.AsNoTracking().ToListAsync();
            _memoryCache.Set(CacheKey, contacts, TimeSpan.FromMinutes(5)); 

            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound(new { message = "Randevu bulunamadı." });
            }

            return Ok(contact);
        }

       
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            bool isTimeSlotTaken = await _context.Contacts
                .AnyAsync(r => r.AppointmentDate == contact.AppointmentDate && r.AppointmentTime == contact.AppointmentTime);

            if (isTimeSlotTaken)
            {
                return BadRequest(new { message = "Bu tarih ve saat için zaten bir randevu mevcut. Lütfen başka bir zaman seçin." });
            }

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            _memoryCache.Remove(CacheKey); 

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest(new { message = "Geçersiz ID." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _memoryCache.Remove(CacheKey); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ContactExists(id))
                {
                    return NotFound(new { message = "Randevu bulunamadı." });
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound(new { message = "Randevu bulunamadı." });
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            _memoryCache.Remove(CacheKey);

            return NoContent();
        }

      
        private async Task<bool> ContactExists(int id)
        {
            return await _context.Contacts.AnyAsync(e => e.Id == id);
        }
    }
}
