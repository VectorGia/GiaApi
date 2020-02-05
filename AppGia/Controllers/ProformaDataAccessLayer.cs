﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ProformaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public ProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public List<Proforma> GetProforma(int idProforma)
        {
            string consulta = "";
            consulta += " select id, anio, modelo_negocio_id, tipo_captura_id, tipo_proforma_id, centro_costo_id, activo, usuario, fecha_captura ";
            consulta += " from proforma ";
            consulta += " where id = " + idProforma.ToString();
            consulta += " and activo = 'true' ";

            try
            {
                List<Proforma> lstProforma = new List<Proforma>();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Proforma proforma = new Proforma();
                    proforma.id = Convert.ToInt32(rdr["id"]);
                    proforma.modelo_negocio_id = Convert.ToInt32(rdr["modelo_negocio_id"]);
                    proforma.tipo_captura_id = Convert.ToInt32(rdr["tipo_captura_id"]);
                    proforma.tipo_proforma_id = Convert.ToInt32(rdr["tipo_proforma_id"]);
                    proforma.centro_costo_id = Convert.ToInt32(rdr["centro_costo_id"]);
                    proforma.activo = Convert.ToBoolean(rdr["activo"]);
                    proforma.usuario = Convert.ToInt32(rdr["usuario"]);
                    proforma.fecha_captura = Convert.ToDateTime(rdr["fecha_captura"]);
                    lstProforma.Add(proforma);
                }

                return lstProforma;
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

        public int AddProforma(Proforma proforma)
        {
            string consulta = "";
            consulta += " insert into proforma ( ";
            consulta += " 	id, anio, modelo_negocio_id, tipo_captura_id, tipo_proforma_id, centro_costo_id, activo, usuario, fecha_captura ";
            consulta += " ) values ( ";
            consulta += " 	nextval('seq_proforma'), @anio, @modelo_negocio_id, @tipo_captura_id, @tipo_proforma_id, @centro_costo_id, @activo, @usuario, @fecha_captura ";
            consulta += " ) ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                cmd.Parameters.AddWithValue("@anio", proforma.anio);
                cmd.Parameters.AddWithValue("@modelo_negocio_id", proforma.modelo_negocio_id);
                cmd.Parameters.AddWithValue("@tipo_captura_id", proforma.tipo_captura_id);
                cmd.Parameters.AddWithValue("@tipo_proforma_id", proforma.tipo_proforma_id);
                cmd.Parameters.AddWithValue("@centro_costo_id", proforma.centro_costo_id);
                cmd.Parameters.AddWithValue("@activo", proforma.activo);
                cmd.Parameters.AddWithValue("@usuario", proforma.usuario);
                cmd.Parameters.AddWithValue("@fecha_captura", proforma.fecha_captura);

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

        public int UpdateProforma(int idProforma, bool bandActivo, int idUsuario)
        {
            string consulta = "";
            consulta += " update proforma set activo = '" + bandActivo.ToString() + "', ";
            consulta += " 	usuario = " + idUsuario.ToString() + ", fecha_captura = current_timestamp ";
            consulta += " 	where id = " + idProforma.ToString();

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

        // Metodo a invocar para crear la proforma
        public int GeneraProforma(Int64 idCC, int anio, Int64 idTipoProforma)
        {
            // Se reciben centro de costos, anio y tipo de proforma
            // Del centro de costos obtiene empresa y proyecto
            // Del tipo de proforma obtiene 
            return 0;
        }

        public ProformaDetalle ConstruyeDetalleTotal(List<ProformaDetalle> detalles, Rubros rubroTotal)
        {
            var aritmeticas = new Dictionary<string, string>();
            aritmeticas.Add("enero", rubroTotal.aritmetica);
            aritmeticas.Add("febrero", rubroTotal.aritmetica);
            aritmeticas.Add("marzo", rubroTotal.aritmetica);
            aritmeticas.Add("abril", rubroTotal.aritmetica);
            aritmeticas.Add("mayo", rubroTotal.aritmetica);
            aritmeticas.Add("junio", rubroTotal.aritmetica);
            aritmeticas.Add("julio", rubroTotal.aritmetica);
            aritmeticas.Add("agosto", rubroTotal.aritmetica);
            aritmeticas.Add("septiembre", rubroTotal.aritmetica);
            aritmeticas.Add("octubre", rubroTotal.aritmetica);
            aritmeticas.Add("noviembre", rubroTotal.aritmetica);
            aritmeticas.Add("diciembre", rubroTotal.aritmetica);

            detalles.ForEach(detalle =>
            {
                Rubros rubrosCta = BuscaRubroPorId(detalle.rubro_id);
                if (rubroTotal.aritmetica.Contains(rubrosCta.clave))
                {
                    aritmeticas["enero"] = aritmeticas["enero"].Replace(rubrosCta.clave, detalle.enero_monto_resultado.ToString());
                    aritmeticas["febrero"] = aritmeticas["febrero"].Replace(rubrosCta.clave, detalle.febrero_monto_resultado.ToString());
                    aritmeticas["marzo"] = aritmeticas["marzo"].Replace(rubrosCta.clave, detalle.marzo_monto_resultado.ToString());
                    aritmeticas["abril"] = aritmeticas["abril"].Replace(rubrosCta.clave, detalle.abril_monto_resultado.ToString());
                    aritmeticas["mayo"] = aritmeticas["mayo"].Replace(rubrosCta.clave, detalle.mayo_monto_resultado.ToString());
                    aritmeticas["junio"] = aritmeticas["junio"].Replace(rubrosCta.clave, detalle.junio_monto_resultado.ToString());
                    aritmeticas["julio"] = aritmeticas["julio"].Replace(rubrosCta.clave, detalle.julio_monto_resultado.ToString());
                    aritmeticas["agosto"] = aritmeticas["agosto"].Replace(rubrosCta.clave, detalle.agosto_monto_resultado.ToString());
                    aritmeticas["septiembre"] = aritmeticas["septiembre"].Replace(rubrosCta.clave, detalle.septiembre_monto_resultado.ToString());
                    aritmeticas["octubre"] = aritmeticas["octubre"].Replace(rubrosCta.clave, detalle.octubre_monto_resultado.ToString());
                    aritmeticas["noviembre"] = aritmeticas["noviembre"].Replace(rubrosCta.clave, detalle.noviembre_monto_resultado.ToString());
                    aritmeticas["diciembre"] = aritmeticas["diciembre"].Replace(rubrosCta.clave, detalle.diciembre_monto_resultado.ToString());
                }

            });
            ProformaDetalle proformaDetalleTotal = new ProformaDetalle();
            proformaDetalleTotal.rubro_id = rubroTotal.id;

            DataTable dt = new DataTable();
            proformaDetalleTotal.enero_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["enero"], ""));
            proformaDetalleTotal.febrero_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["febrero"], ""));
            proformaDetalleTotal.marzo_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["marzo"], ""));
            proformaDetalleTotal.abril_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["abril"], ""));
            proformaDetalleTotal.mayo_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["mayo"], ""));
            proformaDetalleTotal.junio_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["junio"], ""));
            proformaDetalleTotal.julio_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["julio"], ""));
            proformaDetalleTotal.agosto_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["agosto"], ""));
            proformaDetalleTotal.septiembre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["septiembre"], ""));
            proformaDetalleTotal.octubre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["octubre"], ""));
            proformaDetalleTotal.noviembre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["noviembre"], ""));
            proformaDetalleTotal.diciembre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["diciembre"], ""));
            return proformaDetalleTotal;
        }


        public List<ProformaDetalle> CompletaDetalles(List<ProformaDetalle> detCtas, Int64 idModelo)
        {
            List<Rubros> rubTots = GetRubrosTotales(idModelo);
            List<ProformaDetalle> totales = new List<ProformaDetalle>();
            foreach (Rubros rubTot in rubTots)
            {
                ProformaDetalle detTot = ConstruyeDetalleTotal(detCtas, rubTot);
                totales.Add(detTot);
            }
            detCtas.AddRange(totales);
            return detCtas;
        }

        public List<Rubros> GetRubrosTotales(Int64 idModelo)
        {
            string consulta = "";
            consulta += " select rub.id, rub.nombre, rub.clave, rub.aritmetica ";
            consulta += " 	from rubro rub ";
            consulta += " 	inner join tipo_rubro tip on rub.tipo_id = tip.id ";
            consulta += " 	where rub.id_modelo_neg = " + idModelo.ToString();
            consulta += " 	and tip.clave = 'RUBROS' ";
            consulta += " 	and rub.activo = 'true' ";

            try
            {
                List<Rubros> lstRubrosTot = new List<Rubros>();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Rubros rubsObtenidos = new Rubros();
                    rubsObtenidos.id = Convert.ToInt32(rdr["id"]);
                    rubsObtenidos.nombre = Convert.ToString(rdr["nombre"]);
                    rubsObtenidos.clave = Convert.ToString(rdr["clave"]);
                    rubsObtenidos.aritmetica = Convert.ToString(rdr["aritmetica"]);
                    lstRubrosTot.Add(rubsObtenidos);
                }

                return lstRubrosTot;
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

        public Rubros BuscaRubroPorId(Int64 rubro_id)
        {
            string consulta = "";
            consulta += " select id_modelo_neg, tipo_id, clave, aritmetica, naturaleza ";
            consulta += " from rubro ";
            consulta += " where id_rubro = " + rubro_id.ToString();
            consulta += " and activo = 'true' ";

            try
            {
                Rubros detRubros = new Rubros();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                detRubros.id_modelo_neg = Convert.ToInt32(rdr["id_modelo_neg"]);
                detRubros.tipo_id = Convert.ToInt32(rdr["tipo_id"]);
                detRubros.clave = Convert.ToString(rdr["clave"]);
                detRubros.aritmetica = Convert.ToString(rdr["aritmetica"]);
                detRubros.naturaleza = Convert.ToString(rdr["naturaleza"]);

                return detRubros;
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

        public List<ProformaDetalle> CalculaDetalleProforma(Int64 idCC,int anio,Int64 idTipoProforma)
        {
            ///obtener las variables
            ProformaDetalleDataAccessLayer objProfDetalle = new ProformaDetalleDataAccessLayer();
          
            // Obtiene las listas a partir de los parametros recibidos
            // Montos consolidados para ejercicio
            List<ProformaDetalle> lstGetProfDet= objProfDetalle.GetProformaCalculada(1, 1, 1, 1, 1, 1, 1, 1);
            // Sumatorias para el acumulado
            List<ProformaDetalle> lstGetEjerc = objProfDetalle.GetEjercicioAnterior(1, 1, 1, 1, 1, 1, 1, true, 1);

            // Genera una lista para almacenar la informacion consultada
            foreach (ProformaDetalle itemProfDet in lstGetProfDet)
            {
   
                foreach (ProformaDetalle itemSumProfDet in lstGetEjerc)
                {
                    // Compara elementos para completar la lista
                    if(itemProfDet.rubro_id == itemSumProfDet.rubro_id)
                    {
                        // Si coincide el rubro se guardan los acumulados anteriores
                        itemProfDet.acumulado_financiero = itemSumProfDet.acumulado_financiero;
                        itemProfDet.acumulado_resultado = itemSumProfDet.acumulado_resultado;
                        itemProfDet.total_financiero = itemSumProfDet.acumulado_financiero + itemSumProfDet.ejercicio_financiero;
                        itemProfDet.total_resultado = itemSumProfDet.acumulado_resultado + itemSumProfDet.ejercicio_resultado;
                        break;
                    }
                }
            }

            return lstGetProfDet;
        }
        
        // Metodo para almacenar una proforma
        public int GuardaProforma()
        {
            // Toma los datos de pantalla
            // Inserta en la tabla proforma
            // Inserta en la tabla proforma_detalle
            return 0;
        }

    }
}
