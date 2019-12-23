using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionRolPermisoController : ControllerBase
    {
        // GET: api/RelacionRolPermiso
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/RelacionRolPermiso/5
        [HttpGet("{id}", Name = "RelacionRolPermiso")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RelacionRolPermiso
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/RelacionRolPermiso/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
