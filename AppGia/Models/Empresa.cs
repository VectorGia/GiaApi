﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Empresa
    {

        public Int64 id { get; set; }
        public int id_modelo_neg { get; set; }
        public int id_centro_costo { get; set; }
        public int moneda_id { get; set; }
        public bool activo { get; set; }
        public bool estatus { get; set; }
        public bool etl { get; set; }
        public string nombre { get; set; }
        public string desc_id { get; set; }
        public string abrev { get; set; } 
        public string bd_name { get; set; }
        public string contrasenia_etl { get; set; }
        public DateTime fec_modif { get; set; }
        public string host { get; set; }
        public int puerto_compania { get; set; }
        public string usuario_etl { get; set; }
        public byte[] contra_bytes { get; set; }
        public byte[] llave { get; set; }
        public byte[] apuntador { get; set; }

    
    }
}
