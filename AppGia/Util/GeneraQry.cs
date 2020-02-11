﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGia.Util
{
    public class GeneraQry
    {
        private String nombreTablaBalanza;
        private String nombreColumCta;
        private int lengthColumCta;


        public void Parser(String nombreTablaBalanza, String nombreColumCta, int lengthColumCta)
        {
            this.nombreTablaBalanza = nombreTablaBalanza;
            this.nombreColumCta = nombreColumCta;
            this.lengthColumCta = lengthColumCta;
        }
        public String getQuerySums(String includes, String excludes, Int64 empresa_id, String nombreTablaBalanza, String nombreColumCta, int lengthColumCta, Int64 datos)
        {
            Parser(nombreTablaBalanza, nombreColumCta, lengthColumCta);
            String query = "select id_empresa,\n" +
                    "       year,\n" +
                    "       sum(eneabonos) as eneabonos,\n" +
                    "       sum(enecargos) as enecargos,\n" +
                    "       (sum(eneabonos) + sum(enecargos)) as enetotal,\n" +
                    "       sum(febabonos) as febabonos,\n" +
                    "       sum(febcargos) as febcargos,\n" +
                    "       (sum(febabonos) + sum(febcargos)) as febtotal,\n" +
                    "       sum(marabonos) as marabonos,\n" +
                    "       sum(marcargos) as marcargos,\n" +
                    "       (sum(marabonos) + sum(marcargos)) as martotal,\n" +
                    "       sum(abrabonos) as abrabonos,\n" +
                    "       sum(abrcargos) as abrcargos,\n" +
                    "       (sum(abrabonos) + sum(abrcargos)) as abrtotal,\n" +
                    "       sum(mayabonos) as mayabonos,\n" +
                    "       sum(maycargos) as maycargos,\n" +
                    "       (sum(mayabonos) + sum(maycargos)) as maytotal,\n" +
                    "       sum(junabonos) as junabonos,\n" +
                    "       sum(juncargos) as juncargos,\n" +
                    "       (sum(junabonos) + sum(juncargos)) as juntotal,\n" +
                    "       sum(julabonos) as julabonos,\n" +
                    "       sum(julcargos) as julcargos,\n" +
                    "       (sum(julabonos) + sum(julcargos)) as jultotal,\n" +
                    "       sum(agoabonos) as agoabonos,\n" +
                    "       sum(agocargos) as agocargos,\n" +
                    "       (sum(agoabonos) + sum(agocargos)) as agototal,\n" +
                    "       sum(sepabonos) as sepabonos,\n" +
                    "       sum(sepcargos) as sepcargos,\n" +
                    "       (sum(sepabonos) + sum(sepcargos)) as septotal,\n" +
                    "       sum(octabonos) as octabonos,\n" +
                    "       sum(octcargos) as octcargos,\n" +
                    "       (sum(octabonos) + sum(octcargos)) as octtotal,\n" +
                    "       sum(novabonos) as novabonos,\n" +
                    "       sum(novcargos) as novcargos,\n" +
                    "       (sum(novabonos) + sum(novcargos)) as novtotal,\n" +
                    "       sum(dicabonos) as dicabonos,\n" +
                    "       sum(diccargos) as diccargos,\n" +
                    "       (sum(dicabonos) + sum(diccargos)) as dictotal\n" +
                    "from (\n" +
                    getQueryIncludesExcludes(includes, excludes, empresa_id, datos) +
                    "     ) balanza_ctas\n" +
                    "group by id_empresa, year";
            return query;
        }

        public String getQuerySemanalSums(String includes, String excludes, Int64 empresa_id, String nombreTablaBalanza, String nombreColumCta, int lengthColumCta, Int64 datos)
        {
            Parser(nombreTablaBalanza, nombreColumCta, lengthColumCta);
            String query = "select id_empresa, year, mes,   \n" +
                    "       sum(monto::numeric) as saldo\n" +
                    "from (\n" +
                    getQueryIncludesExcludes(includes, excludes, empresa_id, datos) +
                    "     ) semanal_itms\n" +
                    "group by id_empresa, year, mes";
            return query;
        }
        public String getQueryIncludesExcludes(String includes, String excludes, Int64 empresa_id, Int64 datos)
        {
            String query = getQuery(includes, empresa_id, datos);
            if (excludes != null && excludes.Length > 0)
            {
                query += " EXCEPT " + getQuery(excludes, empresa_id,datos);
            }
            return query;
        }
        public String getQuery(String rangesAndCtas, Int64 empresa_id, Int64 datos)
        {
            //String[] arrIncludes = rangesAndCtas.split(",");
            String[] arrIncludes = rangesAndCtas.Split(',');
            List<String> rangos = new List<String>();
            List<String> cuentas = new List<String>();

            foreach (String rangeOrCta in arrIncludes)
            {
                if (rangeOrCta.Contains("-"))
                {
                    rangos.Add(rangeOrCta.Trim());
                }
                else
                {
                    cuentas.Add(rangeOrCta.Trim());
                }
            }
            if (rangos.Count > 0 && cuentas.Count > 0)
            {
                return "(" + getQueryCuentas(cuentas, empresa_id,datos) + " union " + getQueryRangos(rangos, empresa_id,datos) + ")";
            }
            else if (rangos.Count > 0)
            {
                return "(" + getQueryRangos(rangos, empresa_id, datos) + ")";
            }
            else if (cuentas.Count > 0)
            {
                return "(" + getQueryCuentas(cuentas, empresa_id,datos) + ")";
            }
            return "";
        }


        public String getQueryCuentas(List<String> cuentas, Int64 empresa_id, Int64 datos)
        {
            StringBuilder sb = new StringBuilder();
            DateTime fechaactual = DateTime.Today;
            for (int i = 0; i < cuentas.Count; i++)
            {
                if (i + 1 == cuentas.Count)
                {
                    sb.Append('\'').Append(cuentas[i]).Append('\'');
                }
                else
                {
                    sb.Append('\'').Append(cuentas[i]).Append('\'').Append(',');
                }
            }
            if (cuentas.Count > 0)
            {
                String query;
                if(datos == 0)
                {
                    query = String.Format("( select * " +
                        " from " + nombreTablaBalanza +
                        " where " + nombreColumCta + " in ({0}) and id_empresa = {1})", sb.ToString(), empresa_id);
                }
                else
                {
                    query = String.Format("( select * " +
                        " from " + nombreTablaBalanza +
                        " where " + nombreColumCta + " in ({0}) and id_empresa = {1} and year = " + fechaactual.Year + ")", sb.ToString(), empresa_id);
                }
                return query;
            }

            return "";
        }

        public String getQueryRangos(List<String> rangos, Int64 empresa_id, Int64 datos)
        {
            StringBuilder query = new StringBuilder();
            for (int i = 0; i < rangos.Count; i++)
            {
                if (i + 1 == rangos.Count)
                {
                    query.Append(getQueryRango(rangos[i], empresa_id, datos));
                }
                else
                {
                    query.Append(getQueryRango(rangos[i], empresa_id, datos)).Append(" union ");
                }
            }
            if (rangos.Count > 0)
            {
                query.Insert(0, '(').Append(')');
            }
            return query.ToString();
        }

        public String getQueryRango(String rango, Int64 empresa_id, Int64 datos)
        {
            String[] rangoData = rango.Split('-');
            String rangoInferior = rangoData[0].Trim();
            String rangoSuperior = rangoData[1].Trim();
            DateTime fechaactual = DateTime.Today;

            String query;
            if (datos == 0)
            {
                query = String.Format("(select *" +
                            " from " + nombreTablaBalanza +
                            " where " + nombreColumCta + "::numeric >= replace(RPAD('{0}', " + lengthColumCta + ", '0'), '*', '0')::numeric " +
                            "   and " + nombreColumCta + "::numeric <= replace(RPAD('{1}', " + lengthColumCta + ", '9'), '*', '9')::numeric and id_empresa = " + empresa_id + ")\n", rangoInferior, rangoSuperior);
            }
            else
            {
                query = String.Format("(select *" +
                            " from " + nombreTablaBalanza +
                            " where " + nombreColumCta + "::numeric >= replace(RPAD('{0}', " + lengthColumCta + ", '0'), '*', '0')::numeric " +
                            "   and " + nombreColumCta + "::numeric <= replace(RPAD('{1}', " + lengthColumCta + ", '9'), '*', '9')::numeric and id_empresa = " + empresa_id + " and year = " + fechaactual.Year + ")\n", rangoInferior, rangoSuperior);
            }

            return query;
        }
    }
}
