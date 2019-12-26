using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Relacion_Usuario
    {
        public int SERIAL_IDRELACIONUSU_P { set; get; }
        public int INT_IDUSUARIO_P { set; get; }
        public int INT_IDGRUPO_P { set; get; }
        public int INT_IDROL_P { set; get; }
        public int INT_IDPANTALLA_P { set; get; }
        public int INT_IDPERMISO_P { set; get; }
        public bool BOOL_ESTATUS_LOGICO_RELUSU { set; get; }
        public DateTime FEC_MODIF_RELUSU { set; get; }

    }
}
