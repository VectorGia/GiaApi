using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class TipoCambio
    {
        public int INT_ID_TIPOCAMBIO_P { get; set; }
        public Double DBL_TIPOCAMBIO_OFICIAL { get; set; }
        public int INT_ID_MONEDA_F { get; set; }
        public DateTime FEC_MODIF_TIPOCAMBIO { get; set; }
        public DateTime DAT__TIPOCAMBIO { get; set; }
        public bool BOOL_ESTATUS_TIPOCAMBIO { get; set; }
    }
}
