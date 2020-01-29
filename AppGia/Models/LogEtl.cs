using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class LogEtl
    {
        public int INT_IDLOGETL_P { set; get; }
        public DateTime FEC_ETL { set; get; }
        public int INT_IDUSUARIO { set; get; }
        public int INT_IDESTATUSETL_P { set; get; }
        //public int INT_IDBALANZA { set; get; }
        public int INT_TIPOETL { set; get; }
    }
}
