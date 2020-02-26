using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class TipoCambioHelper
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();
        private QueryExecuterSQL _queryExecuterSql = new QueryExecuterSQL();
        private const string RESULTADO="ER";
        private const string FINANCIERO="ESF";

        public Dictionary<string, double> getTiposCambioContable(Int64 idCC,int anio)
        {
            string query =
                "select cc.id as idcc,cc.empresa_id as idempresa,m.id  as idmoneda " +
                " from centro_costo cc join empresa e on cc.empresa_id = e.id " +
                " join moneda m on e.moneda_id = m.id " +
                " where cc.activo=true and e.activo=true and m.activo=true and cc.id="+idCC;
            DataRow res=_queryExecuter.ExecuteQueryUniqueresult(query);
            if (res == null)
            {
                throw new DataException("No se encontro una moneda asociado a la empresa del centro de costro '"+idCC+"'");
            }
            Int64 idMoneda=ToInt64(res["idmoneda"]);

            query = "select anio,mes,tipo,monedarporte,monedainforme from tipo_cambio_gia" +
                    " where monedaid=" +idMoneda+
                    " and anio= " +anio+
                    " and mes="+ DateTime.Now.Month;
            DataTable dataTable=_queryExecuterSql.ExecuteQuerySQL(query);
            Dictionary<string, double> tipoCambio=new Dictionary<string, double>();
            tipoCambio.Add("LOCAL",1.0);
            foreach (DataRow row in dataTable.Rows)
            {
                Double factorDll = 1 / ToDouble(row["monedarporte"]);
                Double factorPesos =  ToDouble(row["monedarporte"])*factorDll;
;
                if (RESULTADO.Equals(row["tipo"].ToString().Trim()))
                {
                    tipoCambio.Add("DOLARES RESULTADO",factorDll);
                    tipoCambio.Add("PESOS RESULTADO",factorPesos);
                }
                if (FINANCIERO.Equals(row["tipo"].ToString().Trim()))
                {
                    tipoCambio.Add("DOLARES FINANCIERO",factorDll);
                    tipoCambio.Add("PESOS FINANCIERO" ,factorPesos);
                }
            }
            return tipoCambio;
        }

    }
}