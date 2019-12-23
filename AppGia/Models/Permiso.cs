using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Permiso
    {
        public string STR_NOMBRE_PERMISO { get; set; }
        public int INT_IDPERMISO_P { get; set; }
        public int INT_IDROL { get; set; }
        public bool BOOL_ESTATUS_LOGICO_PERM { get; set; }
    }
}
