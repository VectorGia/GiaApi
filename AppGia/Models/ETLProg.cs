using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class ETLProg
    {
        public Int64 id { set; get; }
        public string fecha_extraccion { set; get; }
        public string hora_extraccion { set; get; }
        public Int64 id_empresa { get; set; }
        public string modulo { get; set; }
        public int anio_inicio {get;set;}
        public int anio_fin { get; set; }
        public bool activo { get; set; }
    }
}
