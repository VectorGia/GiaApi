using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PantallaController : ControllerBase
    {
        PantallaDataAccessLayer objpantalla = new PantallaDataAccessLayer();
        // GET: api/Pantalla
        [HttpGet]
        public IEnumerable<Pantalla> Get()
        {
            return objpantalla.GetAllPantallas();
        }

        // GET: api/Pantalla/5
        [HttpGet("{id}", Name = "GetPantallas")]
        public string GetPantallas(int id)
        {
            return "value";
        }

        // POST: api/Pantalla
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Pantalla/5
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
