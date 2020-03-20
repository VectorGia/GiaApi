using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLController : ControllerBase
    {
        // POST: api/ETL
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}