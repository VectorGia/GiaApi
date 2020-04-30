using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Modelo_Negocio
    {
        public Int64 id { get; set; }
        public bool activo  { get; set; }
        public string nombre { get; set; }
        public Int64 tipo_captura_id { get; set; }
        public string nombre_tipo_captura { get; set; }
        public List<Int64> unidades_negocio_ids { get; set; }
        
        //permite saber que dos modelos de negocio son parte de uno mismo solo que contable y flujo cada uno
        public string agrupador { get; set; }
    }
}
