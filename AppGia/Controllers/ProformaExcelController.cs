using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaExcelController : ControllerBase
    {
        private ProformaExcelHelper _proformaExcelHelper=new ProformaExcelHelper();
        
        [HttpPost("import")]
        public List<ProformaDetalle> import(String excelFileB64)
        {
            return _proformaExcelHelper.import(Convert.FromBase64String(excelFileB64));
        }
        
        // POST: api/ProformaExcel
        [HttpPost("export")]
        public IActionResult export([FromBody]List<ProformaDetalle> lstGuardaProforma)
        {
            return _proformaExcelHelper.export(lstGuardaProforma);
        }
        

    }
}
