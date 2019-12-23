using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class RelacionRol
    {
        public int INT_IDRELACION_P { set; get; }
        public int INT_IDPERMISO_F { set; get; }
        public int INT_IDROL_F { set; get; }
        public DateTime FEC_MODIF_RELA_ROL_PERMISO { set; get; }
        public bool BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO { set; get; }


    }
}
