using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{id}", Name = "GetExcel")]
        public IActionResult ImportExcel(int id)
        {
            byte[] fileContents;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            ProformaExcelDataAccessLayer dat = new ProformaExcelDataAccessLayer();
            String consulta;

            using (var package = new ExcelPackage())
            {


                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                #region Header
                workSheet.Cells[1, 1].Value = "Rubro";
                workSheet.Cells[1, 1].Style.Font.Size = 12;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 2].Value = "Total";
                workSheet.Cells[1, 2].Style.Font.Size = 12;
                workSheet.Cells[1, 2].Style.Font.Bold = true;
                workSheet.Cells[1, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 3].Value = "Años Anteriores";
                workSheet.Cells[1, 3].Style.Font.Size = 12;
                workSheet.Cells[1, 3].Style.Font.Bold = true;
                workSheet.Cells[1, 3].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 4].Value = "Ejercicio";
                workSheet.Cells[1, 4].Style.Font.Size = 12;
                workSheet.Cells[1, 4].Style.Font.Bold = true;
                workSheet.Cells[1, 4].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 5].Value = "Enero";
                workSheet.Cells[1, 5].Style.Font.Size = 12;
                workSheet.Cells[1, 5].Style.Font.Bold = true;
                workSheet.Cells[1, 5].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 6].Value = "Febrero";
                workSheet.Cells[1, 6].Style.Font.Size = 12;
                workSheet.Cells[1, 6].Style.Font.Bold = true;
                workSheet.Cells[1, 6].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 7].Value = "Marzo";
                workSheet.Cells[1, 7].Style.Font.Size = 12;
                workSheet.Cells[1, 7].Style.Font.Bold = true;
                workSheet.Cells[1, 7].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 8].Value = "Abril";
                workSheet.Cells[1, 8].Style.Font.Size = 12;
                workSheet.Cells[1, 8].Style.Font.Bold = true;
                workSheet.Cells[1, 8].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 9].Value = "Mayo";
                workSheet.Cells[1, 9].Style.Font.Size = 12;
                workSheet.Cells[1, 9].Style.Font.Bold = true;
                workSheet.Cells[1, 9].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 10].Value = "Junio";
                workSheet.Cells[1, 10].Style.Font.Size = 12;
                workSheet.Cells[1, 10].Style.Font.Bold = true;
                workSheet.Cells[1, 10].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 11].Value = "Julio";
                workSheet.Cells[1, 11].Style.Font.Size = 12;
                workSheet.Cells[1, 11].Style.Font.Bold = true;
                workSheet.Cells[1, 11].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 12].Value = "Agosto";
                workSheet.Cells[1, 12].Style.Font.Size = 12;
                workSheet.Cells[1, 12].Style.Font.Bold = true;
                workSheet.Cells[1, 12].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 13].Value = "Septiembre";
                workSheet.Cells[1, 13].Style.Font.Size = 12;
                workSheet.Cells[1, 13].Style.Font.Bold = true;
                workSheet.Cells[1, 13].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 14].Value = "Octubre";
                workSheet.Cells[1, 14].Style.Font.Size = 12;
                workSheet.Cells[1, 14].Style.Font.Bold = true;
                workSheet.Cells[1, 14].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 15].Value = "Noviembre";
                workSheet.Cells[1, 15].Style.Font.Size = 12;
                workSheet.Cells[1, 15].Style.Font.Bold = true;
                workSheet.Cells[1, 15].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 16].Value = "Diciembre";
                workSheet.Cells[1, 16].Style.Font.Size = 12;
                workSheet.Cells[1, 16].Style.Font.Bold = true;
                workSheet.Cells[1, 16].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                workSheet.Cells[1, 17].Value = "Años Posteriores";
                workSheet.Cells[1, 17].Style.Font.Size = 12;
                workSheet.Cells[1, 17].Style.Font.Bold = true;
                workSheet.Cells[1, 17].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                #endregion
                int count = 2;
                consulta = "SELECT * FROM proforma_detalle WHERE id_proforma = " + id;
                dt = dat.Detalle(consulta);
                foreach (DataRow r in dt.Rows)
                {
                    consulta = "SELECT * FROM rubro WHERE id = " + Convert.ToString(r["rubro_id"]);
                    dt2 = dat.Detalle(consulta);
                    #region Body Row
                    foreach (DataRow d in dt2.Rows)
                    {
                        workSheet.Cells[count, 1].Value = Convert.ToString(d["nombre"]);
                    }

                    workSheet.Cells[count, 1].Style.Font.Size = 12;
                    workSheet.Cells[count, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    //workSheet.Cells[count, 2].Value = Convert.ToDouble(r["total_resultado"]);
                    workSheet.Cells[count, 2].Formula = "=SUMA(" + workSheet.Cells[count, 3].Address + ":" + workSheet.Cells[count, 16].Address + ")";
                    workSheet.Cells[count, 2].Style.Font.Size = 12;
                    workSheet.Cells[count, 2].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 3].Value = Convert.ToDouble(r["acumulado_resultado"]);
                    workSheet.Cells[count, 3].Style.Font.Size = 12;
                    workSheet.Cells[count, 3].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 4].Value = Convert.ToDouble(r["ejercicio_resultado"]);
                    workSheet.Cells[count, 4].Style.Font.Size = 12;
                    workSheet.Cells[count, 4].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 5].Value = Convert.ToDouble(r["enero_monto_resultado"]);
                    workSheet.Cells[count, 5].Style.Font.Size = 12;
                    workSheet.Cells[count, 5].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 6].Value = Convert.ToDouble(r["febrero_monto_resultado"]);
                    workSheet.Cells[count, 6].Style.Font.Size = 12;
                    workSheet.Cells[count, 6].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 7].Value = Convert.ToDouble(r["marzo_monto_resultado"]);
                    workSheet.Cells[count, 7].Style.Font.Size = 12;
                    workSheet.Cells[count, 7].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 8].Value = Convert.ToDouble(r["abril_monto_resultado"]);
                    workSheet.Cells[count, 8].Style.Font.Size = 12;
                    workSheet.Cells[count, 8].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 9].Value = Convert.ToDouble(r["mayo_monto_resultado"]);
                    workSheet.Cells[count, 9].Style.Font.Size = 12;
                    workSheet.Cells[count, 9].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 10].Value = Convert.ToDouble(r["junio_monto_resultado"]);
                    workSheet.Cells[count, 10].Style.Font.Size = 12;
                    workSheet.Cells[count, 10].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 11].Value = Convert.ToDouble(r["julio_monto_resultado"]);
                    workSheet.Cells[count, 11].Style.Font.Size = 12;
                    workSheet.Cells[count, 11].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 12].Value = Convert.ToDouble(r["agosto_monto_resultado"]);
                    workSheet.Cells[count, 12].Style.Font.Size = 12;
                    workSheet.Cells[count, 12].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 13].Value = Convert.ToDouble(r["septiembre_monto_resultado"]);
                    workSheet.Cells[count, 13].Style.Font.Size = 12;
                    workSheet.Cells[count, 13].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 14].Value = Convert.ToDouble(r["octubre_monto_resultado"]);
                    workSheet.Cells[count, 14].Style.Font.Size = 12;
                    workSheet.Cells[count, 14].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 15].Value = Convert.ToDouble(r["noviembre_monto_resultado"]);
                    workSheet.Cells[count, 15].Style.Font.Size = 12;
                    workSheet.Cells[count, 15].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 16].Value = Convert.ToDouble(r["diciembre_monto_resultado"]);
                    workSheet.Cells[count, 16].Style.Font.Size = 12;
                    workSheet.Cells[count, 16].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    workSheet.Cells[count, 17].Value = Convert.ToDouble(r["acumulado_resultado"]);
                    workSheet.Cells[count, 17].Style.Font.Size = 12;
                    workSheet.Cells[count, 17].Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    #endregion
                    count = count + 1;
                }

                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }



            return File(fileContents: fileContents, contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: "Proforma.xlsx");
            //return View();
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
    }
}
