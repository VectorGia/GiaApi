using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Relacion
    {
        public int INT_IDUSUARIO_P { set; get; }
        public int INT_IDGRUPO_P { set; get; }
        public int INT_IDROL_P { set; get; }
        public DateTime FEC_MODIF_RELACIONES { set; get; }
        public bool BOOL_ESTATUS_RELACION { set; get; }
        public int INT_IDRELACION_P { set; get; }


    }
}
