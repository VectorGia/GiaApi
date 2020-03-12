using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        CuentasDataAccessLayer objCuentas = new CuentasDataAccessLayer();
        // GET: api/Cuentas
        [HttpGet]
        public IEnumerable<Cuentas> Get()
        {
            return objCuentas.GetAllCuentas();
        }

        // GET: api/Cuentas/5
        [HttpGet("{id}", Name = "GetCuentas")]
        public IEnumerable<Cuentas> GetCuentas(string id)
        {
            return objCuentas.GetCuentasData(id);
        }

        // PUT: api/Cuentas/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        public int Create([FromBody]Cuentas cuentas)
        {
            return objCuentas.AddCuenta(cuentas);
        }
    }
}
