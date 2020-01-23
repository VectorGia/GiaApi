using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class TipoCaptura
    {
        public int INT_IDTIPOCAPTURA_P { get; set; }
        public string STR_DESCRIP_TIPOCAPTURA { get; set; }
        public int INT_IDUSUARIO_F { get; set; }
        public DateTime FEC_MODIF_TIPOCAPTURA { get; set; }
        public bool BOOL_TIPOCAPTURA_ACTIVO { get; set; }
    }
}
