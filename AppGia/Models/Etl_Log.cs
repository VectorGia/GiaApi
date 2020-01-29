using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Etl_Log
    {
        public int id_etl_log { set; get; }
        public int usuario_id { set; get; }
        public int estatus_etl_id { set; get; }
        public int balanza_id { set; get; }
        public int etl_tipo { set; get; }
        public DateTime etl_fec { set; get; }

    }
}
