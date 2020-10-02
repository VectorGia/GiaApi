using System;

namespace AppGia.Models
{
    public class RelacionRol
    {
        public Int64 id { set; get; }
        public Int64 idRol { set; get; }
        public string nombreRol { set; get; }
        public Boolean estatus { set; get; }
        public DateTime fecModif { set; get; }
        public string pantalla { set; get; }
        public string permiso { set; get; }
    }
}