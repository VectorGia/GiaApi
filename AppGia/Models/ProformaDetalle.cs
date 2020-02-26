using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class ProformaDetalle
    {
        public object this[string propertyName]
        {
            get { return GetType().GetProperty(propertyName).GetValue(this, null); }
            set { GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
        // Campos de tabla "proforma_detalle"
        public Int64 id { get; set; }
        public Int64 id_proforma { get; set; }
        public double enero_monto_resultado { get; set; }
        public double febrero_monto_resultado { get; set; }
        public double marzo_monto_resultado { get; set; }
        public double abril_monto_resultado { get; set; }
        public double mayo_monto_resultado { get; set; }
        public double junio_monto_resultado { get; set; }
        public double julio_monto_resultado { get; set; }
        public double agosto_monto_resultado { get; set; }
        public double septiembre_monto_resultado { get; set; }
        public double octubre_monto_resultado { get; set; }
        public double noviembre_monto_resultado { get; set; }
        public double diciembre_monto_resultado { get; set; }
        
        public double acumulado_resultado { get; set; }
        public double valor_tipo_cambio_resultado { get; set; }
        public Int64 rubro_id { get; set; }
        public bool activo { get; set; }
        public double ejercicio_resultado { get; set; }
        public double total_resultado { get; set; }

        // Campos de tabla "proforma"
        public int anio { get; set; }
        public Int64 modelo_negocio_id { get; set; }
        public Int64 tipo_captura_id { get; set; }
        public Int64 tipo_proforma_id { get; set; }
        public Int64 centro_costo_id { get; set; }
        public Int64 periodo_id { get; set; }
        public DateTime fecha_captura { get; set; }
        public Int64 usuario { get; set; }

        // Campos de tabla "rubro"
        public string nombre_rubro { get; set; }
        public string hijos { get; set; }
        public string clave_rubro { get; set; }

        public string aritmetica { get; set; }
        public double anios_posteriores_resultado { get; set; }
        //HNA: no persistible nos permitira saber desde que mes es proforma y que es real
        public int mes_inicio { get; set; }
        //HNA: no persistible, sirve para saber que campo de ajuste tomat cuando aplique 
        public String campoEnAjustes { get; set; }

    }
}
