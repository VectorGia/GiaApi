using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Cuentas
    {        //public int INT_ID_CUENTAS { get; set; }
        //public string CHAR_CTA { get; set; }
        //public string CHAR_SUB_CTA { get; set; }
        //public string CHAR_SUB_SUB_CTA { get; set; }
        //public string TEXT_DESCRIPCION { get; set; }
        //public int INT_ID_COMPANIA_F { get; set; }

        public Int64 id { get; set; }
        public Boolean activo { get; set; }
        public string cta { get; set; }
        public string descripcion { get; set; }
        public int id_companiaf { get; set; }
        public string sub_cta { get; set; }
        public string sub_sub_cta { get; set; }
    }
}
