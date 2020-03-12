using AppGia.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Util;

namespace AppGia.Dao
{
    public class TipoCambioGiaDataAccessLayer
    {
        private QueryExecuterSQL _queryExecuter = new QueryExecuterSQL();

        public TipoCambioGiaDataAccessLayer()
        {
            //Constructor
        }

        public DataRow ConsultatipoCambioGia()
        {
            string consulta = string.Empty;
            consulta = "select * from tipo_cambio_gia";
            DataRow TipoCambioRow = _queryExecuter.ExecuteQueryUniqueresultSQL(consulta);
            return TipoCambioRow;
        }

        public DataRow ExecuteQueryUniqueresult(String qry)
        {
            DataTable dataTable = _queryExecuter.ExecuteQuerySQL(qry);
            if (dataTable.Rows.Count == 1)
            {
                return dataTable.Rows[0];
            }
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            throw new DataException("Se esperaba un resultado pero se obtuvieron " + dataTable.Rows.Count);
        }       
    }
}
