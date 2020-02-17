using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Util
{
    public static class Constantes


    {
        public const string NOMBRE_ARCHIVO_POL_SEM = "MovPolizaSemanalExport";
        public const string NOMBRE_ARCHIVO_BALANZA = "BalanzaExport";
        public const string HEADER_SEMANAL_CSV = "year,"
                                                    + "mes,"
                                                    + "poliza,"
                                                    + "tp,"
                                                    + "linea,"
                                                    + "cta,"
                                                    + "scta,"
                                                    + "sscta,"
                                                    + "concepto,"
                                                    + "monto,"
                                                    + "folio_imp,"
                                                    + "itm,"
                                                    + "tm,"
                                                    + "numpro,"
                                                    + "cc,"
                                                    + "referencia,"
                                                    + "orden_compra,"
                                                    + "fechapol,"
                                                    + "id_empresa,"
                                                    + "id_version,"
                                                    + "cfd_ruta_pdf,"
                                                    + "cfd_ruta_xml,"
                                                    + "uuid";

        public const string HEADER_BALANZA_CSV = "cta,"
                                                 + "scta,"
                                                 + "sscta,"
                                                 + "year,"
                                                 + "salini,"
                                                 + "enecargos,"
                                                 + "eneabonos,"
                                                 + "febcargos,"
                                                 + "febabonos,"
                                                 + "marcargos,"
                                                 + "marabonos,"
                                                 + "abrcargos,"
                                                 + "abrabonos,"
                                                 + "maycargos,"
                                                 + "mayabonos,"
                                                 + "juncargos,"
                                                 + "junabonos,"
                                                 + "julcargos,"
                                                 + "julabonos,"
                                                 + "agocargos,"
                                                 + "agoabonos,"
                                                 + "sepcargos,"
                                                 + "sepabonos,"
                                                 + "octcargos,"
                                                 + "octabonos,"
                                                 + "novcargos,"
                                                 + "novabonos,"
                                                 + "diccargos,"
                                                 + "dicabonos,"
                                                 + "tipo_extraccion,"
                                                 + "id_empresa,"
                                                 + "cierre_cargos,"
                                                 + "cierre_abonos,"
                                                 + "acta,"
                                                 + "cc,"
                                                 + "hora_carga,"
                                                 + "fecha_carga";

        public const int EXTRACCION_MANUAL = 1;
        public const int EXTRACCION_PROGRAMADA = 2; 
          
    }
}
