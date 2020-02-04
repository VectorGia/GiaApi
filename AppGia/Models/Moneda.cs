using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Moneda
    {

        public Int64 id { get; set; }
        public Boolean activo { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string pais { get; set; }
        public string simbolo { get; set; }
     

    }
}
