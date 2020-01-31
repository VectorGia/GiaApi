using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
	public class Proforma
	{
		public int id { get; set; }
		public int anio { get; set; }
		public int usuario { get; set; }
		public int modelo_negocio_id { get; set; }
		public int tipo_captura_id { get; set; }
		public int tipo_proforma_id { get; set; }
		public int centro_costo_id { get; set; }
		public bool activo { get; set; }
		public DateTime fecha_captura { get; set; }

	}
}
