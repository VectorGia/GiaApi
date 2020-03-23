using System;
using System.Collections.Generic;
using AppGia.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLController : ControllerBase
    {

        private ETLHelper _helper;
        
        [HttpPost("contable")]
        public Dictionary<string,object> contable([FromBody] ETLRequest request)
        {
            _helper.extraeBalanza(request.anioInicio,request.anioFin);
            Dictionary<string,Object> res=new Dictionary<string, Object>();
            res.Add("resultado",Boolean.TrueString);
            return res;
        }
        
        [HttpPost("flujo")]
        public Dictionary<string,object> flujo([FromBody] ETLRequest request)
        {
            _helper.extraeFlujo(request.anioInicio,request.anioFin,request.mes);
            Dictionary<string,Object> res=new Dictionary<string, Object>();
            res.Add("resultado",Boolean.TrueString);
            return res;
        }
    }
}