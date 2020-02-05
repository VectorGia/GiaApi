using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class TipoCaptura
    {
        //public int INT_IDTIPOCAPTURA_P { get; set; }
        //public string STR_DESCRIP_TIPOCAPTURA { get; set; }
        //public int INT_IDUSUARIO_F { get; set; }
        //public DateTime FEC_MODIF_TIPOCAPTURA { get; set; }
        //public bool BOOL_TIPOCAPTURA_ACTIVO { get; set; }

        public Int64 id { get; set; }
        public Boolean activo { get; set; }
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string fec_modif { get; set; }
        public Int64 idusuario { get; set; }

    }
}
