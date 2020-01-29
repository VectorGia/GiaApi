using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Empresa_Proyecto
    {
        public int id { get; set; }

        public bool activo { get; set; }

        public int empresa_id { get; set; }

        public int proyecto_id { get; set; }

    }
}
