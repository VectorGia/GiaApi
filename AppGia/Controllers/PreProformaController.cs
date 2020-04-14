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
        public Dictionary<string, object> montosRescheduleContable([FromBody] Dictionary<string, string> cronExpr)
        {
            MontosConsolidadosProcess.rescheduleContable(cronExpr["valor"],0);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }
        

        [HttpPost("montosRescheduleFlujo")]
        public Dictionary<string, object> montosRescheduleFlujo([FromBody] Dictionary<string, string> cronExpr)
        {
            MontosConsolidadosProcess.rescheduleFlujo(cronExpr["valor"],0);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }
        
        [HttpGet("estatusContable")]
        public Dictionary<string, object> estatusContable()
        {
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado",MontosConsolidadosProcess.getEstatusContable());
            return res;
        }
        
        [HttpGet("estatusFlujo")]
        public Dictionary<string, object> estatusFlujo()
        {
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado",MontosConsolidadosProcess.getEstatusFlujo());
            return res;
        }
    }
}
