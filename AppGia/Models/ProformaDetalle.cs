﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class ProformaDetalle
    {
        // Campos de tabla "proforma_detalle"
        public int id { get; set; }
        public int id_proforma { get; set; }
        public double enero_monto_financiero { get; set; }
        public double enero_monto_resultado { get; set; }
        public double febrero_monto_financiero { get; set; }
        public double febrero_monto_resultado { get; set; }
        public double marzo_monto_financiero { get; set; }
        public double marzo_monto_resultado { get; set; }
        public double abril_monto_financiero { get; set; }
        public double abril_monto_resultado { get; set; }
        public double mayo_monto_financiero { get; set; }
        public double mayo_monto_resultado { get; set; }
        public double junio_monto_financiero { get; set; }
        public double junio_monto_resultado { get; set; }
        public double julio_monto_financiero { get; set; }
        public double julio_monto_resultado { get; set; }
        public double agosto_monto_financiero { get; set; }
        public double agosto_monto_resultado { get; set; }
        public double septiembre_monto_financiero { get; set; }
        public double septiembre_monto_resultado { get; set; }
        public double octubre_monto_financiero { get; set; }
        public double octubre_monto_resultado { get; set; }
        public double noviembre_monto_financiero { get; set; }
        public double noviembre_monto_resultado { get; set; }
        public double diciembre_monto_financiero { get; set; }
        public double diciembre_monto_resultado { get; set; }
        public double acumulado_financiero { get; set; }
        public double acumulado_resultado { get; set; }
        public double valor_tipo_cambio_financiero { get; set; }
        public double valor_tipo_cambio_resultado { get; set; }
        public int rubro_id { get; set; }
        public bool activo { get; set; }
        public double ejercicio_financiero { get; set; }
        public double ejercicio_resultado { get; set; }
        public double total_financiero { get; set; }
        public double total_resultado { get; set; }

        // Campos de tabla "proforma"
        public int anio { get; set; }
        public int modelo_negocio_id { get; set; }
        public int tipo_captura_id { get; set; }
        public int periodo_id { get; set; }
        public DateTime fecha_captura { get; set; }

        // Campos de tabla "rubro"
        public string nombre_rubro { get; set; }

    }
}