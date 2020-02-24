using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class CentroCostos
    {

        public Int64 id { get; set; }
        public Int64 proyecto_id { get; set; }
        public Int64 empresa_id { get; set; }
        public string desc_id { get; set; }
        public bool activo { get; set; }
        public string estatus { get; set; }
        public string nombre { get; set; }
        public string gerente { get; set; }
        public string tipo { get; set; }
        public string categoria { get; set; }
        public DateTime fecha_modificacion { get; set; }

        /// <summary>
        /// Entidades nuevas para el innerjoin
        /// </summary>
        public string nombre_empresa { get; set; }
        public string nombre_proyecto { get; set; }
        public Int64 modelo_negocio_id { get; set; }
        public Int64 modelo_negocio_flujo_id { get; set; }
        public double porcentaje { get; set; }
        public string proyeccion { get; set; }
    }
}
