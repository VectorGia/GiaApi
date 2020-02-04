using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ProformaDetalleDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public ProformaDetalleDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public List<ProformaDetalle> GetProformaDetalle(int idProforma)
        {
            string consulta = "";
            consulta += " select ";
            consulta += "   id, id_proforma, rubro_id, ejercicio_financiero, ejercicio_resultado, ";
            consulta += "   coalesce(enero_monto_financiero, 0) as enero_monto_financiero, coalesce(enero_monto_resultado, 0) as enero_monto_resultado, ";
            consulta += "   coalesce(febrero_monto_financiero, 0) as febrero_monto_financiero, coalesce(febrero_monto_resultado, 0) as febrero_monto_resultado, ";
            consulta += "   coalesce(marzo_monto_financiero, 0) as marzo_monto_financiero, coalesce(marzo_monto_resultado, 0) as marzo_monto_resultado, ";
            consulta += "   coalesce(abril_monto_financiero, 0) as abril_monto_financiero, coalesce(abril_monto_resultado, 0) as abril_monto_resultado, ";
            consulta += "   coalesce(mayo_monto_financiero, 0) as mayo_monto_financiero, coalesce(mayo_monto_resultado, 0) as mayo_monto_resultado, ";
            consulta += "   coalesce(junio_monto_financiero, 0) as junio_monto_financiero, coalesce(junio_monto_resultado, 0) as junio_monto_resultado, ";
            consulta += "   coalesce(julio_monto_financiero, 0) as julio_monto_financiero, coalesce(julio_monto_resultado, 0) as julio_monto_resultado, ";
            consulta += "   coalesce(agosto_monto_financiero, 0) as agosto_monto_financiero, coalesce(agosto_monto_resultado, 0) as agosto_monto_resultado, ";
            consulta += "   coalesce(septiembre_monto_financiero, 0) as septiembre_monto_financiero, coalesce(septiembre_monto_resultado, 0) as septiembre_monto_resultado, ";
            consulta += "   coalesce(octubre_monto_financiero, 0) as octubre_monto_financiero, coalesce(octubre_monto_resultado, 0) as octubre_monto_resultado, ";
            consulta += "   coalesce(noviembre_monto_financiero, 0) as noviembre_monto_financiero, coalesce(noviembre_monto_resultado, 0) as noviembre_monto_resultado, ";
            consulta += "   coalesce(diciembre_monto_financiero, 0) as diciembre_monto_financiero, coalesce(diciembre_monto_resultado, 0) as diciembre_monto_resultado, ";
            consulta += "   coalesce(total_financiero, 0) as total_financiero, coalesce(total_resultado, 0) as total_resultado, ";
            consulta += "   coalesce(acumulado_financiero, 0) as acumulado_financiero, coalesce(acumulado_resultado, 0) as acumulado_resultado, ";
            consulta += "   coalesce(valor_tipo_cambio_financiero, 0) as valor_tipo_cambio_financiero, ";
            consulta += "   coalesce(valor_tipo_cambio_resultado, 0) as valor_tipo_cambio_resultado ";
            consulta += " from proforma_detalle ";
            consulta += " where id = " + idProforma.ToString();
            consulta += " and activo = 'true' ";

            try
            {
                List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProformaDetalle proforma_detalle = new ProformaDetalle();
                    proforma_detalle.id = Convert.ToInt32(rdr["id"]);
                    proforma_detalle.id_proforma = Convert.ToInt32(rdr["id_proforma"]);
                    proforma_detalle.rubro_id = Convert.ToInt32(rdr["rubro_id"]);
                    proforma_detalle.ejercicio_financiero = Convert.ToDouble(rdr["ejercicio_financiero"]);
                    proforma_detalle.ejercicio_resultado = Convert.ToDouble(rdr["ejercicio_resultado"]);
                    proforma_detalle.enero_monto_financiero = Convert.ToDouble(rdr["enero_monto_financiero"]);
                    proforma_detalle.enero_monto_resultado = Convert.ToDouble(rdr["enero_monto_resultado"]);
                    proforma_detalle.febrero_monto_financiero = Convert.ToDouble(rdr["febrero_monto_financiero"]);
                    proforma_detalle.febrero_monto_resultado = Convert.ToDouble(rdr["febrero_monto_resultado"]);
                    proforma_detalle.marzo_monto_financiero = Convert.ToDouble(rdr["marzo_monto_financiero"]);
                    proforma_detalle.marzo_monto_resultado = Convert.ToDouble(rdr["marzo_monto_resultado"]);
                    proforma_detalle.abril_monto_financiero = Convert.ToDouble(rdr["abril_monto_financiero"]);
                    proforma_detalle.abril_monto_resultado = Convert.ToDouble(rdr["abril_monto_resultado"]);
                    proforma_detalle.mayo_monto_financiero = Convert.ToDouble(rdr["mayo_monto_financiero"]);
                    proforma_detalle.mayo_monto_resultado = Convert.ToDouble(rdr["mayo_monto_resultado"]);
                    proforma_detalle.junio_monto_financiero = Convert.ToDouble(rdr["junio_monto_financiero"]);
                    proforma_detalle.junio_monto_resultado = Convert.ToDouble(rdr["junio_monto_resultado"]);
                    proforma_detalle.julio_monto_financiero = Convert.ToDouble(rdr["julio_monto_financiero"]);
                    proforma_detalle.julio_monto_resultado = Convert.ToDouble(rdr["julio_monto_resultado"]);
                    proforma_detalle.agosto_monto_financiero = Convert.ToDouble(rdr["agosto_monto_financiero"]);
                    proforma_detalle.agosto_monto_resultado = Convert.ToDouble(rdr["agosto_monto_resultado"]);
                    proforma_detalle.septiembre_monto_financiero = Convert.ToDouble(rdr["septiembre_monto_financiero"]);
                    proforma_detalle.septiembre_monto_resultado = Convert.ToDouble(rdr["septiembre_monto_resultado"]);
                    proforma_detalle.octubre_monto_financiero = Convert.ToDouble(rdr["octubre_monto_financiero"]);
                    proforma_detalle.octubre_monto_resultado = Convert.ToDouble(rdr["octubre_monto_resultado"]);
                    proforma_detalle.noviembre_monto_financiero = Convert.ToDouble(rdr["noviembre_monto_financiero"]);
                    proforma_detalle.noviembre_monto_resultado = Convert.ToDouble(rdr["noviembre_monto_resultado"]);
                    proforma_detalle.diciembre_monto_financiero = Convert.ToDouble(rdr["diciembre_monto_financiero"]);
                    proforma_detalle.diciembre_monto_resultado = Convert.ToDouble(rdr["diciembre_monto_resultado"]);
                    proforma_detalle.total_financiero = Convert.ToDouble(rdr["total_financiero"]);
                    proforma_detalle.total_resultado = Convert.ToDouble(rdr["total_resultado"]);
                    proforma_detalle.acumulado_financiero = Convert.ToDouble(rdr["acumulado_financiero"]);
                    proforma_detalle.acumulado_resultado = Convert.ToDouble(rdr["acumulado_resultado"]);
                    proforma_detalle.valor_tipo_cambio_financiero = Convert.ToDouble(rdr["valor_tipo_cambio_financiero"]);
                    proforma_detalle.valor_tipo_cambio_resultado = Convert.ToDouble(rdr["valor_tipo_cambio_resultado"]);
                    proforma_detalle.activo = Convert.ToBoolean(rdr["activo"]);
                    lstProformaDetalle.Add(proforma_detalle);
                }

                return lstProformaDetalle;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

        }

        public int AddProformaDetalle(ProformaDetalle proforma_detalle)
        {
            string consulta = "";
            consulta += " insert into proforma_detalle ( ";
            consulta += "	 id, id_proforma, rubro_id, activo, ejercicio_financiero, ejercicio_resultado, ";
            consulta += "	 enero_monto_financiero, enero_monto_resultado, febrero_monto_financiero, febrero_monto_resultado, ";
            consulta += "	 marzo_monto_financiero, marzo_monto_resultado, abril_monto_financiero, abril_monto_resultado, ";
            consulta += "	 mayo_monto_financiero, mayo_monto_resultado, junio_monto_financiero, junio_monto_resultado, ";
            consulta += "	 julio_monto_financiero, julio_monto_resultado, agosto_monto_financiero, agosto_monto_resultado, ";
            consulta += "	 septiembre_monto_financiero, septiembre_monto_resultado, octubre_monto_financiero, octubre_monto_resultado, ";
            consulta += "	 noviembre_monto_financiero, noviembre_monto_resultado, diciembre_monto_financiero, diciembre_monto_resultado, ";
            consulta += "	 total_financiero, total_resultado, acumulado_financiero, acumulado_resultado, ";
            consulta += "	 valor_tipo_cambio_financiero, valor_tipo_cambio_resultado ";
            consulta += " ) values ( ";
            consulta += "	 nextval('seq_proforma_detalle'), @id_proforma, @rubro_id, @activo, @ejercicio_financiero, @ejercicio_resultado, ";
            consulta += "	 @enero_monto_financiero, @enero_monto_resultado, @febrero_monto_financiero, @febrero_monto_resultado, ";
            consulta += "	 @marzo_monto_financiero, @marzo_monto_resultado, @abril_monto_financiero, @abril_monto_resultado, ";
            consulta += "	 @mayo_monto_financiero, @mayo_monto_resultado, @junio_monto_financiero, @junio_monto_resultado, ";
            consulta += "	 @julio_monto_financiero, @julio_monto_resultado, @agosto_monto_financiero, @agosto_monto_resultado, ";
            consulta += "	 @septiembre_monto_financiero, @septiembre_monto_resultado, @octubre_monto_financiero, @octubre_monto_resultado, ";
            consulta += "	 @noviembre_monto_financiero, @noviembre_monto_resultado, @diciembre_monto_financiero, @diciembre_monto_resultado, ";
            consulta += "	 @total_financiero, @total_resultado, @acumulado_financiero, @acumulado_resultado, ";
            consulta += "	 @valor_tipo_cambio_financiero, @valor_tipo_cambio_resultado ";
            consulta += " ) ";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                cmd.Parameters.AddWithValue("@id_proforma", proforma_detalle.id_proforma);
                cmd.Parameters.AddWithValue("@rubro_id", proforma_detalle.rubro_id);
                cmd.Parameters.AddWithValue("@activo", proforma_detalle.activo);
                cmd.Parameters.AddWithValue("@ejercicio_financiero", proforma_detalle.ejercicio_financiero);
                cmd.Parameters.AddWithValue("@ejercicio_resultado", proforma_detalle.ejercicio_resultado);
                cmd.Parameters.AddWithValue("@enero_monto_financiero", proforma_detalle.enero_monto_financiero);
                cmd.Parameters.AddWithValue("@enero_monto_resultado", proforma_detalle.enero_monto_resultado);
                cmd.Parameters.AddWithValue("@febrero_monto_financiero", proforma_detalle.febrero_monto_financiero);
                cmd.Parameters.AddWithValue("@febrero_monto_resultado", proforma_detalle.febrero_monto_resultado);
                cmd.Parameters.AddWithValue("@marzo_monto_financiero", proforma_detalle.marzo_monto_financiero);
                cmd.Parameters.AddWithValue("@marzo_monto_resultado", proforma_detalle.marzo_monto_resultado);
                cmd.Parameters.AddWithValue("@abril_monto_financiero", proforma_detalle.abril_monto_financiero);
                cmd.Parameters.AddWithValue("@abril_monto_resultado", proforma_detalle.abril_monto_resultado);
                cmd.Parameters.AddWithValue("@mayo_monto_financiero", proforma_detalle.mayo_monto_financiero);
                cmd.Parameters.AddWithValue("@mayo_monto_resultado", proforma_detalle.mayo_monto_resultado);
                cmd.Parameters.AddWithValue("@junio_monto_financiero", proforma_detalle.junio_monto_financiero);
                cmd.Parameters.AddWithValue("@junio_monto_resultado", proforma_detalle.junio_monto_resultado);
                cmd.Parameters.AddWithValue("@julio_monto_financiero", proforma_detalle.julio_monto_financiero);
                cmd.Parameters.AddWithValue("@julio_monto_resultado", proforma_detalle.julio_monto_resultado);
                cmd.Parameters.AddWithValue("@agosto_monto_financiero", proforma_detalle.agosto_monto_financiero);
                cmd.Parameters.AddWithValue("@agosto_monto_resultado", proforma_detalle.agosto_monto_resultado);
                cmd.Parameters.AddWithValue("@septiembre_monto_financiero", proforma_detalle.septiembre_monto_financiero);
                cmd.Parameters.AddWithValue("@septiembre_monto_resultado", proforma_detalle.septiembre_monto_resultado);
                cmd.Parameters.AddWithValue("@octubre_monto_financiero", proforma_detalle.octubre_monto_financiero);
                cmd.Parameters.AddWithValue("@octubre_monto_resultado", proforma_detalle.octubre_monto_resultado);
                cmd.Parameters.AddWithValue("@noviembre_monto_financiero", proforma_detalle.noviembre_monto_financiero);
                cmd.Parameters.AddWithValue("@noviembre_monto_resultado", proforma_detalle.noviembre_monto_resultado);
                cmd.Parameters.AddWithValue("@diciembre_monto_financiero", proforma_detalle.diciembre_monto_financiero);
                cmd.Parameters.AddWithValue("@diciembre_monto_resultado", proforma_detalle.diciembre_monto_resultado);
                cmd.Parameters.AddWithValue("@total_financiero", proforma_detalle.total_financiero);
                cmd.Parameters.AddWithValue("@total_resultado", proforma_detalle.total_resultado);
                cmd.Parameters.AddWithValue("@acumulado_financiero", proforma_detalle.acumulado_financiero);
                cmd.Parameters.AddWithValue("@acumulado_resultado", proforma_detalle.acumulado_resultado);
                cmd.Parameters.AddWithValue("@valor_tipo_cambio_financiero", proforma_detalle.valor_tipo_cambio_financiero);
                cmd.Parameters.AddWithValue("@valor_tipo_cambio_resultado", proforma_detalle.valor_tipo_cambio_resultado);

                con.Open();
                int regInsert = cmd.ExecuteNonQuery();

                return regInsert;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

        }

        public int UpdateProformaDetalle(int idProformaDetalle, bool bandActivo)
        {
            string consulta = " update proforma_detalle set activo = '" + bandActivo.ToString() + "' ";
            consulta += " where id = " + idProformaDetalle.ToString();

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, conex.ConnexionDB());

                con.Open();
                int regActual = cmd.ExecuteNonQuery();
                return regActual;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // Calculo de la proforma a partir de los montos consolidados
        // El parametro idTipoCaptura define el calculo de la proforma
        //      0 = 0+12 - Cero reales, doce proformados
        //      3 =  3+9 - Tres reales, 9 proformados
        //      6 =  6+6 - Seis reales, 6 proformados
        //      9 =  9+3 - Nueve reales, 3 proformados
        // Los reales se calculan desde los montos consolidados
        // Los proformados se capturan en pantalla
        public IEnumerable<ProformaDetalle> GetProformaCalculada(int idCentroCosto, int mes, int idEmpresa, int idModeloNegocio, int idProyecto, int idRubro, int anio, int idTipoCaptura)
        {
            string consulta = "";
            consulta += " select ";
            consulta += "	 mon.id, anio, mes, empresa_id, modelo_negocio_id, ";
            consulta += "	 proyecto_id, rub.id as rubro_id, rub.nombre as nombre_rubro, ";
            if(idTipoCaptura == 0)
            {
                // Para el 0+12 Enero, Febrero y Marzo se capturan
                consulta += "	 0 as enero_monto_financiero, ";
                consulta += "	 0 as enero_monto_resultado, ";
                consulta += "	 0 as febrero_monto_financiero, ";
                consulta += "	 0 as febrero_monto_resultado, ";
                consulta += "	 0 as marzo_monto_financiero, ";
                consulta += "	 0 as marzo_monto_resultado, ";
            }
            else
            {
                // Para las demas proformas Enero, Febrero y Marzo se calculan
                consulta += "	 coalesce((enero_abono_financiero + enero_cargo_financiero), 0) as enero_monto_financiero, ";
                consulta += "	 coalesce((enero_abono_resultado + enero_cargo_resultado), 0) as enero_monto_resultado, ";
                consulta += "	 coalesce((febrero_abono_financiero + febrero_cargo_financiero), 0) as febrero_monto_financiero, ";
                consulta += "	 coalesce((febrero_abono_resultado + febrero_cargo_resultado), 0) as febrero_monto_resultado, ";
                consulta += "	 coalesce((marzo_abono_financiero + marzo_cargo_financiero), 0) as marzo_monto_financiero, ";
                consulta += "	 coalesce((marzo_abono_resultado + marzo_cargo_resultado), 0) as marzo_monto_resultado, ";
            }
            if(idTipoCaptura == 0 || idTipoCaptura == 3)
            {
                // Para el 0+12 y el 3+9 Abril, Mayo y Junio se capturan
                consulta += "	 0 as abril_monto_financiero, ";
                consulta += "	 0 as abril_monto_resultado, ";
                consulta += "	 0 as mayo_monto_financiero, ";
                consulta += "	 0 as mayo_monto_resultado, ";
                consulta += "	 0 as junio_monto_financiero, ";
                consulta += "	 0 as junio_monto_resultado, ";
            }
            else
            {
                // Para las demas proformas Abril, Mayo y Junio se calculan
                consulta += "	 coalesce((abril_abono_financiero + abril_cargo_financiero), 0) as abril_monto_financiero, ";
                consulta += "	 coalesce((abril_abono_resultado + abril_cargo_resultado), 0) as abril_monto_resultado, ";
                consulta += "	 coalesce((mayo_abono_financiero + mayo_cargo_financiero), 0) as mayo_monto_financiero, ";
                consulta += "	 coalesce((mayo_abono_resultado + mayo_cargo_resultado), 0) as mayo_monto_resultado, ";
                consulta += "	 coalesce((junio_abono_financiero + junio_cargo_financiero), 0) as junio_monto_financiero, ";
                consulta += "	 coalesce((junio_abono_resultado + junio_cargo_resultado), 0) as junio_monto_resultado, ";
            }
            if (idTipoCaptura==0 || idTipoCaptura==3 || idTipoCaptura == 6)
            {
                // Para el 0+12, el 3+9 y el 6+6 Julio, Agosto y Septiembre se capturan
                consulta += "	 0 as julio_monto_financiero, ";
                consulta += "	 0 as julio_monto_resultado, ";
                consulta += "	 0 as agosto_monto_financiero, ";
                consulta += "	 0 as agosto_monto_resultado, ";
                consulta += "	 0 as septiembre_monto_financiero, ";
                consulta += "	 0 as septiembre_monto_resultado, ";
            }
            else
            {
                // Para el 9+3 Julio, Agosto y Septiembre se calculan
                consulta += "	 coalesce((julio_abono_financiero + julio_cargo_financiero), 0) as julio_monto_financiero, ";
                consulta += "	 coalesce((julio_abono_resultado + julio_cargo_resultado), 0) as julio_monto_resultado, ";
                consulta += "	 coalesce((agosto_abono_financiero + agosto_cargo_financiero), 0) as agosto_monto_financiero, ";
                consulta += "	 coalesce((agosto_abono_resultado + agosto_cargo_resultado), 0) as agosto_monto_resultado, ";
                consulta += "	 coalesce((septiembre_abono_financiero + septiembre_cargo_financiero), 0) as septiembre_monto_financiero, ";
                consulta += "	 coalesce((septiembre_abono_resultado + septiembre_cargo_resultado), 0) as septiembre_monto_resultado, ";
            }
            if(idTipoCaptura == 0 || idTipoCaptura == 3 || idTipoCaptura == 6 || idTipoCaptura == 9)
            {
                // Para 0+12, 3+9, 6+6 y 9+3 el resto de los meses se capturan
                consulta += "	 0 as octubre_monto_financiero, ";
                consulta += "	 0 as octubre_monto_resultado, ";
                consulta += "	 0 as noviembre_monto_financiero, ";
                consulta += "	 0 as noviembre_monto_resultado, ";
                consulta += "	 0 as diciembre_monto_financiero, ";
                consulta += "	 0 as diciembre_monto_resultado, ";
            }
            else
            {
                // Apartado para un 12+0 en que se calcularia todo el año
                consulta += "	 coalesce((octubre_abono_financiero + octubre_cargo_financiero), 0) as octubre_monto_financiero, ";
                consulta += "	 coalesce((octubre_abono_resultado + octubre_cargo_resultado), 0) as octubre_monto_resultado, ";
                consulta += "	 coalesce((noviembre_abono_financiero + noviembre_cargo_financiero), 0) as noviembre_monto_financiero, ";
                consulta += "	 coalesce((noviembre_abono_resultado + noviembre_cargo_resultado), 0) as noviembre_monto_resultado, ";
                consulta += "	 coalesce((diciembre_abono_financiero + diciembre_cargo_financiero), 0) as diciembre_monto_financiero, ";
                consulta += "	 coalesce((diciembre_abono_resultado + diciembre_cargo_resultado), 0) as diciembre_monto_resultado, ";
            }
            switch (idTipoCaptura)
            {
                case 0:
                    consulta += "	 0 as total_financiero, ";
                    consulta += "	 0 as total_resultado, ";
                    break;
                case 3:
                    consulta += "	 coalesce((enero_abono_financiero + febrero_abono_financiero + marzo_abono_financiero + ";
                    consulta += "	  enero_cargo_financiero + febrero_cargo_financiero + marzo_cargo_financiero), 0) as total_financiero, ";
                    consulta += "	 coalesce((enero_abono_resultado + febrero_abono_resultado + marzo_abono_resultado + ";
                    consulta += "	  enero_cargo_resultado + febrero_cargo_resultado + marzo_cargo_resultado), 0) as total_resultado, ";
                    break;
                case 6:
                    consulta += "	 coalesce((enero_abono_financiero + febrero_abono_financiero + marzo_abono_financiero + ";
                    consulta += "	  enero_cargo_financiero + febrero_cargo_financiero + marzo_cargo_financiero + ";
                    consulta += "	  abril_abono_financiero + mayo_abono_financiero + junio_abono_financiero + ";
                    consulta += "	  abril_cargo_financiero + mayo_cargo_financiero + junio_cargo_financiero), 0) as total_financiero, ";
                    consulta += "	 coalesce((enero_abono_resultado + febrero_abono_resultado + marzo_abono_resultado + ";
                    consulta += "	  enero_cargo_resultado + febrero_cargo_resultado + marzo_cargo_resultado + ";
                    consulta += "	  abril_abono_resultado + mayo_abono_resultado + junio_abono_resultado + ";
                    consulta += "	  abril_cargo_resultado + mayo_cargo_resultado + junio_cargo_resultado), 0) as total_resultado, ";
                    break;
                case 9:
                    consulta += "	 coalesce((enero_abono_financiero + febrero_abono_financiero + marzo_abono_financiero + ";
                    consulta += "	  enero_cargo_financiero + febrero_cargo_financiero + marzo_cargo_financiero + ";
                    consulta += "	  abril_abono_financiero + mayo_abono_financiero + junio_abono_financiero + ";
                    consulta += "	  abril_cargo_financiero + mayo_cargo_financiero + junio_cargo_financiero + ";
                    consulta += "	  julio_abono_financiero + agosto_abono_financiero + septiembre_abono_financiero + ";
                    consulta += "	  julio_cargo_financiero + agosto_cargo_financiero + septiembre_cargo_financiero), 0) as total_financiero, ";
                    consulta += "	 coalesce((enero_abono_resultado + febrero_abono_resultado + marzo_abono_resultado + ";
                    consulta += "	  enero_cargo_resultado + febrero_cargo_resultado + marzo_cargo_resultado + ";
                    consulta += "	  abril_abono_resultado + mayo_abono_resultado + junio_abono_resultado + ";
                    consulta += "	  abril_cargo_resultado + mayo_cargo_resultado + junio_cargo_resultado + ";
                    consulta += "	  julio_abono_resultado + agosto_abono_resultado + septiembre_abono_resultado + ";
                    consulta += "	  julio_cargo_resultado + agosto_cargo_resultado + septiembre_cargo_resultado), 0) as total_resultado, ";
                    break;
                default:
                    consulta += "	 coalesce((enero_abono_financiero + febrero_abono_financiero + marzo_abono_financiero + ";
                    consulta += "	  enero_cargo_financiero + febrero_cargo_financiero + marzo_cargo_financiero + ";
                    consulta += "	  abril_abono_financiero + mayo_abono_financiero + junio_abono_financiero + ";
                    consulta += "	  abril_cargo_financiero + mayo_cargo_financiero + junio_cargo_financiero + ";
                    consulta += "	  julio_abono_financiero + agosto_abono_financiero + septiembre_abono_financiero + ";
                    consulta += "	  julio_cargo_financiero + agosto_cargo_financiero + septiembre_cargo_financiero + ";
                    consulta += "	  octubre_abono_financiero + noviembre_abono_financiero + diciembre_abono_financiero + ";
                    consulta += "	  octubre_cargo_financiero + noviembre_cargo_financiero + diciembre_cargo_financiero ), 0) as total_financiero, ";
                    consulta += "	 coalesce((enero_abono_resultado + febrero_abono_resultado + marzo_abono_resultado + ";
                    consulta += "	  enero_cargo_resultado + febrero_cargo_resultado + marzo_cargo_resultado + ";
                    consulta += "	  abril_abono_resultado + mayo_abono_resultado + junio_abono_resultado + ";
                    consulta += "	  abril_cargo_resultado + mayo_cargo_resultado + junio_cargo_resultado + ";
                    consulta += "	  julio_abono_resultado + agosto_abono_resultado + septiembre_abono_resultado + ";
                    consulta += "	  julio_cargo_resultado + agosto_cargo_resultado + septiembre_cargo_resultado + ";
                    consulta += "	  octubre_cargo_resultado + noviembre_cargo_resultado + diciembre_cargo_resultado + ";
                    consulta += "	  octubre_abono_resultado + noviembre_abono_resultado + diciembre_abono_resultado), 0) as total_resultado, ";
                    break;
            }
            consulta += "	 coalesce(valor_tipo_cambio_financiero, 0) as valor_tipo_cambio_financiero, coalesce(valor_tipo_cambio_resultado, 0) as valor_tipo_cambio_resultado ";
            consulta += "	 from montos_consolidados mon ";
            consulta += "	 inner join rubro rub on mon.rubro_id = rub.id ";
            consulta += "	 where 1 = 1 ";
            consulta += "	 and anio = " + anio.ToString();                            // Año a proformar
            consulta += "	 and mes = " + mes.ToString();                              // Mes (revisar)
            consulta += "	 and empresa_id = " + idEmpresa.ToString();                 // Empresa
            consulta += "	 and modelo_negocio_id = " + idModeloNegocio.ToString();    // Modelo de Negocio
            consulta += "	 and proyecto_id = " + idProyecto.ToString();               // Proyecto
            consulta += "	 and rub.id = " + idRubro.ToString();                       // Rubro
            // Aparentemente falta el Centro de Costos (en Shadow se debera crear uno)
            consulta += "	 and mon.activo = 'true' "; // Este puede salir sobrando
            consulta += "	 order by rub.id ";

            try
            {

                con.Open();

                List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    //con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        ProformaDetalle proforma_detalle = new ProformaDetalle();

                        proforma_detalle.id_proforma = Convert.ToInt32(rdr["id"]);
                        proforma_detalle.anio = Convert.ToInt32(rdr["anio"]);
                        proforma_detalle.modelo_negocio_id = Convert.ToInt32(rdr["modelo_negocio_id"]);
                        proforma_detalle.rubro_id = Convert.ToInt32(rdr["rubro_id"]);
                        proforma_detalle.nombre_rubro = (rdr["nombre_rubro"]).ToString().Trim();
                        proforma_detalle.enero_monto_financiero = Convert.ToDouble(rdr["enero_monto_financiero"]);
                        proforma_detalle.enero_monto_resultado = Convert.ToDouble(rdr["enero_monto_resultado"]);
                        proforma_detalle.febrero_monto_financiero = Convert.ToDouble(rdr["febrero_monto_financiero"]);
                        proforma_detalle.febrero_monto_resultado = Convert.ToDouble(rdr["febrero_monto_resultado"]);
                        proforma_detalle.marzo_monto_financiero = Convert.ToDouble(rdr["marzo_monto_financiero"]);
                        proforma_detalle.marzo_monto_resultado = Convert.ToDouble(rdr["marzo_monto_resultado"]);
                        proforma_detalle.abril_monto_financiero = Convert.ToDouble(rdr["abril_monto_financiero"]);
                        proforma_detalle.abril_monto_resultado = Convert.ToDouble(rdr["abril_monto_resultado"]);
                        proforma_detalle.mayo_monto_financiero = Convert.ToDouble(rdr["mayo_monto_financiero"]);
                        proforma_detalle.mayo_monto_resultado = Convert.ToDouble(rdr["mayo_monto_resultado"]);
                        proforma_detalle.junio_monto_financiero = Convert.ToDouble(rdr["junio_monto_financiero"]);
                        proforma_detalle.junio_monto_resultado = Convert.ToDouble(rdr["junio_monto_resultado"]);
                        proforma_detalle.julio_monto_financiero = Convert.ToDouble(rdr["julio_monto_financiero"]);
                        proforma_detalle.julio_monto_resultado = Convert.ToDouble(rdr["julio_monto_resultado"]);
                        proforma_detalle.agosto_monto_financiero = Convert.ToDouble(rdr["agosto_monto_financiero"]);
                        proforma_detalle.agosto_monto_resultado = Convert.ToDouble(rdr["agosto_monto_resultado"]);
                        proforma_detalle.septiembre_monto_financiero = Convert.ToDouble(rdr["septiembre_monto_financiero"]);
                        proforma_detalle.septiembre_monto_resultado = Convert.ToDouble(rdr["septiembre_monto_resultado"]);
                        proforma_detalle.octubre_monto_financiero = Convert.ToDouble(rdr["octubre_monto_financiero"]);
                        proforma_detalle.octubre_monto_resultado = Convert.ToDouble(rdr["octubre_monto_resultado"]);
                        proforma_detalle.noviembre_monto_financiero = Convert.ToDouble(rdr["noviembre_monto_financiero"]);
                        proforma_detalle.noviembre_monto_resultado = Convert.ToDouble(rdr["noviembre_monto_resultado"]);
                        proforma_detalle.diciembre_monto_financiero = Convert.ToDouble(rdr["diciembre_monto_financiero"]);
                        proforma_detalle.diciembre_monto_resultado = Convert.ToDouble(rdr["diciembre_monto_resultado"]);
                        proforma_detalle.total_financiero = Convert.ToDouble(rdr["total_financiero"]);
                        proforma_detalle.total_resultado = Convert.ToDouble(rdr["total_resultado"]);
                        lstProformaDetalle.Add(proforma_detalle);
                    }

                }

                return lstProformaDetalle;

            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        // Calculo del ejercicio anterior
        public IEnumerable<ProformaDetalle> GetEjercicioAnterior(int idCentroCosto, int mes, int idEmpresa, int idModeloNegocio, int idProyecto, int idRubro, int anio, int activo, int idTipoCaptura)
        {
            string cadena = "";
            cadena += " select coalesce(";
            cadena += "	 sum(mon.enero_abono_financiero) + sum(mon.enero_cargo_financiero) + ";
            cadena += "	 sum(mon.febrero_abono_financiero) + sum(mon.febrero_cargo_financiero) + ";
            cadena += "	 sum(mon.marzo_abono_financiero) + sum(mon.marzo_cargo_financiero) + ";
            cadena += "	 sum(mon.abril_abono_financiero) + sum(mon.abril_cargo_financiero) + ";
            cadena += "	 sum(mon.mayo_abono_financiero) + sum(mon.mayo_cargo_financiero) + ";
            cadena += "	 sum(mon.junio_abono_financiero) + sum(mon.junio_cargo_financiero) + ";
            cadena += "	 sum(mon.julio_abono_financiero) + sum(mon.julio_cargo_financiero) + ";
            cadena += "	 sum(mon.agosto_abono_financiero) + sum(mon.agosto_cargo_financiero) + ";
            cadena += "	 sum(mon.septiembre_abono_financiero) + sum(mon.septiembre_cargo_financiero) + ";
            cadena += "	 sum(mon.octubre_abono_financiero) + sum(mon.octubre_cargo_financiero) + ";
            cadena += "	 sum(mon.noviembre_abono_financiero) + sum(mon.noviembre_cargo_financiero) + ";
            cadena += "	 sum(mon.diciembre_abono_financiero) + sum(mon.diciembre_cargo_financiero) ";
            cadena += "	 , 0) as ejercicio_financiero, coalesce (";
            cadena += "	 sum(mon.enero_abono_resultado) + sum(mon.enero_cargo_resultado) + ";
            cadena += "	 sum(mon.febrero_abono_resultado) + sum(mon.febrero_cargo_resultado) + ";
            cadena += "	 sum(mon.marzo_abono_resultado) + sum(mon.marzo_cargo_resultado) + ";
            cadena += "	 sum(mon.abril_abono_resultado) + sum(mon.abril_cargo_resultado) + ";
            cadena += "	 sum(mon.mayo_abono_resultado) + sum(mon.mayo_cargo_resultado) + ";
            cadena += "	 sum(mon.junio_abono_resultado) + sum(mon.junio_cargo_resultado) + ";
            cadena += "	 sum(mon.julio_abono_resultado) + sum(mon.julio_cargo_resultado) + ";
            cadena += "	 sum(mon.agosto_abono_resultado) + sum(mon.agosto_cargo_resultado) + ";
            cadena += "	 sum(mon.septiembre_abono_resultado) + sum(mon.septiembre_cargo_resultado) + ";
            cadena += "	 sum(mon.octubre_abono_resultado) + sum(mon.octubre_cargo_resultado) + ";
            cadena += "	 sum(mon.noviembre_abono_resultado) + sum(mon.noviembre_cargo_resultado) + ";
            cadena += "	 sum(mon.diciembre_abono_resultado) + sum(mon.diciembre_cargo_resultado) ";
            cadena += "	 , 0) as ejercicio_resultado ";
            cadena += "	 from montos_consolidados mon ";
            cadena += "	 inner join proyecto pry on mon.proyecto_id = pry.id and mon.modelo_negocio_id = pry.modelo_negocio_id ";
            cadena += "	 where 1 = 1 ";
            cadena += "	 and anio < " + anio.ToString(); // Corregir para que tome del inicio del proyecto al año actual
            cadena += "	 and mes = " + mes.ToString();                              // Mes (revisar)
            cadena += "	 and empresa_id = " + idEmpresa.ToString();                 // Empresa
            cadena += "	 and modelo_negocio_id = " + idModeloNegocio.ToString();    // Modelo de Negocio
            cadena += "	 and proyecto_id = " + idProyecto.ToString();               // Proyecto
            cadena += "	 and rub.id = " + idRubro.ToString();                       // Rubro
            cadena += "	 and mon.activo = 'true' "; // Este puede salir sobrando
            cadena += "	 order by rub.id ";

            try
            {
                List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProformaDetalle proforma_detalle = new ProformaDetalle();
                    proforma_detalle.ejercicio_financiero = Convert.ToDouble(rdr["ejercicio_financiero"]);
                    proforma_detalle.ejercicio_resultado = Convert.ToDouble(rdr["ejercicio_resultado"]);
                }

                return lstProformaDetalle;

            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
