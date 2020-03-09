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
        public List<ProformaDetalle> import([FromBody]String excelFileB64)
        {
            return _proformaExcelHelper.import(FromBase64String(excelFileB64));
        }
        
        // POST: api/ProformaExcel
        [HttpPost("export")]

        public string export([FromBody]List<ProformaDetalle> detallesProfToRender)
        {
            return  ToBase64String(_proformaExcelHelper.export(detallesProfToRender));
        }
        

    }
}
