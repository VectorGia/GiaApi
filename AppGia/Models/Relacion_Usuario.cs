using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Relacion_Usuario
    {
        public string usuario { set; get; }
        public string grupo { set; get; }
        public string rol { set; get; }

        public int SERIAL_IDRELACIONUSU_P { set; get; }
        public int INT_IDUSUARIO_P { set; get; }
        public int INT_IDGRUPO_P { set; get; }
        public int INT_IDROL_P { set; get; }
        public string pantalla { set; get; }
        public string permiso { set; get; }
        public bool BOOL_ESTATUS_LOGICO_RELUSU { set; get; }
        public DateTime FEC_MODIF_RELUSU { set; get; }
    }
}