using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private ReportesHelper _reportesHelper=new ReportesHelper();
        
        // POST: api/ProformaExcel
        [HttpPost("generar")]
        public Dictionary<string,string> export([FromBody] ReportesRequest request)

        {
            string resB64=  Convert.ToBase64String(_reportesHelper.buildReport(request));
            Dictionary<string, string> dic=new Dictionary<string, string>();
            dic.Add("resB64",resB64);
            return  dic;
        }

        [HttpGet("reportes")]
        public List<Dictionary<string, object>> getReportes()
        {
            return _reportesHelper.getReportesActivos();
        }
        
        [HttpGet("parametros/{idReport}")]
        public List<Dictionary<string, object>> getParametros(int idReport)
        {
            return _reportesHelper.getParametrosOf(idReport);
        }
        
    }
}
