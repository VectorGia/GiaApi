using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private QueryExecuter _queryExecuter=new QueryExecuter();
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

        [HttpGet("{id}", Name = "GetExcel")]
        public IActionResult ImportExcel(Int64 idProforma)

        {
            String consulta = "SELECT * FROM proforma_detalle WHERE id_proforma = " + idProforma;
            List<ProformaDetalle> detalles = transformDtToDetalles(_queryExecuter.ExecuteQuery(consulta));
            return buildProformaToExcel(detalles);
        }
        

        // POST: api/ProformaExcel
        [HttpPost]
        public IActionResult Create([FromBody]List<ProformaDetalle> lstGuardaProforma)
        {
            return buildProformaToExcel(lstGuardaProforma);
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
        public IActionResult Post([FromBody] Proforma proforma)
        {
            List<ProformaDetalle> detalles = new ProformaDataAccessLayer().manageBuildProforma(proforma.centro_costo_id,
                proforma.anio, proforma.tipo_proforma_id, proforma.tipo_captura_id);
            return buildProformaToExcel(detalles);

        }

        private List<ProformaDetalle> transformDtToDetalles(DataTable dataTableDetalles)
        {
            List<ProformaDetalle> listaRubros = new List<ProformaDetalle>();
            
            foreach (DataRow rubrosRow in dataTableDetalles.Rows)
            {
                ProformaDetalle rubro = new ProformaDetalle();
                rubro.id = Convert.ToInt64(rubrosRow["id"]);
                rubro.activo = Convert.ToBoolean(rubrosRow["activo"]);
                rubro.acumulado_resultado = Convert.ToDouble(rubrosRow["acumulado_resultado"]);
                rubro.ejercicio_resultado = Convert.ToDouble(rubrosRow["ejercicio_resultado"]);
                rubro.enero_monto_resultado = Convert.ToDouble(rubrosRow["enero_monto_resultado"]);
                rubro.febrero_monto_resultado = Convert.ToDouble(rubrosRow["febrero_monto_resultado"]);
                rubro.marzo_monto_resultado = Convert.ToDouble(rubrosRow["marzo_monto_resultado"]);
                rubro.abril_monto_resultado = Convert.ToDouble(rubrosRow["abril_monto_resultado"]);
                rubro.mayo_monto_resultado = Convert.ToDouble(rubrosRow["mayo_monto_resultado"]);
                rubro.junio_monto_resultado = Convert.ToDouble(rubrosRow["junio_monto_resultado"]);
                rubro.julio_monto_resultado = Convert.ToDouble(rubrosRow["julio_monto_resultado"]);
                rubro.agosto_monto_resultado = Convert.ToDouble(rubrosRow["agosto_monto_resultado"]);
                rubro.septiembre_monto_resultado = Convert.ToDouble(rubrosRow["septiembre_monto_resultado"]);
                rubro.octubre_monto_resultado = Convert.ToDouble(rubrosRow["octubre_monto_resultado"]);
                rubro.noviembre_monto_resultado = Convert.ToDouble(rubrosRow["noviembre_monto_resultado"]);
                rubro.diciembre_monto_resultado = Convert.ToDouble(rubrosRow["diciembre_monto_resultado"]);
                listaRubros.Add(rubro);
            }

            return listaRubros;
        }

        public IActionResult buildProformaToExcel(List<ProformaDetalle> detalles)
        {
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {

                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
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
                int count = 2;

                foreach (ProformaDetalle detalle in detalles)

                {
                    DataTable dt2 = _queryExecuter.ExecuteQuery( "SELECT * FROM rubro WHERE id = " + detalle.rubro_id);
                    

                    foreach (DataRow d in dt2.Rows)
                    {
                        workSheet.Cells[count, 1].Value = Convert.ToString(d["nombre"]);
                        detalle.hijos = Convert.ToString(d["hijos"]);
                        detalle.aritmetica = Convert.ToString(d["aritmetica"]);
                    }

                    List<ProformaDetalle> realprof = convierte(detalle);

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

        public List<ProformaDetalle> convierte(ProformaDetalle detalle)
        {
            List<ProformaDetalle> listaRubros = new List<ProformaDetalle>();
            listaRubros.Add(detalle);
            PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(ProformaDetalle));
            DataTable profdetalle = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                profdetalle.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (ProformaDetalle item in listaRubros)
            {
                DataRow row = profdetalle.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                profdetalle.Rows.Add(row);
            }



            return listaRubros;
        }
    }
}
