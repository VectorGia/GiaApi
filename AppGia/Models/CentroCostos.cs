using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class CentroCostos
    {

        public int id { get; set; }
        public string desc_id { get; set; }
        public bool activo { get; set; }
        public bool estatus { get; set; }
        public string nombre { get; set; }
        public string gerente { get; set; }
        public string tipo { get; set; }
        public string categoria { get; set; }
        public DateTime fech_modificacion { get; set; }

    }
}
