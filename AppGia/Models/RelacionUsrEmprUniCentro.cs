using System;

namespace AppGia.Models
{
    public class RelacionUsrEmprUniCentro
    {
        public int id { get; set; }
        public Boolean activo { get; set; }
        public Int32 id_usuario { get; set; }
        public Int64 id_empresa { get; set; }
        public Int64 id_unidad { get; set; }
        public Int64 id_centrocosto { get; set; }

        public string user_name { get; set; }
        public string empresa { get; set; }
        public string unidad { get; set; }
        public string centro { get; set; }
    }
}