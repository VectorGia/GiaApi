using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Moneda
    {
        public int INT_IDMONEDA { get; set; }

        public string STR_DESCRIPCION { get; set; }

        public string STR_CLAVEDESC { get; set; }

        public string STR_PAIS { get; set; }

        public bool BOOL_ESTATUS_MONEDA { get; set; }
    }
}
