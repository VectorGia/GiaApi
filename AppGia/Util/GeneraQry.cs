using System;
using System.Collections.Generic;
using System.Text;
using AppGia.Models;
using static AppGia.Util.Constantes;

namespace AppGia.Util
{
    public class GeneraQry
    {
        private String nombreTablaBalanza;
        private String nombreColumCta;
        private int lengthColumCta;
        private static  string ctasFlujo = "'1006'";


        public GeneraQry(string nombreTablaBalanza, string nombreColumCta, int lengthColumCta)
        {
            this.nombreTablaBalanza = nombreTablaBalanza;
            this.nombreColumCta = nombreColumCta;
            this.lengthColumCta = lengthColumCta;
        }

     
        public String getQuerySums(Rubros rubro, CentroCostos cc, Empresa empresa, Int64 existenRegistros)
        {
            String includes = rubro.rangos_cuentas_incluidas;
            String excludes = rubro.rango_cuentas_excluidas;
            String idEmpresaExternal = empresa.desc_id;
            String idCCExternal = cc.desc_id;
            int multiplicadorSigno = 1;
            if (rubro.naturaleza != null && rubro.naturaleza == NATACREEDORA)
            {
                multiplicadorSigno = -1;
            }
            String query = String.Format("select id_empresa,\n" +
                    "       year,\n" +
                    "       sum(eneabonos) as eneabonos,\n" +
                    "       sum(enecargos) as enecargos,\n" +
                    "       round((sum(eneabonos) + sum(enecargos))) * {0} as enetotal,\n" +
                    "       sum(febabonos) as febabonos,\n" +
                    "       sum(febcargos) as febcargos,\n" +
                    "       round((sum(febabonos) + sum(febcargos))) * {0} as febtotal,\n" +
                    "       sum(marabonos) as marabonos,\n" +
                    "       sum(marcargos) as marcargos,\n" +
                    "       round((sum(marabonos) + sum(marcargos))) * {0} as martotal,\n" +
                    "       sum(abrabonos) as abrabonos,\n" +
                    "       sum(abrcargos) as abrcargos,\n" +
                    "       round((sum(abrabonos) + sum(abrcargos))) * {0} as abrtotal,\n" +
                    "       sum(mayabonos) as mayabonos,\n" +
                    "       sum(maycargos) as maycargos,\n" +
                    "       round((sum(mayabonos) + sum(maycargos))) * {0} as maytotal,\n" +
                    "       sum(junabonos) as junabonos,\n" +
                    "       sum(juncargos) as juncargos,\n" +
                    "       round((sum(junabonos) + sum(juncargos))) * {0} as juntotal,\n" +
                    "       sum(julabonos) as julabonos,\n" +
                    "       sum(julcargos) as julcargos,\n" +
                    "       round((sum(julabonos) + sum(julcargos))) * {0} as jultotal,\n" +
                    "       sum(agoabonos) as agoabonos,\n" +
                    "       sum(agocargos) as agocargos,\n" +
                    "       round((sum(agoabonos) + sum(agocargos))) * {0} as agototal,\n" +
                    "       sum(sepabonos) as sepabonos,\n" +
                    "       sum(sepcargos) as sepcargos,\n" +
                    "       round((sum(sepabonos) + sum(sepcargos))) * {0} as septotal,\n" +
                    "       sum(octabonos) as octabonos,\n" +
                    "       sum(octcargos) as octcargos,\n" +
                    "       round((sum(octabonos) + sum(octcargos))) * {0} as octtotal,\n" +
                    "       sum(novabonos) as novabonos,\n" +
                    "       sum(novcargos) as novcargos,\n" +
                    "       round((sum(novabonos) + sum(novcargos))) * {0} as novtotal,\n" +
                    "       sum(dicabonos) as dicabonos,\n" +
                    "       sum(diccargos) as diccargos,\n" +
                    "       round((sum(dicabonos) + sum(diccargos))) * {0} as dictotal\n" +
                    "from (\n" +
                    getQueryIncludesExcludes(includes, excludes, idEmpresaExternal, idCCExternal,existenRegistros) +
                    "     ) balanza_ctas\n" +
                    "group by id_empresa, year",multiplicadorSigno);
            return query;
        }

        public String getQuerySemanalSums(Rubros rubro, CentroCostos cc,Empresa empresa,  Int64 numRegistrosExistentes)
        {
            String includes=rubro.rangos_cuentas_incluidas; 
            String excludes=rubro.rango_cuentas_excluidas;
            String idEmpresaExternal = empresa.desc_id;
            String idCCExternal = cc.desc_id;
            int multiplicadorSigno = 1;
            if (rubro.naturaleza != null && rubro.naturaleza == NATDEUDORA)
            {
                multiplicadorSigno = -1;
            }
            String query = String.Format("select id_empresa, year, mes,   \n" +
                           "       sum(monto::numeric) * {0} as saldo\n" +
                           "from (\n" +
                           getQueryIncludesExcludes(includes, excludes, idEmpresaExternal, idCCExternal,numRegistrosExistentes) +
                           "     ) semanal_itms\n" +
                           "group by id_empresa, year, mes",multiplicadorSigno);
            return query;
        }
        public String getQueryIncludesExcludes(String includes, String excludes, String idEmpresaExternal, String idCCExternal,Int64 numRegistrosExistentes)
        {
            String query = getQuery(includes, idEmpresaExternal,  idCCExternal,numRegistrosExistentes);
            if (excludes != null && excludes.Length > 0)
            {
                query += " EXCEPT " + getQuery(excludes, idEmpresaExternal,idCCExternal,numRegistrosExistentes);
            }
            return query;
        }
        public String getQuery(String rangesAndCtas, String idEmpresaExternal,String idCCExternal, Int64 numRegistrosExistentes)
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
                }else if (rangeOrCta.EndsWith("*"))
                {
                    rangos.Add(rangeOrCta+"-"+rangeOrCta);
                }
                else
                {
                    cuentas.Add(rangeOrCta.Trim());
                }
            }
            if (rangos.Count > 0 && cuentas.Count > 0)
            {
                return "(" + getQueryCuentas(cuentas, idEmpresaExternal,idCCExternal,numRegistrosExistentes) + " union " + getQueryRangos(rangos, idEmpresaExternal,idCCExternal,numRegistrosExistentes) + ")";
            }
            else if (rangos.Count > 0)
            {
                return "(" + getQueryRangos(rangos, idEmpresaExternal, idCCExternal,numRegistrosExistentes) + ")";
            }
            else if (cuentas.Count > 0)
            {
                return "(" + getQueryCuentas(cuentas, idEmpresaExternal,idCCExternal,numRegistrosExistentes) + ")";
            }
            return "";
        }


        public String getQueryCuentas(List<String> cuentas, String idEmpresaExternal,String idCCExternal, Int64 numRegistrosExistentes)
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
                String filtroCtaFlujo = getFiltroCtaFlujos(nombreTablaBalanza);
                String query;
                if(numRegistrosExistentes == 0)
                {
                    query = String.Format("( select * " +
                        " from " + nombreTablaBalanza +
                        " where " + nombreColumCta + " in ({0}) and id_empresa = {1} and cc='{2}' {3})", sb, idEmpresaExternal,idCCExternal,filtroCtaFlujo);
                }
                else
                {
                    query = String.Format("( select * " +
                        " from " + nombreTablaBalanza +
                        " where " + nombreColumCta + " in ({0}) and id_empresa = {1} and cc='{2}' {3} and year = {4})", sb, idEmpresaExternal,idCCExternal,filtroCtaFlujo,fechaactual.Year);
                }
                return query;
            }

            return "";
        }

        private String getFiltroCtaFlujos(string nombreTabla)
        {
            String filtroCtaFlujo = "";
            if (nombreTabla.ToLower().Equals("semanal"))
            {
                filtroCtaFlujo = String.Format(" and cta in ({0}) ",ctasFlujo);
            }

            return filtroCtaFlujo;
        }
        public String getQueryRangos(List<String> rangos, string idEmpresaExternal,String idCCExternal, Int64 numRegistrosExistentes)
        {
            StringBuilder query = new StringBuilder();
            for (int i = 0; i < rangos.Count; i++)
            {
                if (i + 1 == rangos.Count)
                {
                    query.Append(getQueryRango(rangos[i], idEmpresaExternal, idCCExternal,numRegistrosExistentes));
                }
                else
                {
                    query.Append(getQueryRango(rangos[i], idEmpresaExternal,idCCExternal, numRegistrosExistentes)).Append(" union ");
                }
            }
            if (rangos.Count > 0)
            {
                query.Insert(0, '(').Append(')');
            }
            return query.ToString();
        }

        public String getQueryRango(String rango, String idEmpresaExternal,String idCCExternal, Int64 numRegistrosExistentes)
        {
            String[] rangoData = rango.Split('-');
            String rangoInferior = rangoData[0].Trim();
            String rangoSuperior = rangoData[1].Trim();
            DateTime fechaactual = DateTime.Today;
            
            String filtroCtaFlujo = getFiltroCtaFlujos(nombreTablaBalanza);
            String query;
            if (numRegistrosExistentes == 0)
            {
                query = String.Format("(select *" +
                            " from " + nombreTablaBalanza +
                            " where " + nombreColumCta + "::numeric >= replace(RPAD('{0}', " + lengthColumCta + ", '0'), '*', '0')::numeric " +
                            "   and " + nombreColumCta + "::numeric <= replace(RPAD('{1}', " + lengthColumCta + ", '9'), '*', '9')::numeric and id_empresa = {2} and cc='{3}' {4})\n", 
                    rangoInferior, rangoSuperior,idEmpresaExternal,idCCExternal,filtroCtaFlujo);
            }
            else
            {
                query = String.Format("(select *" +
                            " from " + nombreTablaBalanza +
                            " where " + nombreColumCta + "::numeric >= replace(RPAD('{0}', " + lengthColumCta + ", '0'), '*', '0')::numeric " +
                            "   and " + nombreColumCta + "::numeric <= replace(RPAD('{1}', " + lengthColumCta + ", '9'), '*', '9')::numeric and id_empresa = {2}  and cc='{3}' {4} and year = {5})\n", 
                    rangoInferior, rangoSuperior,idEmpresaExternal,idCCExternal,filtroCtaFlujo,fechaactual.Year);
            }

            return query;
        }
    }
}
