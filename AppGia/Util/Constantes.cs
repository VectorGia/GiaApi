﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Util
{
    public static class Constantes 


    {
        public const string EST_EXT_ERR = "Con error";
        public const string EST_EXT_FIN = "Finaliazdo";
        public const string MSJ_CORREO_ETL_BALANZA = "ETL Balanza Programada";
        public const string MODULO_SEMANAL = "semanal";
        public const string MODULO_BALANZA = "balanza";
        public const string TIPO_EXT_MANUAL = "Manual";
        public const string TIPO_EXT_PROGRAMADA = "Programada";
        public const string CSV_PATH_BALANZA = @"C:\GIA\balanza\";
        public const string CSV_PATH_SEMANAL = @"C:\GIA\semanal\";
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
                                                    + "uuid,"
                                                    + "tipo_extraccion,"
                                                    + "fecha_carga," 
                                                    + "hora_carga";

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
                                                 + "incluir_suma,"
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
        
        public const int TipoCapturaContable = 1;
        public const int TipoCapturaFlujo = 2;
        public const int TipoRubroCuentas = 2;
        
        public const string ProyeccionBase = "BASE";
        public const string ProyeccionMetodo = "METODO";
        public const string ProyeccionShadow = "SHADOW";

        public const string ClaveProforma012 = "12";
        
        public const string TIPODETPROFORM = "proform";
        public const string TIPODETPROREAL = "real";
        public const string ESTILODETHIJO = "hijo";
        public const string ESTILODETPADRE = "padre";
        public const string ESTILODETPADREINGRESOS = "padreingresos";

        public const string claveMonedaMex = "MX";
        
        public const string NATACREEDORA = "ACREEDORA";
        public const string NATDEUDORA = "DEUDORA";
        
        

    }
}
