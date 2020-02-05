using System;

namespace AppGia.Models
{
    public class Proyecto
    {
        public string idsempresas { get; set; }
        public long id { get; set; }
        public string desc_id { get; set; }
        public bool activo { get; set; }
        public string estatus { get; set; } 
        public string nombre { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public DateTime fecha_modificacion { get; set; }
        public string responsable { get; set; }
        public int modelo_negocio_id { get; set; }


        private Empresa empresa;

        public Empresa EMPRESA
        {
            get { return empresa; }
            set { empresa = value; }
        }

        private Empresa_Proyecto empresa_proyecto;

        public Empresa_Proyecto EMPRESA_PROYECTO
        {
            get { return empresa_proyecto; }
            set { empresa_proyecto = value; }
        }

    }
}
