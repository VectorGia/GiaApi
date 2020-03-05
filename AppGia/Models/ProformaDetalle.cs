using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class ProformaDetalle : IConceptoProforma
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

        // //HNA: no persistible  Campos de tabla "proforma"
        public int anio { get; set; }
        //HNA: no persistible
        public Int64 modelo_negocio_id { get; set; }
        //HNA: no persistible
        public Int64 tipo_captura_id { get; set; }
        //HNA: no persistible
        public Int64 tipo_proforma_id { get; set; }
        //HNA: no persistible
        public Int64 centro_costo_id { get; set; }
        //HNA: no persistible
        public Int64 usuario { get; set; }

        //HNA: no persistible Campos de tabla "rubro"
        public string nombre_rubro { get; set; }
        //HNA: no persistible
        public string hijos { get; set; }
       
        //HNA: no persistible
        public string clave_rubro { get; set; }
        
        //HNA: no persistible
        public string aritmetica { get; set; }
        public double anios_posteriores_resultado { get; set; }
        
        //HNA: no persistible nos permitira saber desde que mes es proforma y que es real
        public int mes_inicio { get; set; }
        //HNA: no persistible, sirve para saber que campo de ajuste tomat cuando aplique 
        public String campoEnAjustes { get; set; }
        //HNA: no persistible, sirve para identificar detalles antes de que sean guardados
        public Boolean editable { get; set; }
        //HNA: no persistible pero usada para renderear la empresa asociada en pantalla
        public Int64 empresa_id { get; set; }
        
        private string idInternoInternal;
        public String idInterno
        {
            get
            {
                String valor = id > 0 ? id.ToString() : idInternoInternal;
                return valor;
            }
            set { idInternoInternal = value; }
        }



        public string GetHijos()
        {
            return hijos;
        }

        public string GetAritmetica()
        {
            return aritmetica;
        }

        public long GetIdConcepto()
        {
            return rubro_id;
        }
        

    }
}
