﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Proceso
    {
        public Int64 id { set; get; }
        public string empresa { set; get; }
        public string estatus { set; get; }
        public DateTime fecha_fin { set; get; }
        public DateTime fecha_inicio { set; get; }
        public string mensaje { set; get; }
        public string tipo { set; get; }
        public Int64 id_empresa { get; set; }
        public string modulo { get; set; }
        public Int64 id_etl_prog { get; set; }

    }
}
