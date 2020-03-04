using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Periodo
    {
        public Int64 id { get; set; }
        public Boolean activo { get; set; }
        public int anio_periodo { get; set; }
        public Boolean estatus { get; set; }
        public DateTime fec_modif { get; set; }
        public Int64 idusuario { get; set; }
        public Int64 tipo_captura_id { get; set; }
        public Int64 tipo_proforma_id { get; set; }
        
    }
}
