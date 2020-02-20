using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class UnidadNegocio
    {
        public Int64 id { get; set; }
        public int clave { get; set; }
        public string descripcion { get; set; }
        public Int64 idusuario { get; set; }
        public string fec_modif { get; set; }
        public Boolean activo { get; set; }

    }
}
