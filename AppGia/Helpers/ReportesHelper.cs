using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using AppGia.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static System.Convert;

namespace AppGia.Helpers
{
    public class ReportesHelper
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();

        public byte[] buildReport(ReportesRequest request)
        {
            string queryReporte=_queryExecuter.ExecuteQueryUniqueresult("select contenido from reportes where id=" + request.idReporte)["contenido"].ToString();
            foreach (var parametro in request.parametros)
            {
                string claveReporte = parametro.Key;
                string valorReporte = parametro.Value;

                queryReporte=queryReporte.Replace($"${claveReporte}",$"{valorReporte}");
            }
            
            DataTable dataTable = _queryExecuter.ExecuteQuery(queryReporte);
            var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("Reporte");
            workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);
            ExcelRange rowEncabezado=workSheet.Cells[1,1,1, workSheet.Dimension.End.Column];
            rowEncabezado.Style.Font.Color.SetColor(Color.White);
            rowEncabezado.Style.Font.Bold = true;
            rowEncabezado.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rowEncabezado.Style.Fill.BackgroundColor.SetColor(Color.Blue);
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }

        public List<Dictionary<string, object>> getReportesActivos()
        {
            List<Dictionary<string, object>> reportes=new List<Dictionary<string, object>>();
            DataTable dataTable = _queryExecuter.ExecuteQuery("select id,nombre from reportes where activo=true order by orden");
            foreach (DataRow modeloIdRow in dataTable.Rows)
            {
                Dictionary<string, object> reporte=new Dictionary<string, object>();
                reporte.Add("id",ToInt64(modeloIdRow["id"]));
                reporte.Add("nombre",modeloIdRow["nombre"].ToString());
                reportes.Add(reporte);
            }

            return reportes;
        }
        
        public List<Dictionary<string, object>> getParametrosOf(Int64 idReport)
        {
            List<Dictionary<string, object>> parametros=new List<Dictionary<string, object>>();
            DataTable dataTable = _queryExecuter.ExecuteQuery("select id, nombre, clave, tipo,requerido from reportes_parametros where id_reporte="+idReport);
            foreach (DataRow modeloIdRow in dataTable.Rows)
            {
                Dictionary<string, object> parametro=new Dictionary<string, object>();
                parametro.Add("id",ToInt64(modeloIdRow["id"]));
                parametro.Add("nombre",modeloIdRow["nombre"].ToString());
                parametro.Add("clave", modeloIdRow["clave"].ToString());
                parametro.Add("tipo", modeloIdRow["tipo"].ToString());
                parametro.Add("requerido", ToBoolean(modeloIdRow["requerido"]==null?false:modeloIdRow["requerido"]));
                parametros.Add(parametro);
            }

            return parametros;
        }
    }
}