using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Grupo
    {
        public int INT_IDGRUPO_P { get; set; }

        public string STR_NOMBRE_GRUPO { get; set; }

        public bool BOOL_ESTATUS_LOGICO_GRUPO { get; set; }

        public DateTime FEC_MODIF_GRUPO { get; set; }
    }
}
