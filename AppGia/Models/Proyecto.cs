using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Proyecto
    {
        public string STR_NOMBRE_PROYECTO { get; set; }

        public string STR_RESPONSABLE { get; set; }

        public string STR_IDPROYECTO { get; set; }

        public bool BOOL_ESTATUS_PROYECTO { get; set; }

        public bool BOOL_ESTATUS_LOGICO_PROYECTO { get; set; }

        public DateTime FEC_MODIF { get; set; }

        public int INT_IDPROYECTO_P { get; set; }


    }
}
