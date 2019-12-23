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
}
