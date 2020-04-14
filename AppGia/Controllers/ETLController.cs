using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Helpers;
using AppGia.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLController : ControllerBase
    {
        private ETLHelper _helper=new ETLHelper();
        private QueryExecuter _queryExecuter=new QueryExecuter();

        [HttpPost("contable")]
        public Dictionary<string, object> contable([FromBody] ETLRequest request)
        {
            _helper.extraeBalanza(request.anioInicio, request.anioFin);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }

        [HttpPost("rescheduleContable")]
        public Dictionary<string, object> rescheduleContable([FromBody] Dictionary<string, string> cronExpr)
        {
            ExtraccionProcess.rescheduleContable(cronExpr["valor"], 0);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }

        [HttpPost("flujo")]
        public Dictionary<string, object> flujo([FromBody] ETLRequest request)
        {
            _helper.extraeFlujo(request.anioInicio, request.anioFin, request.mes);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }

        [HttpPost("rescheduleFlujo")]
        public Dictionary<string, object> rescheduleFlujo([FromBody] Dictionary<string, string> cronExpr)
        {
            ExtraccionProcess.rescheduleFlujo(cronExpr["valor"],0);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }
        
        
        [HttpGet("estatusContable")]
        public Dictionary<string, object> estatusContable()
        {
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado",ExtraccionProcess.getEstatusContable());
            return res;
        }
        
        [HttpGet("estatusFlujo")]
        public Dictionary<string, object> estatusFlujo()
        {
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado",ExtraccionProcess.getEstatusFlujo());
            return res;
        }
        
        [HttpGet("configCrons")]
        public Dictionary<string, string> configCrons()
        {
            DataTable dataTable = _queryExecuter.ExecuteQuery("select clave,cron_expresion from programacion_proceso");
            Dictionary<string,string> res=new Dictionary<string, string>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                res.Add("clave",rdr["clave"].ToString());
                res.Add("cron_expresion",rdr["cron_expresion"].ToString());
            }

            return res;
        }
    }
}