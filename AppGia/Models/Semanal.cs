using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Semanal

    {
        public int  INT_ID_SEMANAL { get; set; }
        public int NUM_YEAR { get; set; }
        public int NUM_MES { get; set; }
        public int NUM_POLIZA { get; set; }
        public string TEXT_TP { get;set; }
        public int NUM_LINEA { get; set; }
        public int NUM_CTA { get; set; }
        public int NUM_SCTA { get; set; }
        public int NUM_SSCTA { get; set; }
        public string TEXT_CONCEPTO { get; set; }
        public string TEXT_MONTO { get; set; }
        public string TEXT_FOLIO_IMP { get; set; }
        public int NUM_ITM { get; set; }
        public int NUM_TM { get; set; }
        public string TEXT_NUMPRO { get; set; }
        public string TEXT_CC { get; set; }
        public string TEXT_REFERENCIA { get; set; }
        public string TEXT_ORDEN_COMPRA { get; set; }
        public string TEXT_FECHAPOL { get; set; }
        public int INT_IDEMPRESA { get; set; }
        public int INT_IDVERSION { get; set; }
        public string TEXT_CFD_RUTA_PDF { get; set; }
        public string TEXT_CFD_RUTA_XML { get; set; }
        public string TEXT_UUID { get; set; }
    }
}
