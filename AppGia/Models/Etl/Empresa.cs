﻿using System;

namespace AppGia.Models.Etl
{
    public class Empresa
    {
    
        public Int64 id { get; set; }
        public string nombre { get; set; }
        public string bd_name { get; set; }
        public string contrasenia_etl { get; set; }
        public string host { get; set; }
        public int puerto_compania { get; set; }
        public string usuario_etl { get; set; }
        public byte[] contra_bytes { get; set; }
        public byte[] llave { get; set; }
        public byte[] apuntador { get; set; }

    }
}
