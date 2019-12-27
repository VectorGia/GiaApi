using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class RelacionCompania
    {
        public int INT_IDRELACION_COMPANIA { get; set; }
        public int INT_IDCOMPANIA_P { get; set; }
        public int INT_IDMODELO_NEGOCIO_P { get; set; }
        public int INT_IDPROYECTO_P { get; set; }
        public int INT_IDCENTROSCOSTO_P { get; set; }
        public bool BOOL_ESTATUS_LOGICO_RELACION_COMPANIA { get; set; }
        public DateTime FECH_MODIF_RELCOMP { get; set; }
    }
}
