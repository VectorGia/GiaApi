using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class Usuario
    {

        public string user_name { get; set; }

        public string display_name { get; set; }

        public int id { get; set; }

        public string user_name_interno { get; set; }

        public string password{ get; set; }

        public string email { get; set; }

        public bool estatus { get; set; }

        public string puesto { get; set; }

        public DateTime fech_modificacion { get; set; }

        public string nombre { get; set; }

    }
}
