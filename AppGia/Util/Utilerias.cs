using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace AppGia.Util
{
    public class utilerias
    {
        public utilerias()
        {
            //Constructor
        }

        public string encriptar(string cadena)
        {
            HashAlgorithm hashAlgorithm = (HashAlgorithm)new SHA512Managed();
            for (int index = 1; index <= 5; ++index)
                cadena = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(cadena + 5)));
            hashAlgorithm.Clear();
            return cadena;
        }
    }
}