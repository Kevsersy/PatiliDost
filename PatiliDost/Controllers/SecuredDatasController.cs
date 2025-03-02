using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatiliDost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuredDatasController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Burası yetkilendirilmiş alandır.");
        }
    }
}
