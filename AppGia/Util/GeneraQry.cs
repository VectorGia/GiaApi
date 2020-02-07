using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGia.Util
{
    public class GeneraQry
    {
        public static String NOMBRE_TABLA_BALANZA = "balanza";
        public static String NOMBRE_COLUM_CTA = "cuenta_unificada";
        public static int LENGTH_COLUM_CTA = 12;


        //public static void main(String[] args)
        //{
        //    String query = new Parser().getQueryTotal("1000*-3000*", "300000000000");
        //    System.out.println(query);
        //}
        public String getQuerySums(String includes, String excludes, Int64 empresa_id)
        {
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
                    getQueryIncludesExcludes(includes, excludes, empresa_id) +
                    "     ) balanza_ctas\n" +
                    "group by id_empresa, year";
            return query;
        }
        public String getQueryIncludesExcludes(String includes, String excludes, Int64 empresa_id)
        {
            String query = getQuery(includes, empresa_id);
            if (excludes != null && excludes.Length > 0)
            {
                query += " EXCEPT " + getQuery(excludes, empresa_id);
            }
            return query;
        }
        public String getQuery(String rangesAndCtas, Int64 empresa_id)
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
                return "(" + getQueryCuentas(cuentas, empresa_id) + " union " + getQueryRangos(rangos, empresa_id) + ")";
            }
            else if (rangos.Count > 0)
            {
                return "(" + getQueryRangos(rangos, empresa_id) + ")";
            }
            else if (cuentas.Count > 0)
            {
                return "(" + getQueryCuentas(cuentas, empresa_id) + ")";
            }
            return "";
        }


        public String getQueryCuentas(List<String> cuentas, Int64 empresa_id)
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
                String query = String.Format("( select * " +
                        " from " + NOMBRE_TABLA_BALANZA +
                        " where " + NOMBRE_COLUM_CTA + " in ({0}) and id_empresa = {1} )", sb.ToString(), empresa_id);
                return query;
            }

            return "";
        }

        public String getQueryRangos(List<String> rangos, Int64 empresa_id)
        {
            StringBuilder query = new StringBuilder();
            for (int i = 0; i < rangos.Count; i++)
            {
                if (i + 1 == rangos.Count)
                {
                    query.Append(getQueryRango(rangos[i], empresa_id));
                }
                else
                {
                    query.Append(getQueryRango(rangos[i], empresa_id)).Append(" union ");
                }
            }
            if (rangos.Count > 0)
            {
                query.Insert(0, '(').Append(')');
            }
            return query.ToString();
        }

        public String getQueryRango(String rango, Int64 empresa_id)
        {
            String[] rangoData = rango.Split('-');
            String rangoInferior = rangoData[0].Trim();
            String rangoSuperior = rangoData[1].Trim();
            DateTime fechaactual = DateTime.Today;

            String query =
                    String.Format("(select *" +
                            " from " + NOMBRE_TABLA_BALANZA +
                            " where " + NOMBRE_COLUM_CTA + "::numeric >= replace(RPAD('{0}', " + LENGTH_COLUM_CTA + ", '0'), '*', '0')::numeric " +
                            "   and " + NOMBRE_COLUM_CTA + "::numeric <= replace(RPAD('{1}', " + LENGTH_COLUM_CTA + ", '9'), '*', '9')::numeric and id_empresa = " + empresa_id + ")\n", rangoInferior, rangoSuperior);

            return query;
        }
    }
}
