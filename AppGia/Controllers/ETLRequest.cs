using System;
using System.Collections.Generic;
using System.Data;
using static System.Convert;
using static System.DateTime;
using static AppGia.Util.Constantes;

namespace AppGia.Helpers
{
    public class ETLRequest
    {
        public int anioInicio { get; set; }
        public int anioFin { get; set; }
        public  int mes { get; set; }
        
    }
}