using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AppGia.Models
{
    public class Login
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
    }

    public class User
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string displayName { get; set; }

    }

    public class Response
    {

        public bool Message { set; get; }
    }

    public class CentroCostos
    {
        [Key]
        public string id_cc { get; set; }
        [Required]
        public string name_cc { get; set; }
        [Required]
        public string categoria { get; set; }
        [Required]
        public string estatus { get; set; }
        [Required]
        public string gerente { get; set; }
        [Required]
        public string id_empresa { get; set; }
        [Required]
        public string id_proyecto { get; set; }
       
        
    }
}
