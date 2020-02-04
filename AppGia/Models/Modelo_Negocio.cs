using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Modelo_Negocio
    {
        public Int64 id { get; set; }
        public bool activo  { get; set; }
        public string nombre { get; set; }
        public string nombrer { get; set; }
        public string rangos_cuentas_incluidas { get; set; }
        public string rango_cuentas_excluidas { get; set; }
        public Int64 tipo_captura_id { get; set; } 


    }
}
