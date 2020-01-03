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

        public int AddBalanza(Balanza balanza) {

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
                     + cod + "INT_CC" + cod + ","
                     + cod + "TEXT_DESCRIPCION" + cod + ","
                     + cod + "TEXT_DESCRIPCION2" + cod + ","
                     + cod + "INT_INCLUIR_SUMA" + cod + ")"
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
                             + "@INT_CC,"
                             + "@TEXT_DESCRIPCION,"
                             + "@TEXT_DESCRIPCION2,"
                             + "@INT_INCLUIR_SUMA)";
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
                    cmd.Parameters.AddWithValue("@INT_CC", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_CC);
                    cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_DESCRIPCION.Trim());
                    cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION2", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_DESCRIPCION2.Trim());
                    cmd.Parameters.AddWithValue("@INT_INCLUIR_SUMA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_INCLUIR_SUMA);
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
