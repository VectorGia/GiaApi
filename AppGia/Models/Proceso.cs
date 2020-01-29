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
<<<<<<< HEAD
        public string tipo { set; get; }
=======
        public string tipo { set; get; } 
>>>>>>> 23751227726a2594f691d918ce28f772145f1e7e
    }
}
