using System;
using System.Collections.Generic;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Convert;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaExcelController : ControllerBase
    {
        private ProformaExcelHelper _proformaExcelHelper=new ProformaExcelHelper();
        
        [HttpPost("import")]
        public List<ProformaDetalle> import([FromBody] Dictionary<string,string> data)
        {
            String excelFileB64 = data["b64Data"];
            return _proformaExcelHelper.import(FromBase64String(excelFileB64));
        }
        
        // POST: api/ProformaExcel
        [HttpPost("export")]
        public Dictionary<string,string> export([FromBody]List<ProformaDetalle> detallesProfToRender)

        {
            string resB64=  ToBase64String(_proformaExcelHelper.export(detallesProfToRender));
            Dictionary<string, string> dic=new Dictionary<string, string>();
            dic.Add("resB64",resB64);
            return  dic;
        }
        

    }
}
