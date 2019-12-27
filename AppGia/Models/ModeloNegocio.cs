using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class ModeloNegocio
    {
        public int INT_IDMODELONEGOCIO_P { get; set; }
        public string STR_NOMBREMODELONEGOCIO { get; set; }
        public string STR_TIPOMONTO { get; set; }
        public string STR_IDCOMPANIA { get; set; } 
        public string STR_CUENTASMODELO { get; set; }
        public bool BOOL_ESTATUS_LOGICO_MODE_NEGO { get; set; }
        public DateTime FEC_MODIF_MODELONEGOCIO { get; set; }
   }
}
