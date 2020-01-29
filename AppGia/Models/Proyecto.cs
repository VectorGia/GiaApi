﻿using System;

namespace AppGia.Models
{
    public class Proyecto
    {
        public int id { get; set; }
        public string id_proyecto { get; set; }
        public bool activo { get; set; }
        public bool estatus { get; set; }
        public string nombre { get; set; }
        public DateTime fecha_modificacion { get; set; }
        public string responsable { get; set; }
        public int centro_costo_id { get; set; }


    }
}
