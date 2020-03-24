using System;
using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreProformaController : ControllerBase
    {
        PreProformaDataAccessLayer prepro = new PreProformaDataAccessLayer();
        
        [HttpGet]
        public int GetPreProforma()
        {
            return prepro.MontosConsolidados();
        }
        

        [HttpPost("montosRescheduleContable")]
        public Dictionary<string, object> montosRescheduleContable([FromBody] string cronExpr)
        {
            MontosConsolidadosProcess.rescheduleContable(cronExpr);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }
        

        [HttpPost("montosRescheduleFlujo")]
        public Dictionary<string, object> montosRescheduleFlujo([FromBody] string cronExpr)
        {
            MontosConsolidadosProcess.rescheduleFlujo(cronExpr);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }
    }
}
