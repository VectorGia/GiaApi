using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class TipoCambioGia
    {
        public TipoCambioGia()
        {
            //Constructor
        }

        public int id { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
        public string monedaid { get; set; }
        public string tipo { get; set; }
        public string monedareporte { get; set; }
        public string monedainforme { get; set; }
        public string usuario { get; set; }
        public string fecharegistro { get; set; }
    }
}
