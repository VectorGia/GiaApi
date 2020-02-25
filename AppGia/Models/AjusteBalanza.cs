using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class AjusteBalanza
    {
        public AjusteBalanza()
        {
            //Constructor
        }

        public int id_ajuste { get; set; }
        public int id_registro { get; set; }
        public int empresa { get; set; }
        public string centrocosto { get; set; }
        public string razonS { get; set; }
        public string ingreso { get; set; }
        public string directo { get; set; }
        public string indirecto { get; set; }
        public string general { get; set; }
        public string financiero { get; set; }
        public string fiscal { get; set; }
        public string otros { get; set; }
        public int mes { get; set; }
        public int anio { get; set; }
        public string fechacarga { get; set; }
        public string usuario { get; set; }
        public string ofna_asociada { get; set; }
        public string inversiones { get; set; }
        public string dividendos { get; set; }
        public string razonajuste { get; set; }
        public int esmetodo { get; set; }
        public int capitalizaciones { get; set; }
        public int clientes { get; set; }
        public int Oene { get; set; }
        public int anticipoproveedores { get; set; }
        public int anticipoclientes { get; set; }
        public int fondogarantia { get; set; }
        public int proveedores { get; set; }
        public int provisiones { get; set; }
    }    
}
