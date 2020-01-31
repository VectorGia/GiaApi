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
            "sum(" + cod + "DECI_JUNCARGOS" + cod + ") as " + cod + "DECI_JUNCARGOS" + cod + ", sum (" + cod + "DECI_JUNABONOS" + cod + ") as " + cod + "DECI_JUNABONOS" + cod + "," +
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
                    balanza.INT_YEAR = Convert.ToInt32(rdr["INT_YEAR"]);
                    balanza.TEXT_DESCRIPCION = rdr["STR_CONCEPTO"].ToString().Trim();
                    balanza.DECI_SALINI = Convert.ToInt32(rdr["DECI_SALINI"]);
                    balanza.DECI_ENECARGOS = Convert.ToInt32(rdr["DECI_ENECARGOS"]);
                    balanza.DECI_ENEABONOS = Convert.ToInt32(rdr["DECI_ENEABONOS"]);
                    balanza.DECI_FEBCARGOS = Convert.ToInt32(rdr["DECI_FEBCARGOS"]);
                    balanza.DECI_FEBABONOS = Convert.ToInt32(rdr["DECI_FEBABONOS"]);
                    balanza.DECI_MARCARGOS = Convert.ToInt32(rdr["DECI_MARCARGOS"]);
                    balanza.DECI_MARABONOS = Convert.ToInt32(rdr["DECI_MARABONOS"]);
                    balanza.DECI_ABRCARGOS = Convert.ToInt32(rdr["DECI_ABRCARGOS"]);
                    balanza.DECI_ABRABONOS = Convert.ToInt32(rdr["DECI_ABRABONOS"]);
                    balanza.DECI_MAYCARGOS = Convert.ToInt32(rdr["DECI_MAYCARGOS"]);
                    balanza.DECI_MAYABONOS = Convert.ToInt32(rdr["DECI_MAYABONOS"]);
                    balanza.DECI_JUNCARGOS = Convert.ToInt32(rdr["DECI_JUNCARGOS"]);
                    balanza.DECI_JUNABONOS = Convert.ToInt32(rdr["DECI_JUNABONOS"]);
                    balanza.DECI_JULCARGOS = Convert.ToInt32(rdr["DECI_JULCARGOS"]);
                    balanza.DECI_JULABONOS = Convert.ToInt32(rdr["DECI_JULABONOS"]);
                    balanza.DECI_AGOCARGOS = Convert.ToInt32(rdr["DECI_AGOCARGOS"]);
                    balanza.DECI_AGOABONOS = Convert.ToInt32(rdr["DECI_AGOABONOS"]);
                    balanza.DECI_SEPCARGOS = Convert.ToInt32(rdr["DECI_SEPCARGOS"]);
                    balanza.DECI_SEPABONOS = Convert.ToInt32(rdr["DECI_SEPABONOS"]);
                    balanza.DECI_OCTCARGOS = Convert.ToInt32(rdr["DECI_OCTCARGOS"]);
                    balanza.DECI_OCTABONOS = Convert.ToInt32(rdr["DECI_OCTABONOS"]);
                    balanza.DECI_NOVCARGOS = Convert.ToInt32(rdr["DECI_NOVCARGOS"]);
                    balanza.DECI_NOVABONOS = Convert.ToInt32(rdr["DECI_NOVABONOS"]);
                    balanza.DECI_DICCARGOS = Convert.ToInt32(rdr["DECI_DICCARGOS"]);
                    balanza.DECI_DICABONOS = Convert.ToInt32(rdr["DECI_DICABONOS"]);

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
                    cmd.Parameters.AddWithValue("@TEXT_CTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_CTA.Trim());
                    cmd.Parameters.AddWithValue("@TEXT_SCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_SCTA.Trim());
                    cmd.Parameters.AddWithValue("@TEXT_SSCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_SSCTA.Trim());
                    cmd.Parameters.AddWithValue("@INT_YEAR", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_YEAR);
                    cmd.Parameters.AddWithValue("@DECI_SALINI", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_SALINI);
                    cmd.Parameters.AddWithValue("@DECI_ENECARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ENECARGOS);
                    cmd.Parameters.AddWithValue("@DECI_ENEABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ENEABONOS);
                    cmd.Parameters.AddWithValue("@DECI_FEBCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_FEBCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_FEBABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_FEBABONOS);
                    cmd.Parameters.AddWithValue("@DECI_MARCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MARCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_MARABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MARABONOS);
                    cmd.Parameters.AddWithValue("@DECI_ABRCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ABRCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_ABRABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ABRABONOS);
                    cmd.Parameters.AddWithValue("@DECI_MAYCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MAYCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_MAYABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MAYABONOS);
                    cmd.Parameters.AddWithValue("@DECI_JUNCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JUNCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_JUNABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JUNABONOS);
                    cmd.Parameters.AddWithValue("@DECI_JULCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JULCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_JULABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JULABONOS);
                    cmd.Parameters.AddWithValue("@DECI_AGOCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_AGOCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_AGOABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_AGOABONOS);
                    cmd.Parameters.AddWithValue("@DECI_SEPCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_SEPCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_SEPABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_SEPABONOS);
                    cmd.Parameters.AddWithValue("@DECI_OCTCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_OCTCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_OCTABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_OCTABONOS);
                    cmd.Parameters.AddWithValue("@DECI_NOVCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_NOVCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_NOVABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_NOVABONOS);
                    cmd.Parameters.AddWithValue("@DECI_DICCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_DICCARGOS);
                    cmd.Parameters.AddWithValue("@DECI_DICABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_DICABONOS);
                    //cmd.Parameters.AddWithValue("@INT_CC", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_CC);
                    //cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_DESCRIPCION.Trim());
                    //cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION2", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_DESCRIPCION2.Trim());
                    cmd.Parameters.AddWithValue("@INT_INCLUIR_SUMA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_INCLUIR_SUMA);
                    cmd.Parameters.AddWithValue("@INT_TIPO_EXTRACCION", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_TIPO_EXTRACCION);
                    cmd.Parameters.AddWithValue("@TEXT_FECH_EXTR", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_FECH_EXTR.Trim());
                    cmd.Parameters.AddWithValue("@TEXT_HORA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_HORA.Trim());
                    cmd.Parameters.AddWithValue("@INT_ID_EMPRESA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_ID_EMPRESA);
                    cmd.Parameters.AddWithValue("@DECI_CIERRE_CARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_CIERRE_CARGOS);
                    cmd.Parameters.AddWithValue("@DECI_CIERRE_ABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_CIERRE_ABONOS);
                    cmd.Parameters.AddWithValue("@INT_ACTA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_ACTA);
                    cmd.Parameters.AddWithValue("@TEXT_CC", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_CC);


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
