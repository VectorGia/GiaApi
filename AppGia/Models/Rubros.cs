﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Rubros : IConceptoProforma
    {
        public Int64 id { get; set; }
        public bool activo { get; set; }
        public string nombre { get; set; }
        public string aritmetica { get; set; }
        public string clave { get; set; }
        public string hijos { get; set; }
        public string rango_cuentas_excluidas { get; set; }
        public string rangos_cuentas_incluidas { get; set; }
        //sirve para saber que columna en tabla de ajustes se debe tomar 
        public string tipo_cuenta { get; set; }
        public string tipo_agrupador { get; set; }
        public Int64 tipo_id { get; set; }
        public Int64 id_modelo_neg { get; set; }
   
  
        
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
            return id;
        }

    }
}
