using System.Collections.Generic;

namespace AppGia.Controllers
{
    public class SeguridadConstantes
    {
        public static List<Dictionary<string, string>> getPermisos()
        {
            List<Dictionary<string, string>> permisos = new List<Dictionary<string, string>>();
            permisos.Add(kv("ALTA", "ALTA"));
            permisos.Add(kv("BAJA", "BAJA"));
            permisos.Add(kv("MODIF", "MODIFICAR"));
            permisos.Add(kv("TODOS", "TODOS"));
            return permisos;
        }

        private static Dictionary<string, string> kv(string key, string value)
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv.Add("clave", key);
            kv.Add("valor", value);
            return kv;
        }

        public static List<Dictionary<string, string>> getPantallas()
        {
            List<Dictionary<string, string>> pantallas = new List<Dictionary<string, string>>();
            pantallas.Add(kv("USUARIOS", "USUARIOS"));
            pantallas.Add(kv("GRUPOS", "GRUPOS"));
            pantallas.Add(kv("ROLES", "ROLES"));
            pantallas.Add(kv("RELACIONES", "RELACIONES"));
            pantallas.Add(kv("RELACIONES-ROL", "RELACIONES ROL"));
            pantallas.Add(kv("USUARIO_EMPR_CENTRO", "USUARIO EMPRESA CENTRO"));
            
            pantallas.Add(kv("EMPRESAS", "EMPRESAS"));
            pantallas.Add(kv("MODELOS", "MODELOS"));
            pantallas.Add(kv("PROYECTOS", "PROYECTOS"));
            
            pantallas.Add(kv("CON PROFORMAS", "CONSULTA DE PROFORMAS"));
            pantallas.Add(kv("PROFORMAS", "PROFORMAS"));
            pantallas.Add(kv("REPORTES", "REPORTES"));
            pantallas.Add(kv("PERIODOS", "PERIODOS"));
            pantallas.Add(kv("EXRACCION", "EXRACCION"));

            return pantallas;
        }
    }
}