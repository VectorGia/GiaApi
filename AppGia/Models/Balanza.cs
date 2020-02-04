using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Balanza
    {
        public int id { get; set; } 
        public string cta { get; set; }
        public string scta { get; set; }
        public string sscta { get; set; }
        public int year { get; set; }
        public string concepto { get; set; }
        public double enecargos { get; set; }
        public double salini { get; set; }
        public double eneabonos { get; set; }
        public double febcargos { get; set; }
        public double febabonos { get; set; }
        public double marcargos { get; set; }
        public double marabonos { get; set; }
        public double abrcargos { get; set; }
        public double abrabonos { get; set; }
        public double maycargos { get; set; }
        public double mayabonos { get; set; }
        public double juncargos { get; set; }
        public double junabonos { get; set; }
        public double julcargos { get; set; }
        public double julabonos { get; set; }
        public double agocargos { get; set; }
        public double agoabonos { get; set; }
        public double sepcargos { get; set; }
        public double sepabonos { get; set; }
        public double octcargos { get; set; }
        public double octabonos { get; set; }
        public double novcargos { get; set; }
        public double novabonos { get; set; }
        public double diccargos { get; set; }
        public double dicabonos { get; set; }
        public int incluir_suma { get; set; }
        public int tipo_extraccion { get; set; }
        public string fecha_carga { get; set; }
        public string hora_carga { get; set; }
        public int id_empresa { get; set; }
        public double cierre_cargos { get; set; }
        public double cierre_abonos { get; set; }
        public int acta { get; set; }
        public string cc { get; set; }
        public string cuenta_unificada { get; set; }
    }
}
