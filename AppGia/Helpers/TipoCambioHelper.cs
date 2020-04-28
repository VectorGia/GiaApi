using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Controllers;
using AppGia.Models;
using AppGia.Util;
using static System.Convert;
using static System.DateTime;
using static AppGia.Util.Constantes;

namespace AppGia.Helpers
{
    public class TipoCambioHelper
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();
        private QueryExecuterSQL _queryExecuterSql = new QueryExecuterSQL();
        private const string RESULTADO = "ER";
        private const string FINANCIERO = "ESF";

        public Dictionary<string, double> getTiposCambio(Int64 idCC, int anio, Int64 idTipoCaptura)
        {
            if (idTipoCaptura == TipoCapturaContable)
            {
                return getTiposCambioContable(idCC, anio);
            }

            if (idTipoCaptura == TipoCapturaFlujo)
            {
                return getTiposCambioFlujo(idCC, anio);
            }

            return null;
        }

       
        public Dictionary<string, double> getTiposCambioContable(Int64 idCC, int anio)
        {
            Dictionary<string, double> tipoCambio = new Dictionary<string, double>();
            Moneda moneda = findIdMonedaByCentroCosto(idCC);
            if (esMonedaExtrangera(moneda))
            {
                return tipoCambio;
            }
            string query = "select anio,mes,tipo,monedarporte,monedainforme from tipo_cambio_gia" +
                    " where monedaid=" + moneda.id +
                    " and anio= " + anio +
                    " and mes=" + Now.Month;
            DataTable dataTable = _queryExecuterSql.ExecuteQuerySQL(query);
           
            tipoCambio.Add("LOCAL", 1.0);
            foreach (DataRow row in dataTable.Rows)
            {
                Double factorDll = 1 / ToDouble(row["monedarporte"]);
                Double factorPesos = ToDouble(row["monedainforme"]) * factorDll;
                ;
                if (RESULTADO.Equals(row["tipo"].ToString().Trim()))
                {
                    tipoCambio.Add("USD RESUL", factorDll);
                    tipoCambio.Add("MXN RESUL", factorPesos);
                }

                if (FINANCIERO.Equals(row["tipo"].ToString().Trim()))
                {
                    tipoCambio.Add("USD FINAN", factorDll);
                    tipoCambio.Add("MXN FINAN", factorPesos);
                }
            }

            return tipoCambio;
        }

        public Dictionary<string, double> getTiposCambioFlujo(Int64 idCC, int anio)
        {
            Dictionary<string, double> tipoCambio = new Dictionary<string, double>();
            Moneda moneda = findIdMonedaByCentroCosto(idCC);
            if (esMonedaExtrangera(moneda))
            {
                return tipoCambio;
            }
            string query="select fecharegistro,monedareporte,monedainforme " +
                         " from tipo_cambio_flujo_gia " +
                         " where monedaid= " +moneda.id+
                         " and fecharegistro = " +
                         " (select max(fecharegistro) from tipo_cambio_flujo_gia " +
                         "   where monedaid= " +moneda.id+" and fecharegistro > DATEADD(day, -7, '"+Now.ToString("yyyy-MM-dd")+"')" +
                         " )";
 
            DataTable dataTable = _queryExecuterSql.ExecuteQuerySQL(query);
            tipoCambio.Add("LOCAL", 1.0);
            foreach (DataRow row in dataTable.Rows)
            {
                Double factorDll = 1 / ToDouble(row["monedareporte"]);
                Double factorPesos = ToDouble(row["monedainforme"]) * factorDll;
                tipoCambio.Add("USD", factorDll);
                tipoCambio.Add("MXN", factorPesos);
            }
            return tipoCambio;
        }
        private Moneda findIdMonedaByCentroCosto(Int64 idCC)
        {
            string query =
                "select cc.id as idcc,cc.empresa_id as idempresa,m.id  as idmoneda, m.clave  as claveMoneda  " +
                " from centro_costo cc join empresa e on cc.empresa_id = e.id " +
                " join moneda m on e.moneda_id = m.id " +
                " where cc.activo=true and e.activo=true and m.activo=true and cc.id=" + idCC;
            DataRow res = _queryExecuter.ExecuteQueryUniqueresult(query);
            if (res == null)
            {
                throw new DataException("No se encontro una moneda asociado a la empresa del centro de costro '" +
                                        idCC + "'");
            }
            Moneda moneda=new Moneda();
            moneda.id=ToInt64(res["idmoneda"]);
            moneda.clave = res["claveMoneda"].ToString();
            return moneda;
        }

        private bool esMonedaExtrangera(Moneda moneda)
        {
            return !moneda.clave.Equals(Constantes.claveMonedaMex);
        }
    }
}