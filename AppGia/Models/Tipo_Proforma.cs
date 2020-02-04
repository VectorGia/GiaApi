using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Tipo_Proforma
    {
        public Int64 id { get; set; }
        public Boolean activo { get; set; }
        public string nombre { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string fec_modif { get; set; }
        public Int64 idusuario { get; set; }
        public int mes_inicio { get; set; }
    }
}
