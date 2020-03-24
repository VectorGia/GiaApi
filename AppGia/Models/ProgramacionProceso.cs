using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Models
{
    public class ProgramacionProceso
    {
        public ProgramacionProceso() { }


        public ProgramacionProceso(string clave, string descripcion, string cronExpresion, long idusuario)
        {
            this.clave = clave;
            this.descripcion = descripcion;
            this.cronExpresion = cronExpresion;
            this.idusuario = idusuario;
        }

        public string clave { get; set; }
        public string descripcion { get; set; }
        public string cronExpresion { get; set; }
        public Int64 idusuario { get; set; }
    }
}
