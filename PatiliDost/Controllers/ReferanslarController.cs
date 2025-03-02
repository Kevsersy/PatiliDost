using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatiliDost.Data;
using PatiliDost.Models;
using PatiliDost.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace PatiliDost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferanslarController : ControllerBase
    {
        private readonly PatiAPIDbContext _db;

        public ReferanslarController(PatiAPIDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommentDTO>> Get()
        {
            var response = _db.Comments
                .Select(item => new CommentDTO(item.Name, item.Message, item.Id))
                .ToList();

            return Ok(response);
        }

        [HttpGet("{Name}")]
        public ActionResult<CommentDTO> Get(string Name)
        {
            var comment = _db.Comments.FirstOrDefault(c => c.Name == Name);
            if (comment is null) return NotFound();

            return Ok(new CommentDTO(comment.Name, comment.Message, comment.Id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CommentCreatDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = new Comment
            {
                Name = model.Name,
                Message = model.Message
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return CreatedAtAction(nameof(Get), new { Name = comment.Name }, new CommentDTO(comment.Name, comment.Message, comment.Id));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CommentUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = _db.Comments.Find(id);

            if (comment is null)
            {
                comment = new Comment
                {
                    Name = model.Name,
                    Message = model.message
                };
                _db.Comments.Add(comment);
                _db.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = comment.Id }, new CommentDTO(comment.Name, comment.Message, comment.Id));
            }

            comment.Name = model.Name;
            comment.Message = model.message;
            _db.SaveChanges();

            return NoContent();
        }
    }
}
