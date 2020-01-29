using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Etl_Estatus
    {
        public int id { set; get; }
        public string descripcion { set; get; }
        public bool activo { set; get; }
        public DateTime fech_modificacion { set; get; }
    }
}
