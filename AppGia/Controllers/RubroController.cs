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
    public class RubrosController : ControllerBase
    {
        RubrosDataAccesLayer objrubro = new RubrosDataAccesLayer();
        // GET: api/Rubros
        [HttpGet]
        public IEnumerable<Rubros> Get()
        {
            return objrubro.GetAllRubros();
        }

        // GET: api/Rubros/5
        [HttpGet("id/{id}", Name = "GetRubros")]
        public List<Rubros> Get(int id)
        {
            return objrubro.GetRubroById(id);
        }

        [HttpGet("{id}", Name = "GetRubrosByModeloId")]
        public List<Rubros> GetRubrosByModeloId(int id)
        {
            return objrubro.GetRubroByModeloId(id);
        }
        // POST: api/Rubros
        [HttpPost]
        public int Post([FromBody] Rubros rubro)
        {
            return objrubro.InsertRubro(rubro);
        }

        // PUT: api/Rubros/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Rubros rubro)
        {
            return objrubro.UpdateRubro(id, rubro);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return objrubro.DeleteRubro(id);
        }
    }
}
