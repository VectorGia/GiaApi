﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Balanza
    {
        public int INT_IDBALANZA { get; set; }
        public string TEXT_CTA { get; set; }
        public string TEXT_SCTA { get; set; }
        public string TEXT_SSCTA { get; set; }
        public int INT_YEAR { get; set; }
        public double DECI_ENECARGOS { get; set; }
        public double DECI_SALINI { get; set; }
        public double DECI_ENEABONOS { get; set; }
        public double DECI_FEBCARGOS { get; set; }
        public double DECI_FEBABONOS { get; set; }
        public double DECI_MARCARGOS { get; set; }
        public double DECI_MARABONOS { get; set; }
        public double DECI_ABRCARGOS { get; set; }
        public double DECI_ABRABONOS { get; set; }
        public double DECI_MAYCARGOS { get; set; }
        public double DECI_MAYABONOS { get; set; }
        public double DECI_JUNCARGOS { get; set; }
        public double DECI_JUNABONOS { get; set; }
        public double DECI_JULCARGOS { get; set; }
        public double DECI_JULABONOS { get; set; }
        public double DECI_AGOCARGOS { get; set; }
        public double DECI_AGOABONOS { get; set; }
        public double DECI_SEPCARGOS { get; set; }
        public double DECI_SEPABONOS { get; set; }
        public double DECI_OCTCARGOS { get; set; }
        public double DECI_OCTABONOS { get; set; } 
        public double DECI_NOVCARGOS { get; set; } 
        public double DECI_NOVABONOS { get; set; }
        public double DECI_DICCARGOS { get; set; }
        public double DECI_DICABONOS { get; set; }
        public int INT_CC { get; set; }
        public string TEXT_DESCRIPCION { get; set; }
        public string TEXT_DESCRIPCION2 { get; set; }
        public int INT_INCLUIR_SUMA { get; set; }
    }
}
