using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class LogEtl
    {
        public int INT_IDLOGETL_P { set; get; }
        public int INT_IDETL_P { set; get; }
        public int INT_IDUSUARIO_P { set; get; }
        public DateTime FEC_ETL { set; get; }
        public int INT_ESTATUS_ETL_P { set; get; }
        public bool BOOL_ESTATUS_LOGICO_LOGETL { set; get; }
        public DateTime FEC_MODIF_LOGETL { set; get; }
    }
}
