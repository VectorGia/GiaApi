using System;
using System.Collections.Generic;
using AppGia.Helpers;
using AppGia.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLController : ControllerBase
    {
        private ETLHelper _helper;

        [HttpPost("contable")]
        public Dictionary<string, object> contable([FromBody] ETLRequest request)
        {
            _helper.extraeBalanza(request.anioInicio, request.anioFin);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }

        [HttpPost("rescheduleContable")]
        public Dictionary<string, object> rescheduleContable([FromBody] string cronExpr)
        {
            ExtraccionProcess.rescheduleContable(cronExpr);
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
        public Dictionary<string, object> rescheduleFlujo([FromBody] string cronExpr)
        {
            ExtraccionProcess.rescheduleFlujo(cronExpr);
            Dictionary<string, Object> res = new Dictionary<string, Object>();
            res.Add("resultado", Boolean.TrueString);
            return res;
        }
    }
}