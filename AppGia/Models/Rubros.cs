using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Rubros
    {
        public int id { set; get; }
        public bool activo { set; get; }
        public string nombre { set; get; }
        public string nombreM { set; get; }
        public string aritmetica { set; get; }
        public string clave { set; get; }
        public string naturaleza { set; get; }
        public string rango_cuentas_excluidas { set; get; }
        public string rangos_cuentas_incluidas { set; get; }
        public int tipo_id { set; get; }
        public int id_modelo_neg { set; get; }


    }
}
