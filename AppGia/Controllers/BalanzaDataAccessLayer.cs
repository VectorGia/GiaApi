using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
namespace AppGia.Controllers
{
    public class BalanzaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public BalanzaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        char cod = '"';

        public Balanza GetEncabezado(string cc, int anio)
        {
            string consulta = "select " + cod + "INT_YEAR" + cod + "," + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "STR_CONCEPTO" + cod + ", sum(" + cod + "DECI_SALINI" + cod + ") as " + cod + "DECI_SALINI" + cod + "," +
            "sum(" + cod + "DECI_ENECARGOS" + cod + ") as " + cod + "DECI_ENECARGOS" + cod + ", sum(" + cod + "DECI_ENEABONOS" + cod + ") as " + cod + "DECI_ENEABONOS" + cod + "," +
            "sum(" + cod + "DECI_FEBCARGOS" + cod + ") as " + cod + "DECI_FEBCARGOS" + cod + ", sum(" + cod + "DECI_FEBABONOS" + cod + ") as " + cod + "DECI_FEBABONOS" + cod + "," +
            "sum(" + cod + "DECI_MARCARGOS" + cod + ") as " + cod + "DECI_MARCARGOS" + cod + ", sum(" + cod + "DECI_MARABONOS" + cod + ") as " + cod + "DECI_MARABONOS" + cod + "," +
            "sum(" + cod + "DECI_ABRCARGOS" + cod + ") as " + cod + "DECI_ABRCARGOS" + cod + ", sum(" + cod + "DECI_ABRABONOS" + cod + ") as " + cod + "DECI_ABRABONOS" + cod + "," +
            "sum(" + cod + "DECI_MAYCARGOS" + cod + ") as " + cod + "DECI_MAYCARGOS" + cod + ", sum(" + cod + "DECI_MAYABONOS" + cod + ") as " + cod + "DECI_MAYABONOS" + cod + "," +
            "sum(" + cod + "DECI_JUNCARGOS" + cod + ") as " + cod + "DECI_JUNCARGOS" + cod + ", sum(" + cod + "DECI_JUNABONOS" + cod + ") as " + cod + "DECI_JUNABONOS" + cod + "," +
            "sum(" + cod + "DECI_JULCARGOS" + cod + ") as " + cod + "DECI_JULCARGOS" + cod + ", sum(" + cod + "DECI_JULABONOS" + cod + ") as " + cod + "DECI_JULABONOS" + cod + "," +
            "sum(" + cod + "DECI_AGOCARGOS" + cod + ") as " + cod + "DECI_AGOCARGOS" + cod + ", sum(" + cod + "DECI_AGOABONOS" + cod + ") as " + cod + "DECI_AGOABONOS" + cod + "," +
            "sum(" + cod + "DECI_SEPCARGOS" + cod + ") as " + cod + "DECI_SEPCARGOS" + cod + ", sum(" + cod + "DECI_SEPABONOS" + cod + ") as " + cod + "DECI_SEPABONOS" + cod + "," +
            "sum(" + cod + "DECI_OCTCARGOS" + cod + ") as " + cod + "DECI_OCTCARGOS" + cod + ", sum(" + cod + "DECI_OCTABONOS" + cod + ") as " + cod + "DECI_OCTABONOS" + cod + "," +
            "sum(" + cod + "DECI_NOVCARGOS" + cod + ") as " + cod + "DECI_NOVCARGOS" + cod + ", sum(" + cod + "DECI_NOVABONOS" + cod + ") as " + cod + "DECI_NOVABONOS" + cod + "," +
            "sum(" + cod + "DECI_DICCARGOS" + cod + ") as " + cod + "DECI_DICCARGOS" + cod + ", sum(" + cod + "DECI_DICABONOS" + cod + ") as " + cod + "DECI_DICABONOS" + cod +
            " from " + cod + "TAB_BALANZA" + cod + ", " + cod + "TAB_RELMODELO_CTA" + cod +
            " where " + cod + "TAB_BALANZA" + cod + "." + cod + "TEXT_CC" + cod + " = " + cc +
            " and " + cod + "TAB_BALANZA" + cod + "." + cod + "INT_YEAR" + cod + " = " + anio +
            " and " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "INT_IDRELMODCTA" + cod + " between 5 and 7" +
            " and " + cod + "TAB_BALANZA" + cod + "." + cod + "TEXT_CTA" + cod + " between " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "STR_CTA_INICIO" + cod + " and " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "STR_CTA_FIN" + cod +
            " group by " + cod + "TAB_BALANZA" + cod + "." + cod + "INT_YEAR" + cod + ", " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "STR_CONCEPTO" + cod +
            " union " +
            "select " + cod + "INT_YEAR" + cod + ", 'TOTAL' as " + cod + "STR_CONCEPTO" + cod + ", sum(" + cod + "DECI_SALINI" + cod + ") as " + cod + "DECI_SALINI" + cod + "," +
            "sum(" + cod + "DECI_ENECARGOS" + cod + ") as " + cod + "DECI_ENECARGOS" + cod + ", sum(" + cod + "DECI_ENEABONOS" + cod + ") as " + cod + "DECI_ENEABONOS" + cod + "," +
            "sum(" + cod + "DECI_FEBCARGOS" + cod + ") as " + cod + "DECI_FEBCARGOS" + cod + ", sum(" + cod + "DECI_FEBABONOS" + cod + ") as " + cod + "DECI_FEBABONOS" + cod + "," +
            "sum(" + cod + "DECI_MARCARGOS" + cod + ") as " + cod + "DECI_MARCARGOS" + cod + ", sum(" + cod + "DECI_MARABONOS" + cod + ") as " + cod + "DECI_MARABONOS" + cod + "," +
            "sum(" + cod + "DECI_ABRCARGOS" + cod + ") as " + cod + "DECI_ABRCARGOS" + cod + ", sum(" + cod + "DECI_ABRABONOS" + cod + ") as " + cod + "DECI_ABRABONOS" + cod + "," +
            "sum(" + cod + "DECI_MAYCARGOS" + cod + ") as " + cod + "DECI_MAYCARGOS" + cod + ", sum(" + cod + "DECI_MAYABONOS" + cod + ") as " + cod + "DECI_MAYABONOS" + cod + "," +
            "sum(" + cod + "DECI_JUNCARGOS" + cod + ") as " + cod + "DECI_JUNCARGOS" + cod + ", sum(" + cod + "DECI_JUNABONOS" + cod + ") as " + cod + "DECI_JUNABONOS" + cod + "," +
            "sum(" + cod + "DECI_JULCARGOS" + cod + ") as " + cod + "DECI_JULCARGOS" + cod + ", sum(" + cod + "DECI_JULABONOS" + cod + ") as " + cod + "DECI_JULABONOS" + cod + "," +
            "sum(" + cod + "DECI_AGOCARGOS" + cod + ") as " + cod + "DECI_AGOCARGOS" + cod + ", sum(" + cod + "DECI_AGOABONOS" + cod + ") as " + cod + "DECI_AGOABONOS" + cod + "," +
            "sum(" + cod + "DECI_SEPCARGOS" + cod + ") as " + cod + "DECI_SEPCARGOS" + cod + ", sum(" + cod + "DECI_SEPABONOS" + cod + ") as " + cod + "DECI_SEPABONOS" + cod + "," +
            "sum(" + cod + "DECI_OCTCARGOS" + cod + ") as " + cod + "DECI_OCTCARGOS" + cod + ", sum(" + cod + "DECI_OCTABONOS" + cod + ") as " + cod + "DECI_OCTABONOS" + cod + "," +
            "sum(" + cod + "DECI_NOVCARGOS" + cod + ") as " + cod + "DECI_NOVCARGOS" + cod + ", sum(" + cod + "DECI_NOVABONOS" + cod + ") as " + cod + "DECI_NOVABONOS" + cod + "," +
            "sum(" + cod + "DECI_DICCARGOS" + cod + ") as " + cod + "DECI_DICCARGOS" + cod + ", sum(" + cod + "DECI_DICABONOS" + cod + ") as " + cod + "DECI_DICABONOS" + cod +
            " from " + cod + "TAB_BALANZA" + cod + ", " + cod + "TAB_RELMODELO_CTA" + cod +
            " where " + cod + "TAB_BALANZA" + cod + "." + cod + "TEXT_CC" + cod + " = " + cc +
            " and " + cod + "TAB_BALANZA" + cod + "." + cod + "INT_YEAR" + cod + " = " + anio +
            " and " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "INT_IDRELMODCTA" + cod + " between 5 and 7" +
            " and " + cod + "TAB_BALANZA" + cod + "." + cod + "TEXT_CTA" + cod + " between " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "STR_CTA_INICIO" + cod + " and " + cod + "TAB_RELMODELO_CTA" + cod + "." + cod + "STR_CTA_FIN" + cod +
            " group by " + cod + "INT_YEAR" + cod;
            try
            {
                Balanza balanza = new Balanza();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    balanza.year = Convert.ToInt32(rdr["INT_YEAR"]);
                    balanza.concepto = rdr["STR_CONCEPTO"].ToString().Trim();
                    balanza.salini = Convert.ToInt32(rdr["DECI_SALINI"]);
                    balanza.enecargos = Convert.ToInt32(rdr["DECI_ENECARGOS"]);
                    balanza.eneabonos = Convert.ToInt32(rdr["DECI_ENEABONOS"]);
                    balanza.febcargos = Convert.ToInt32(rdr["DECI_FEBCARGOS"]);
                    balanza.febabonos = Convert.ToInt32(rdr["DECI_FEBABONOS"]);
                    balanza.marcargos = Convert.ToInt32(rdr["DECI_MARCARGOS"]);
                    balanza.marabonos = Convert.ToInt32(rdr["DECI_MARABONOS"]);
                    balanza.abrcargos = Convert.ToInt32(rdr["DECI_ABRCARGOS"]);
                    balanza.abrabonos = Convert.ToInt32(rdr["DECI_ABRABONOS"]);
                    balanza.maycargos = Convert.ToInt32(rdr["DECI_MAYCARGOS"]);
                    balanza.mayabonos = Convert.ToInt32(rdr["DECI_MAYABONOS"]);
                    balanza.juncargos = Convert.ToInt32(rdr["DECI_JUNCARGOS"]);
                    balanza.junabonos = Convert.ToInt32(rdr["DECI_JUNABONOS"]);
                    balanza.julcargos = Convert.ToInt32(rdr["DECI_JULCARGOS"]);
                    balanza.julabonos = Convert.ToInt32(rdr["DECI_JULABONOS"]);
                    balanza.agocargos = Convert.ToInt32(rdr["DECI_AGOCARGOS"]);
                    balanza.agoabonos = Convert.ToInt32(rdr["DECI_AGOABONOS"]);
                    balanza.sepcargos = Convert.ToInt32(rdr["DECI_SEPCARGOS"]);
                    balanza.sepabonos = Convert.ToInt32(rdr["DECI_SEPABONOS"]);
                    balanza.octcargos = Convert.ToInt32(rdr["DECI_OCTCARGOS"]);
                    balanza.octabonos = Convert.ToInt32(rdr["DECI_OCTABONOS"]);
                    balanza.novcargos = Convert.ToInt32(rdr["DECI_NOVCARGOS"]);
                    balanza.novabonos = Convert.ToInt32(rdr["DECI_NOVABONOS"]);
                    balanza.diccargos = Convert.ToInt32(rdr["DECI_DICCARGOS"]);
                    balanza.dicabonos = Convert.ToInt32(rdr["DECI_DICABONOS"]);

                }

                con.Close();
                return balanza;

            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public int AddBalanza(Balanza balanza)
        {

            string addBalanza = "INSERT INTO"
                     + cod + "TAB_BALANZA" + cod + "("
                     //+ cod + "INT_IDBALANZA" + cod + ","
                     + cod + "TEXT_CTA" + cod + ","
                     + cod + "TEXT_SCTA" + cod + ","
                     + cod + "TEXT_SSCTA" + cod + ","
                     + cod + "INT_YEAR" + cod + ","
                     + cod + "DECI_SALINI" + cod + ","
                     + cod + "DECI_ENECARGOS" + cod + ","
                     + cod + "DECI_ENEABONOS" + cod + ","
                     + cod + "DECI_FEBCARGOS" + cod + ","
                     + cod + "DECI_FEBABONOS" + cod + ","
                     + cod + "DECI_MARCARGOS" + cod + ","
                     + cod + "DECI_MARABONOS" + cod + ","
                     + cod + "DECI_ABRCARGOS" + cod + ","
                     + cod + "DECI_ABRABONOS" + cod + ","
                     + cod + "DECI_MAYCARGOS" + cod + ","
                     + cod + "DECI_MAYABONOS" + cod + ","
                     + cod + "DECI_JUNCARGOS" + cod + ","
                     + cod + "DECI_JUNABONOS" + cod + ","
                     + cod + "DECI_JULCARGOS" + cod + ","
                     + cod + "DECI_JULABONOS" + cod + ","
                     + cod + "DECI_AGOCARGOS" + cod + ","
                     + cod + "DECI_AGOABONOS" + cod + ","
                     + cod + "DECI_SEPCARGOS" + cod + ","
                     + cod + "DECI_SEPABONOS" + cod + ","
                     + cod + "DECI_OCTCARGOS" + cod + ","
                     + cod + "DECI_OCTABONOS" + cod + ","
                     + cod + "DECI_NOVCARGOS" + cod + ","
                     + cod + "DECI_NOVABONOS" + cod + ","
                     + cod + "DECI_DICCARGOS" + cod + ","
                     + cod + "DECI_DICABONOS" + cod + ","
                     //+ cod + "INT_CC" + cod + ","
                     //+ cod + "TEXT_DESCRIPCION" + cod + ","
                     //+ cod + "TEXT_DESCRIPCION2" + cod + ","
                     + cod + "INT_INCLUIR_SUMA" + cod + ","
                     + cod + "INT_TIPO_EXTRACCION" + cod + ","
                     + cod + "TEXT_FECH_EXTR" + cod + ","
                     + cod + "TEXT_HORA" + cod + ","
                     + cod + "INT_ID_EMPRESA" + cod + ","
                     + cod + "DECI_CIERRE_CARGOS" + cod + ","
                     + cod + "DECI_CIERRE_ABONOS" + cod + ","
                     + cod + "INT_ACTA" + cod + ","
                     + cod + "TEXT_CC" + cod + ")"


            + "VALUES "
                             //+ "(@INT_IDBALANZA,"
                             + "(@TEXT_CTA,"
                             + "@TEXT_SCTA,"
                             + "@TEXT_SSCTA,"
                             + "@INT_YEAR,"
                             + "@DECI_SALINI,"
                             + "@DECI_ENECARGOS,"
                             + "@DECI_ENEABONOS,"
                             + "@DECI_FEBCARGOS,"
                             + "@DECI_FEBABONOS,"
                             + "@DECI_MARCARGOS,"
                             + "@DECI_MARABONOS,"
                             + "@DECI_ABRCARGOS,"
                             + "@DECI_ABRABONOS,"
                             + "@DECI_MAYCARGOS,"
                             + "@DECI_MAYABONOS,"
                             + "@DECI_JUNCARGOS,"
                             + "@DECI_JUNABONOS,"
                             + "@DECI_JULCARGOS,"
                             + "@DECI_JULABONOS,"
                             + "@DECI_AGOCARGOS,"
                             + "@DECI_AGOABONOS,"
                             + "@DECI_SEPCARGOS,"
                             + "@DECI_SEPABONOS,"
                             + "@DECI_OCTCARGOS,"
                             + "@DECI_OCTABONOS,"
                             + "@DECI_NOVCARGOS,"
                             + "@DECI_NOVABONOS,"
                             + "@DECI_DICCARGOS,"
                             + "@DECI_DICABONOS,"
                             //+ "@INT_CC,"
                             //+ "@TEXT_DESCRIPCION,"
                             //+ "@TEXT_DESCRIPCION2,"
                             + "@INT_INCLUIR_SUMA,"
                             + "@INT_TIPO_EXTRACCION,"
                             + "@TEXT_FECH_EXTR,"
                             + "@TEXT_HORA,"
                             + "@INT_ID_EMPRESA,"
                             + "@DECI_CIERRE_CARGOS,"
                             + "@DECI_CIERRE_ABONOS,"
                             + "@INT_ACTA,"
                             + "@TEXT_CC)";
            try
            {
                {

                    NpgsqlCommand cmd = new NpgsqlCommand(addBalanza, con);
                    //cmd.Parameters.AddWithValue("@INT_IDBALANZA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_IDBALANZA);
                    cmd.Parameters.AddWithValue("@CTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.cta.Trim());
                    cmd.Parameters.AddWithValue("@SCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.scta.Trim());
                    cmd.Parameters.AddWithValue("@SSCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.sscta.Trim());
                    cmd.Parameters.AddWithValue("@YEAR", NpgsqlTypes.NpgsqlDbType.Integer, balanza.year);
                    cmd.Parameters.AddWithValue("@SALINI", NpgsqlTypes.NpgsqlDbType.Double, balanza.salini);
                    cmd.Parameters.AddWithValue("@ENECARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.enecargos);
                    cmd.Parameters.AddWithValue("@ENEABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.eneabonos);
                    cmd.Parameters.AddWithValue("@FEBCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.febcargos);
                    cmd.Parameters.AddWithValue("@FEBABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.febabonos);
                    cmd.Parameters.AddWithValue("@MARCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.marcargos);
                    cmd.Parameters.AddWithValue("@MARABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.marabonos);
                    cmd.Parameters.AddWithValue("@ABRCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.abrcargos);
                    cmd.Parameters.AddWithValue("@ABRABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.abrabonos);
                    cmd.Parameters.AddWithValue("@MAYCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.maycargos);
                    cmd.Parameters.AddWithValue("@MAYABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.mayabonos);
                    cmd.Parameters.AddWithValue("@JUNCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.juncargos);
                    cmd.Parameters.AddWithValue("@JUNABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.junabonos);
                    cmd.Parameters.AddWithValue("@JULCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.julcargos);
                    cmd.Parameters.AddWithValue("@JULABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.julabonos);
                    cmd.Parameters.AddWithValue("@AGOCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.agocargos);
                    cmd.Parameters.AddWithValue("@AGOABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.agoabonos);
                    cmd.Parameters.AddWithValue("@SEPCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.sepcargos);
                    cmd.Parameters.AddWithValue("@SEPABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.sepabonos);
                    cmd.Parameters.AddWithValue("@OCTCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.octcargos);
                    cmd.Parameters.AddWithValue("@OCTABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.octabonos);
                    cmd.Parameters.AddWithValue("@NOVCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.novcargos);
                    cmd.Parameters.AddWithValue("@NOVABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.novabonos);
                    cmd.Parameters.AddWithValue("@DICCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.diccargos);
                    cmd.Parameters.AddWithValue("@DICABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.dicabonos);
                    //cmd.Parameters.AddWithValue("@CC", NpgsqlTypes.NpgsqlDbType.Integer, balanza.CC);
                    //cmd.Parameters.AddWithValue("@DESCRIPCION", NpgsqlTypes.NpgsqlDbType.Text, balanza.DESCRIPCION.Trim());
                    //cmd.Parameters.AddWithValue("@DESCRIPCION2", NpgsqlTypes.NpgsqlDbType.Text, balanza.DESCRIPCION2.Trim());
                    cmd.Parameters.AddWithValue("@INCLUIR_SUMA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.incluir_suma);
                    cmd.Parameters.AddWithValue("@TIPO_EXTRACCION", NpgsqlTypes.NpgsqlDbType.Integer, balanza.tipo_extraccion);
                    cmd.Parameters.AddWithValue("@FECH_EXTR", NpgsqlTypes.NpgsqlDbType.Text, balanza.fecha_carga.Trim());
                    cmd.Parameters.AddWithValue("@HORA", NpgsqlTypes.NpgsqlDbType.Text, balanza.hora_carga.Trim());
                    cmd.Parameters.AddWithValue("@ID_EMPRESA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.id_empresa);
                    cmd.Parameters.AddWithValue("@CIERRE_CARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.cierre_cargos);
                    cmd.Parameters.AddWithValue("@CIERRE_ABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.cierre_abonos);
                    cmd.Parameters.AddWithValue("@ACTA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.acta);
                    cmd.Parameters.AddWithValue("@CC", NpgsqlTypes.NpgsqlDbType.Text, balanza.cc);

                    con.Open();
                    int cantFilaAfect = Convert.ToInt32(cmd.ExecuteNonQuery());
                    con.Close();
                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
        }
    }
}
