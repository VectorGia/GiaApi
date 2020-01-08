using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class RelacionModeloCta
    {
        public int INT_IDREINT_IDRELMODCTALACION_COMPANIA { get; set; }
        public string STR_CONCEPTO { get; set; }
        public string STR_CTA_INICIO { get; set; }
        public string STR_CTA_FIN { get; set; }
        public int INT_NATURALEZA { get; set; }
        public int INT_IDMODELO { get; set; }
        public int INT_TIPO { get; set; }
        public bool BOOL_ESTATUS_LOGICO { get; set; }
        public DateTime FECH_MODIF_RELMODELO { get; set; }
    }
}
