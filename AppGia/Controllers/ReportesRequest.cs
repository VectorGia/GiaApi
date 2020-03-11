﻿using System;
using System.Collections.Generic;
using System.Data;
using static System.Convert;
using static System.DateTime;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ReportesRequest
    {
        public Int64 idReporte { get; set; }
        public Dictionary<string, string> parametros { get; set; }
    }
}