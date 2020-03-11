using System.Data;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AppGia.Controllers
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
            workSheet.Row(1).Style.Font.Color.SetColor(Color.White);
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.Blue);
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}