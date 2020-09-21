using System.Collections.Generic;

namespace AppGia.Controllers
{
    public class SeguridadConstantes
    {
        public static List<Dictionary<string, string>> getPermisos()
        {
            List<Dictionary<string, string>> permisos = new List<Dictionary<string, string>>();
            permisos.Add(kv("A", "ALTA"));
            permisos.Add(kv("B", "BAJA"));
            permisos.Add(kv("E", "MODIFICAR"));
            permisos.Add(kv("ALL", "TODOS"));
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
            pantallas.Add(kv("USR", "USUARIOS"));
            pantallas.Add(kv("GRU", "GRUPOS"));
            pantallas.Add(kv("ROL", "ROLES"));
            pantallas.Add(kv("REL", "RELACIONES"));
            pantallas.Add(kv("EMP", "EMPRESAS"));
            pantallas.Add(kv("MOD", "MODELOS"));
            pantallas.Add(kv("PROY", "PROYECTOS"));
            pantallas.Add(kv("CPROF", "CPROFORMAS"));
            pantallas.Add(kv("PROF", "PROFORMAS"));
            pantallas.Add(kv("REP", "REPORTES"));
            pantallas.Add(kv("PER", "PERIODOS"));
            pantallas.Add(kv("EXTR", "EXRACCION"));

            return pantallas;
        }
    }
}