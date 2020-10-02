using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionRolController : ControllerBase
    {
        RelacionRolDataAccessLayer _relacionRolDataAccessLayer = new RelacionRolDataAccessLayer();

        [HttpGet]
        public IEnumerable<RelacionRol> Get()
        {
            return _relacionRolDataAccessLayer.GetAllRelacionRol();
        }


        [HttpPost]
        public int Post([FromBody] RelacionRol relacion)
        {
            return _relacionRolDataAccessLayer.insert(relacion);
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _relacionRolDataAccessLayer.delete(id);
        }
    }
}