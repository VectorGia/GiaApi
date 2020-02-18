using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaExcelController : ControllerBase
    {
        // GET: api/ProformaExcel
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ProformaExcel/5

       /* [HttpGet("{id}", Name = "GetExcel")]
        public IActionResult ImportExcel(int id) 
        { }*/

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ProformaExcel
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ProformaExcel/5
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
        public IActionResult ImportExcel(Int64 idProforma)

        {
            ProformaExcelDataAccessLayer dat = new ProformaExcelDataAccessLayer();
            String consulta= "SELECT * FROM proforma_detalle WHERE id_proforma = "+idProforma;
            List<ProformaDetalle> detalles=transformDtToDetalles(dat.Detalle(consulta));
            return buildProformaToExcel(detalles);
        }
        
        [HttpPost]
        public IActionResult Create([FromBody]List<ProformaDetalle> lstGuardaProforma)
        {
            return buildProformaToExcel(lstGuardaProforma);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Proforma proforma)
        {
            List<ProformaDetalle> detalles = new ProformaDataAccessLayer().GeneraProforma(proforma.centro_costo_id,
                proforma.anio, proforma.tipo_proforma_id, proforma.tipo_captura_id);
            return buildProformaToExcel(detalles);
        }

        private List<ProformaDetalle> transformDtToDetalles(DataTable dataTableDetalles)
        {
            //llenado de lista
            return null;
        }

        public IActionResult buildProformaToExcel(List<ProformaDetalle> detalles)
        {
            ProformaExcelDataAccessLayer dat = new ProformaExcelDataAccessLayer();
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {

                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int count = 2;

                foreach (ProformaDetalle detalle in detalles)

                {
                    DataTable dt2 = dat.Detalle( "SELECT * FROM rubro WHERE id = " + detalle.rubro_id);
                    

                    foreach (DataRow d in dt2.Rows)
                    {
                        workSheet.Cells[count, 1].Value = Convert.ToString(d["nombre"]);
                    }

                    workSheet.Cells[count, 1].Style.Font.Size = 12;
                    workSheet.Cells[count, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    //workSheet.Cells[count, 2].Value = Convert.ToDouble(r["total_resultado"]);
                    workSheet.Cells[count, 2].Formula = "=SUMA(" + workSheet.Cells[count, 3].Address + ":" +
                                                        workSheet.Cells[count, 16].Address + ")";
                    workSheet.Cells[count, 2].Style.Font.Size = 12;
                    workSheet.Cells[count, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 3].Value = Convert.ToDouble(detalle.acumulado_resultado);
                    workSheet.Cells[count, 3].Style.Font.Size = 12;
                    workSheet.Cells[count, 3].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 4].Value = Convert.ToDouble(detalle.ejercicio_resultado);
                    workSheet.Cells[count, 4].Style.Font.Size = 12;
                    workSheet.Cells[count, 4].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 5].Value = Convert.ToDouble(detalle.enero_monto_resultado);
                    workSheet.Cells[count, 5].Style.Font.Size = 12;
                    workSheet.Cells[count, 5].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 6].Value = Convert.ToDouble(detalle.febrero_monto_resultado);
                    workSheet.Cells[count, 6].Style.Font.Size = 12;
                    workSheet.Cells[count, 6].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 7].Value = Convert.ToDouble(detalle.marzo_monto_resultado);
                    workSheet.Cells[count, 7].Style.Font.Size = 12;
                    workSheet.Cells[count, 7].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 8].Value = Convert.ToDouble(detalle.abril_monto_resultado);
                    workSheet.Cells[count, 8].Style.Font.Size = 12;
                    workSheet.Cells[count, 8].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 9].Value = Convert.ToDouble(detalle.mayo_monto_resultado);
                    workSheet.Cells[count, 9].Style.Font.Size = 12;
                    workSheet.Cells[count, 9].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 10].Value = Convert.ToDouble(detalle.junio_monto_resultado);
                    workSheet.Cells[count, 10].Style.Font.Size = 12;
                    workSheet.Cells[count, 10].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 11].Value = Convert.ToDouble(detalle.julio_monto_resultado);
                    workSheet.Cells[count, 11].Style.Font.Size = 12;
                    workSheet.Cells[count, 11].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 12].Value = Convert.ToDouble(detalle.agosto_monto_resultado);
                    workSheet.Cells[count, 12].Style.Font.Size = 12;
                    workSheet.Cells[count, 12].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 13].Value = Convert.ToDouble(detalle.septiembre_monto_resultado);
                    workSheet.Cells[count, 13].Style.Font.Size = 12;
                    workSheet.Cells[count, 13].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 14].Value = Convert.ToDouble(detalle.octubre_monto_resultado);
                    workSheet.Cells[count, 14].Style.Font.Size = 12;
                    workSheet.Cells[count, 14].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 15].Value = Convert.ToDouble(detalle.noviembre_monto_resultado);
                    workSheet.Cells[count, 15].Style.Font.Size = 12;
                    workSheet.Cells[count, 15].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 16].Value = Convert.ToDouble(detalle.diciembre_monto_resultado);
                    workSheet.Cells[count, 16].Style.Font.Size = 12;
                    workSheet.Cells[count, 16].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 17].Value = Convert.ToDouble(detalle.acumulado_resultado);
                    workSheet.Cells[count, 17].Style.Font.Size = 12;
                    workSheet.Cells[count, 17].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    
                    count++;
                }
                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            return File(fileContents: fileContents, contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: "Proforma.xlsx");
        }
    }
}
