using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class CentroCostos
    {

        public string STR_TIPO_CC { get; set; }

        public string STR_IDCENTROCOSTO { get; set; }

        public string STR_NOMBRE_CC { get; set; }

        public string STR_CATEGORIA_CC { get; set; }

        public bool BOOL_ESTATUS_LOGICO_CENTROCOSTO { get; set; }

        public string STR_GERENTE_CC { get; set; }

        public string STR_ESTATUS_CC { get; set; }

        public int INT_IDCENTROCOSTO_P { get; set; }

        public DateTime FEC_MODIF_CC { get; set; }


    }
}
