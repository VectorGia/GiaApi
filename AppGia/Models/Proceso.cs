using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Proceso
    {
        public int id { set; get; }
        public string empresa { set; get; }
        public bool estatus { set; get; }
        public DateTime fecha_fin { set; get; }
        public DateTime fecha_inicio { set; get; }
        public string mensaje { set; get; }
        public string tipo { set; get; }

    }
}
