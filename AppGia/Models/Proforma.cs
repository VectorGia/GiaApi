using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
	public class Proforma
	{
		public Int64 id { get; set; }
		public int anio { get; set; }
		public Int64 usuario { get; set; }
		public Int64 modelo_negocio_id { get; set; }
		public Int64 tipo_captura_id { get; set; }
		public Int64 tipo_proforma_id { get; set; }
		public Int64 centro_costo_id { get; set; }
		public bool activo { get; set; }
		public DateTime fecha_captura { get; set; }
		
		public DateTime fecha_actualizacion { get; set; }//agregar este dato a la bd
		public string nombre_proforma { get; set; }

	}
}
