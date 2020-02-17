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
            consulta += "   det.id, det.id_proforma, det.rubro_id, rub.nombre as nombre_rubro, ";
            consulta += "   coalesce(ejercicio_financiero, 0) as ejercicio_financiero, coalesce(ejercicio_resultado, 0) as ejercicio_resultado, ";
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
            consulta += " from proforma_detalle det ";
            consulta += " inner join rubro rub on det.rubro_id = rub.id ";
            consulta += " where id_proforma = " + idProforma;
            consulta += " and activo = 'true' ";

            try
            {
                List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProformaDetalle proforma_detalle = new ProformaDetalle();
                    proforma_detalle.id = Convert.ToInt64(rdr["id"]);
                    proforma_detalle.id_proforma = Convert.ToInt64(rdr["id_proforma"]);
                    proforma_detalle.rubro_id = Convert.ToInt64(rdr["rubro_id"]);
                    proforma_detalle.nombre_rubro = Convert.ToString(rdr["nombre_rubro"]);
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
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
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
            string consulta = "";
            consulta += " update proforma_detalle set activo = '" + bandActivo + "' ";
            consulta += " where id = " + idProformaDetalle;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);

                    int regActual = cmd.ExecuteNonQuery();
                    return regActual;
                }
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

        public int UpdateProformaDetalle(ProformaDetalle proformaDetalle)
        {
            string consulta = "";
            consulta += " update proforma_detalle set activo = @activo, ";
            consulta += "    enero_monto_financiero = @enero_monto_financiero, ";
            consulta += "    enero_monto_resultado = @enero_monto_resultado, ";
            consulta += "    febrero_monto_financiero = @febrero_monto_financiero, ";
            consulta += "    febrero_monto_resultado = @febrero_monto_resultado, ";
            consulta += "    marzo_monto_financiero = @marzo_monto_financiero, ";
            consulta += "    marzo_monto_resultado = @marzo_monto_resultado, ";
            consulta += "    abril_monto_financiero = @abril_monto_financiero, ";
            consulta += "    abril_monto_resultado = @abril_monto_resultado, ";
            consulta += "    mayo_monto_financiero = @mayo_monto_financiero, ";
            consulta += "    mayo_monto_resultado = @mayo_monto_resultado, ";
            consulta += "    junio_monto_financiero = @junio_monto_financiero, ";
            consulta += "    junio_monto_resultado = @junio_monto_resultado, ";
            consulta += "    julio_monto_financiero = @julio_monto_financiero, ";
            consulta += "    julio_monto_resultado = @julio_monto_resultado, ";
            consulta += "    agosto_monto_financiero = @agosto_monto_financiero, ";
            consulta += "    agosto_monto_resultado = @agosto_monto_resultado, ";
            consulta += "    septiembre_monto_financiero = @septiembre_monto_financiero, ";
            consulta += "    septiembre_monto_resultado = @septiembre_monto_resultado, ";
            consulta += "    octubre_monto_financiero = @octubre_monto_financiero, ";
            consulta += "    octubre_monto_resultado = @octubre_monto_resultado, ";
            consulta += "    noviembre_monto_financiero = @noviembre_monto_financiero, ";
            consulta += "    noviembre_monto_resultado = @noviembre_monto_resultado, ";
            consulta += "    diciembre_monto_financiero = @diciembre_monto_financiero, ";
            consulta += "    diciembre_monto_resultado = @diciembre_monto_resultado, ";
            consulta += "    acumulado_financiero = @acumulado_financiero, ";
            consulta += "    acumulado_resultado = @acumulado_resultado, ";
            consulta += "    ejercicio_financiero = @ejercicio_financiero, ";
            consulta += "    ejercicio_resultado = @ejercicio_resultado, ";
            consulta += "    total_financiero = @total_financiero, ";
            consulta += "    total_resultado = @total_resultado ";
            consulta += " where id = @id ";

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                    cmd.Parameters.AddWithValue("@id", proformaDetalle.id);
                    cmd.Parameters.AddWithValue("@enero_monto_financiero", proformaDetalle.enero_monto_financiero);
                    cmd.Parameters.AddWithValue("@enero_monto_resultado", proformaDetalle.enero_monto_resultado);
                    cmd.Parameters.AddWithValue("@febrero_monto_financiero", proformaDetalle.febrero_monto_financiero);
                    cmd.Parameters.AddWithValue("@febrero_monto_resultado", proformaDetalle.febrero_monto_resultado);
                    cmd.Parameters.AddWithValue("@marzo_monto_financiero", proformaDetalle.marzo_monto_financiero);
                    cmd.Parameters.AddWithValue("@marzo_monto_resultado", proformaDetalle.marzo_monto_resultado);
                    cmd.Parameters.AddWithValue("@abril_monto_financiero", proformaDetalle.abril_monto_financiero);
                    cmd.Parameters.AddWithValue("@abril_monto_resultado", proformaDetalle.abril_monto_resultado);
                    cmd.Parameters.AddWithValue("@mayo_monto_financiero", proformaDetalle.mayo_monto_financiero);
                    cmd.Parameters.AddWithValue("@mayo_monto_resultado", proformaDetalle.mayo_monto_resultado);
                    cmd.Parameters.AddWithValue("@junio_monto_financiero", proformaDetalle.junio_monto_financiero);
                    cmd.Parameters.AddWithValue("@junio_monto_resultado", proformaDetalle.junio_monto_resultado);
                    cmd.Parameters.AddWithValue("@julio_monto_financiero", proformaDetalle.julio_monto_financiero);
                    cmd.Parameters.AddWithValue("@julio_monto_resultado", proformaDetalle.julio_monto_resultado);
                    cmd.Parameters.AddWithValue("@agosto_monto_financiero", proformaDetalle.agosto_monto_financiero);
                    cmd.Parameters.AddWithValue("@agosto_monto_resultado", proformaDetalle.agosto_monto_resultado);
                    cmd.Parameters.AddWithValue("@septiembre_monto_financiero", proformaDetalle.septiembre_monto_financiero);
                    cmd.Parameters.AddWithValue("@septiembre_monto_resultado", proformaDetalle.septiembre_monto_resultado);
                    cmd.Parameters.AddWithValue("@octubre_monto_financiero", proformaDetalle.octubre_monto_financiero);
                    cmd.Parameters.AddWithValue("@octubre_monto_resultado", proformaDetalle.octubre_monto_resultado);
                    cmd.Parameters.AddWithValue("@noviembre_monto_financiero", proformaDetalle.noviembre_monto_financiero);
                    cmd.Parameters.AddWithValue("@noviembre_monto_resultado", proformaDetalle.noviembre_monto_resultado);
                    cmd.Parameters.AddWithValue("@diciembre_monto_financiero", proformaDetalle.diciembre_monto_financiero);
                    cmd.Parameters.AddWithValue("@diciembre_monto_resultado", proformaDetalle.diciembre_monto_resultado);
                    cmd.Parameters.AddWithValue("@acumulado_financiero", proformaDetalle.acumulado_financiero);
                    cmd.Parameters.AddWithValue("@acumulado_resultado", proformaDetalle.acumulado_resultado);
                    cmd.Parameters.AddWithValue("@ejercicio_financiero", proformaDetalle.ejercicio_financiero);
                    cmd.Parameters.AddWithValue("@ejercicio_resultado", proformaDetalle.ejercicio_resultado);
                    cmd.Parameters.AddWithValue("@total_financiero", proformaDetalle.total_financiero);
                    cmd.Parameters.AddWithValue("@total_resultado", proformaDetalle.total_resultado);
                    cmd.Parameters.AddWithValue("@activo", proformaDetalle.activo);

                    int regActual = cmd.ExecuteNonQuery();
                    return regActual;
                }
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
        // El parametro mesInicio define el calculo de la proforma
        //      0 = 0+12 - Cero reales, doce proformados
        //      3 =  3+9 - Tres reales, 9 proformados
        //      6 =  6+6 - Seis reales, 6 proformados
        //      9 =  9+3 - Nueve reales, 3 proformados
        // Los reales se calculan desde los montos consolidados
        // Los proformados se capturan en pantalla
        // El parametreo idTipoCaptura indica si la proforma es:
        //      1 - Contable
        //      2 - Flujo
        //
        public List<ProformaDetalle> GetProformaCalculada(Int64 idCenCos, int mesInicio, int idEmpresa, int idModeloNegocio, int idProyecto, int anio, Int64 idTipoCaptura)
        {
            string consulta = "";
            consulta += " select ";
            consulta += "	 mon.id, anio, mes, empresa_id, modelo_negocio_id, ";
            consulta += "	 mon.centro_costo_id, mon.activo, ";
            consulta += "	 proyecto_id, rub.id as rubro_id, rub.nombre as nombre_rubro, ";
            if(mesInicio == 0)
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
                consulta += "	 coalesce(enero_total_financiero, 0) as enero_monto_financiero, ";
                consulta += "	 coalesce(enero_total_resultado, 0) as enero_monto_resultado, ";
                consulta += "	 coalesce(febrero_total_financiero, 0) as febrero_monto_financiero, ";
                consulta += "	 coalesce(febrero_total_resultado, 0) as febrero_monto_resultado, ";
                consulta += "	 coalesce(marzo_total_financiero, 0) as marzo_monto_financiero, ";
                consulta += "	 coalesce(marzo_total_resultado, 0) as marzo_monto_resultado, ";
            }
            if (mesInicio == 0 || mesInicio == 3)
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
                consulta += "	 coalesce(abril_total_financiero, 0) as abril_monto_financiero, ";
                consulta += "	 coalesce(abril_total_resultado, 0) as abril_monto_resultado, ";
                consulta += "	 coalesce(mayo_total_financiero, 0) as mayo_monto_financiero, ";
                consulta += "	 coalesce(mayo_total_resultado, 0) as mayo_monto_resultado, ";
                consulta += "	 coalesce(junio_total_financiero, 0) as junio_monto_financiero, ";
                consulta += "	 coalesce(junio_total_resultado, 0) as junio_monto_resultado, ";
            }
            if (mesInicio == 0 || mesInicio == 3 || mesInicio == 6)
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
                consulta += "	 coalesce(julio_total_financiero, 0) as julio_monto_financiero, ";
                consulta += "	 coalesce(julio_total_resultado, 0) as julio_monto_resultado, ";
                consulta += "	 coalesce(agosto_total_financiero, 0) as agosto_monto_financiero, ";
                consulta += "	 coalesce(agosto_total_resultado, 0) as agosto_monto_resultado, ";
                consulta += "	 coalesce(septiembre_total_financiero, 0) as septiembre_monto_financiero, ";
                consulta += "	 coalesce(septiembre_total_resultado, 0) as septiembre_monto_resultado, ";
            }
            if (mesInicio == 0 || mesInicio == 3 || mesInicio == 6 || mesInicio == 9)
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
                consulta += "	 coalesce(octubre_total_financiero, 0) as octubre_monto_financiero, ";
                consulta += "	 coalesce(octubre_total_resultado, 0) as octubre_monto_resultado, ";
                consulta += "	 coalesce(noviembre_total_financiero, 0) as noviembre_monto_financiero, ";
                consulta += "	 coalesce(noviembre_total_resultado, 0) as noviembre_monto_resultado, ";
                consulta += "	 coalesce(diciembre_total_financiero, 0) as diciembre_monto_financiero, ";
                consulta += "	 coalesce(diciembre_total_resultado, 0) as diciembre_monto_resultado, ";
            }
            switch (mesInicio)
            {
                case 0:
                    consulta += "	 0 as ejercicio_financiero, ";
                    consulta += "	 0 as ejercicio_resultado, ";
                    break;
                case 3:
                    consulta += "	 coalesce(enero_total_financiero + febrero_total_financiero + marzo_total_financiero, 0) as ejercicio_financiero, ";
                    consulta += "	 coalesce(enero_total_resultado + febrero_total_resultado + marzo_total_resultado, 0) as ejercicio_resultado, ";
                    break;
                case 6:
                    consulta += "	 coalesce(enero_total_financiero + febrero_total_financiero + marzo_total_financiero + ";
                    consulta += "	  abril_total_financiero + mayo_total_financiero + junio_total_financiero, 0) as ejercicio_financiero, ";
                    consulta += "	 coalesce(enero_total_resultado + febrero_total_resultado + marzo_total_resultado + ";
                    consulta += "	  abril_total_resultado + mayo_total_resultado + junio_total_resultado, 0) as ejercicio_resultado, ";
                    break;
                case 9:
                    consulta += "	 coalesce(enero_total_financiero + febrero_total_financiero + marzo_total_financiero + ";
                    consulta += "	  abril_total_financiero + mayo_total_financiero + junio_total_financiero + ";
                    consulta += "	  julio_total_financiero + agosto_total_financiero + septiembre_total_financiero, 0) as ejercicio_financiero, ";
                    consulta += "	 coalesce(enero_total_resultado + febrero_total_resultado + marzo_total_resultado + ";
                    consulta += "	  abril_total_resultado + mayo_total_resultado + junio_total_resultado + ";
                    consulta += "	  julio_total_resultado + agosto_total_resultado + septiembre_total_resultado, 0) as ejercicio_resultado, ";
                    break;
                default:
                    consulta += "	 coalesce(enero_total_financiero + febrero_total_financiero + marzo_total_financiero + ";
                    consulta += "	  abril_total_financiero + mayo_total_financiero + junio_total_financiero + ";
                    consulta += "	  julio_total_financiero + agosto_total_financiero + septiembre_total_financiero + ";
                    consulta += "	  octubre_total_financiero + noviembre_total_financiero + diciembre_total_financiero, 0) as ejercicio_financiero, ";
                    consulta += "	 coalesce(enero_total_resultado + febrero_total_resultado + marzo_total_resultado + ";
                    consulta += "	  abril_total_resultado + mayo_total_resultado + junio_total_resultado + ";
                    consulta += "	  julio_total_resultado + agosto_total_resultado + septiembre_total_resultado + ";
                    consulta += "	  octubre_total_resultado + noviembre_total_resultado + diciembre_total_resultado, 0) as ejercicio_resultado, ";
                    break;
            }
            consulta += "	 coalesce(valor_tipo_cambio_financiero, 0) as valor_tipo_cambio_financiero, coalesce(valor_tipo_cambio_resultado, 0) as valor_tipo_cambio_resultado ";
            consulta += "	 from montos_consolidados mon ";
            consulta += "	 inner join rubro rub on mon.rubro_id = rub.id ";
            consulta += "	 where date_trunc('DAY', fecha) = date_trunc('DAY', '" + DateTime.Today.ToString("yyyy-MM-dd") + "'::date) ";
            consulta += "	 and anio = " + anio;                           // Año a proformar
            consulta += "	 and empresa_id = " + idEmpresa;                // Empresa
            consulta += "	 and modelo_negocio_id = " + idModeloNegocio;   // Modelo de Negocio
            consulta += "	 and proyecto_id = " + idProyecto;              // Proyecto
            consulta += "	 and centro_costo_id = " + idCenCos;            // Centro de Costos
            consulta += "	 and tipo_captura_id = " + idTipoCaptura;       // Tipo de captura
            consulta += "	 and mon.activo = 'true' ";                     // Este puede salir sobrando
            consulta += "	 order by rub.id ";

            try
            {

                con.Open();

                List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        ProformaDetalle proforma_detalle = new ProformaDetalle();

                        proforma_detalle.id_proforma = Convert.ToInt64(rdr["id"]);
                        proforma_detalle.anio = Convert.ToInt32(rdr["anio"]);
                        proforma_detalle.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"]);
                        proforma_detalle.centro_costo_id= Convert.ToInt64(rdr["centro_costo_id"]);
                        proforma_detalle.activo = Convert.ToBoolean(rdr["activo"]);
                        proforma_detalle.rubro_id = Convert.ToInt64(rdr["rubro_id"]);
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
                        proforma_detalle.ejercicio_financiero = Convert.ToDouble(rdr["ejercicio_financiero"]);
                        proforma_detalle.ejercicio_resultado = Convert.ToDouble(rdr["ejercicio_resultado"]);
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
        public List<ProformaDetalle> GetAcumuladoAnteriores(Int64 idCenCos,  int idEmpresa, int idModeloNegocio, int idProyecto, int anio, Int64 idTipoCaptura)
        {
            string consulta = "";
            consulta += " select coalesce( ";
            consulta += "	 sum(cns.enero_total_financiero) + ";
            consulta += "	 sum(cns.febrero_total_financiero) + ";
            consulta += "	 sum(cns.marzo_total_financiero) + ";
            consulta += "	 sum(cns.abril_total_financiero) + ";
            consulta += "	 sum(cns.mayo_total_financiero) + ";
            consulta += "	 sum(cns.junio_total_financiero) + ";
            consulta += "	 sum(cns.julio_total_financiero) + ";
            consulta += "	 sum(cns.agosto_total_financiero) + ";
            consulta += "	 sum(cns.septiembre_total_financiero) + ";
            consulta += "	 sum(cns.octubre_total_financiero) + ";
            consulta += "	 sum(cns.noviembre_total_financiero) + ";
            consulta += "	 sum(cns.diciembre_total_financiero) ";
            consulta += "	 , 0) as acumulado_financiero, coalesce ( ";
            consulta += "	 sum(cns.enero_total_resultado) + ";
            consulta += "	 sum(cns.febrero_total_resultado) + ";
            consulta += "	 sum(cns.marzo_total_resultado) + ";
            consulta += "	 sum(cns.abril_total_resultado) + ";
            consulta += "	 sum(cns.mayo_total_resultado) + ";
            consulta += "	 sum(cns.junio_total_resultado) + ";
            consulta += "	 sum(cns.julio_total_resultado) + ";
            consulta += "	 sum(cns.agosto_total_resultado) + ";
            consulta += "	 sum(cns.septiembre_total_resultado) + ";
            consulta += "	 sum(cns.octubre_total_resultado) + ";
            consulta += "	 sum(cns.noviembre_total_resultado) + ";
            consulta += "	 sum(cns.diciembre_total_resultado) ";
            consulta += "	 , 0) as acumulado_resultado, cns.rubro_id as rubro_id, rub.nombre as nombre_rubro ";
            consulta += "	 from montos_consolidados cns ";
            consulta += "	 inner join proyecto pry on cns.proyecto_id = pry.id and cns.modelo_negocio_id = pry.modelo_negocio_id ";
            consulta += "	 inner join rubro rub on cns.rubro_id = rub.id ";
            consulta += "	 where cns.id in ( ";
            consulta += "		 select nue.id ";
            consulta += "		 from montos_consolidados nue ";
            consulta += "		 inner join ( ";
            consulta += "			 select distinct anio as aniosort from montos_consolidados mon ";
            consulta += "				 where mon.anio < " + anio;                         // Anio a proformar
            consulta += "				 and mon.empresa_id = " + idEmpresa;                // Empresa
            consulta += "				 and mon.modelo_negocio_id = " + idModeloNegocio;   // Modelo de Negocio
            consulta += "				 and mon.proyecto_id = " + idProyecto;              // Proyecto
            consulta += "				 and mon.centro_costo_id = " + idCenCos;            // Centro de costos
            consulta += "				 and mon.tipo_captura_id = " + idTipoCaptura;       // Tipo de captura
            consulta += "				 and mon.activo = 'true' ";
            consulta += "		 ) anios on anios.aniosort = nue.anio ";
            consulta += "		 group by nue.id ";
            consulta += "	 ) ";
            consulta += "	 group by cns.rubro_id, rub.nombre ";
            consulta += "	 order by cns.rubro_id ";

            try
            {
                List<ProformaDetalle> lstProfDetalleEjercicioFinanc = new List<ProformaDetalle>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProformaDetalle proforma_detalle_ej_financ = new ProformaDetalle();
                    proforma_detalle_ej_financ.acumulado_financiero = Convert.ToDouble(rdr["acumulado_financiero"]);
                    proforma_detalle_ej_financ.acumulado_resultado = Convert.ToDouble(rdr["acumulado_resultado"]);
                    proforma_detalle_ej_financ.rubro_id = Convert.ToInt64(rdr["rubro_id"]);
                    proforma_detalle_ej_financ.nombre_rubro = rdr["nombre_rubro"].ToString();
                    lstProfDetalleEjercicioFinanc.Add(proforma_detalle_ej_financ);
                }

                return lstProfDetalleEjercicioFinanc;

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

        //Calculo de años posteriores
        public List<ProformaDetalle> GetEjercicioPosterior(int idCentroCosto, int mes, int idEmpresa, int idModeloNegocio, int idProyecto, int idRubro, int anio, bool activo, int idTipoCaptura)
        {
            string consulta = "";
            consulta += " select coalesce(";
            consulta += "	 sum(mon.enero_monto_financiero) + ";
            consulta += "	 sum(mon.febrero_monto_financiero) + ";
            consulta += "	 sum(mon.marzo_monto_financiero) + ";
            consulta += "	 sum(mon.abril_monto_financiero) + ";
            consulta += "	 sum(mon.mayo_monto_financiero) + ";
            consulta += "	 sum(mon.junio_monto_financiero) + ";
            consulta += "	 sum(mon.julio_monto_financiero) + ";
            consulta += "	 sum(mon.agosto_monto_financiero) + ";
            consulta += "	 sum(mon.septiembre_monto_financiero) + ";
            consulta += "	 sum(mon.octubre_monto_financiero) + ";
            consulta += "	 sum(mon.noviembre_monto_financiero) + ";
            consulta += "	 sum(mon.diciembre_monto_financiero) ";
            consulta += "	 , 0) as total_financiero, coalesce (";
            consulta += "	 sum(mon.enero_monto_resultado) + ";
            consulta += "	 sum(mon.febrero_monto_resultado) + ";
            consulta += "	 sum(mon.marzo_monto_resultado) + ";
            consulta += "	 sum(mon.abril_monto_resultado) + ";
            consulta += "	 sum(mon.mayo_monto_resultado) + ";
            consulta += "	 sum(mon.junio_monto_resultado) + ";
            consulta += "	 sum(mon.julio_monto_resultado) + ";
            consulta += "	 sum(mon.agosto_monto_resultado) + ";
            consulta += "	 sum(mon.septiembre_monto_resultado) + ";
            consulta += "	 sum(mon.octubre_monto_resultado) + ";
            consulta += "	 sum(mon.noviembre_monto_resultado) + ";
            consulta += "	 sum(mon.diciembre_monto_resultado) ";
            consulta += "	 , 0) as total_resultado, mon.rubro_id as rubro_id, rub.nombre as nombre_rubro ";
            consulta += "	 from proforma_detalle det ";
            consulta += "	 inner join proyecto pry on det.proyecto_id = pry.id and det.modelo_negocio_id = pry.modelo_negocio_id ";
            consulta += "	 inner join rubro rub on det.rubro_id = rub.id ";
            consulta += "	 where 1 = 1 ";
            consulta += "	 and anio > " + anio;                               // Anio a proformar
            consulta += "	 and empresa_id = " + idEmpresa;                    // Empresa
            consulta += "	 and det.modelo_negocio_id = " + idModeloNegocio;   // Modelo de Negocio
            consulta += "	 and proyecto_id = " + idProyecto;                  // Proyecto
            consulta += "	 and det.rubro_id = " + idRubro;                    // Rubro
            consulta += "	 and det.activo = " + activo;                       // Este puede salir sobrando
            consulta += "	 group by det.rubro_id, rub.nombre ";

            try
            {
                List<ProformaDetalle> lstProfDetalleEjercFinancPost = new List<ProformaDetalle>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ProformaDetalle proforma_detalle_ej_financ_post = new ProformaDetalle();
                    proforma_detalle_ej_financ_post.total_financiero = Convert.ToDouble(rdr["total_financiero"]);
                    proforma_detalle_ej_financ_post.total_resultado = Convert.ToDouble(rdr["total_resultado"]);
                    proforma_detalle_ej_financ_post.rubro_id = Convert.ToInt64(rdr["rubro_id"]);
                    proforma_detalle_ej_financ_post.nombre_rubro = rdr["nombre_rubro"].ToString();
                    lstProfDetalleEjercFinancPost.Add(proforma_detalle_ej_financ_post);
                }

                return lstProfDetalleEjercFinancPost;

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
