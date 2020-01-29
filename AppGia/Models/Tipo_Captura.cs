using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Tipo_Captura
    {
        public int id { set; get; }
        public bool activo { set; get; }
        public string clave { set; get; }
        public string descripcion { set; get; }
    }
}
