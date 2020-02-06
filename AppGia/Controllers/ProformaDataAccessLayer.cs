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
                
                 cmd = new NpgsqlCommand("select currval('seq_proforma') as idproforma", con);
                 NpgsqlDataReader reader = cmd.ExecuteReader();
                 if (reader.Read())
                 {
                     proforma.id=Convert.ToInt32(reader["idproforma"]);
                 }
                
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

        // Metodo a invocar para crear la proforma (cambiar por lista)
        // Parametros de entrada: centro de costos, anio y tipo de proforma
        public List<ProformaDetalle> GeneraProforma(Int64 idCC, int anio, Int64 idTipoProforma)
        {
            // Del centro de costos se obtienen empresa y proyecto
            CentroCostos datCenCos = new CentroCostos();
            datCenCos = ObtenerDatosCC(idCC);
            int idEmpresa = datCenCos.empresa_id;
            int idProyecto = datCenCos.proyecto_id;

            // De la empresa se obtiene el modelo de negocio
            Proyecto datProyec = new Proyecto();
            datProyec = ObtenerDatosProy(idEmpresa);
            int idModeloNeg = datProyec.modelo_negocio_id;

            // Del tipo de proforma obtiene mes de inicio
            Tipo_Proforma datTipoProf = new Tipo_Proforma();
            datTipoProf = ObtenerDatosTipoProf(idTipoProforma);
            int mesInicio = datTipoProf.mes_inicio;

            // Obtiene detalle de la proforma calculada con montos, ejercicio y acuumulado
            List<ProformaDetalle> listDetProformaCalc = CalculaDetalleProforma(mesInicio, idEmpresa, idModeloNeg, idProyecto, anio, Convert.ToInt32(idTipoProforma));

            // Enlista la proforma
            List<ProformaDetalle> lstProformaCompleta = CompletaDetalles(listDetProformaCalc, idModeloNeg);

            return lstProformaCompleta;
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
            aritmeticas.Add("ejercicio", rubroTotal.aritmetica);
            aritmeticas.Add("acumulado", rubroTotal.aritmetica);
            aritmeticas.Add("total", rubroTotal.aritmetica);

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
                    aritmeticas["ejercicio"] = aritmeticas["ejercicio"].Replace(rubrosCta.clave, detalle.ejercicio_resultado.ToString());
                    aritmeticas["acumulado"] = aritmeticas["acumulado"].Replace(rubrosCta.clave, detalle.acumulado_resultado.ToString());
                    aritmeticas["total"] = aritmeticas["total"].Replace(rubrosCta.clave, detalle.total_resultado.ToString());
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
            proformaDetalleTotal.ejercicio_resultado = Convert.ToDouble(dt.Compute(aritmeticas["ejercicio"], ""));
            proformaDetalleTotal.acumulado_resultado = Convert.ToDouble(dt.Compute(aritmeticas["acumulado"], ""));
            proformaDetalleTotal.total_resultado = Convert.ToDouble(dt.Compute(aritmeticas["total"], ""));
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
            consulta += " 	from rubro ";
            consulta += " 	where id_rubro = " + rubro_id.ToString();
            consulta += " 	and activo = 'true' ";

            try
            {
                Rubros detRubros = new Rubros();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    detRubros.id_modelo_neg = Convert.ToInt32(rdr["id_modelo_neg"]);
                    detRubros.tipo_id = Convert.ToInt32(rdr["tipo_id"]);
                    detRubros.clave = Convert.ToString(rdr["clave"]);
                    detRubros.aritmetica = Convert.ToString(rdr["aritmetica"]);
                    detRubros.naturaleza = Convert.ToString(rdr["naturaleza"]);
                }

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

        public CentroCostos ObtenerDatosCC(Int64 idCenCos)
        {
            string consulta = "";
            consulta += " select empresa_id, proyecto_id ";
            consulta += " 	from centro_costo ";
            consulta += " 	where id = " + idCenCos.ToString();
            consulta += " 	and activo = 'true' ";

            try
            {
                CentroCostos datosCenCos = new CentroCostos();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    datosCenCos.empresa_id = Convert.ToInt32(rdr["empresa_id"]);
                    datosCenCos.proyecto_id = Convert.ToInt32(rdr["proyecto_id"]);
                }

                return datosCenCos;
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

        public Proyecto ObtenerDatosProy(Int64 idEmpresa)
        {
            string consulta = "";
            consulta += " select id, modelo_negocio_id ";
            consulta += " 	from proyecto ";
            consulta += " 	where id = " + idEmpresa.ToString();
            consulta += " 	and activo = 'true' ";

            try
            {
                Proyecto datosProy = new Proyecto();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    datosProy.id = Convert.ToInt32(rdr["id"]);
                    datosProy.modelo_negocio_id = Convert.ToInt32(rdr["modelo_negocio_id"]);
                }

                return datosProy;
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

        public Tipo_Proforma ObtenerDatosTipoProf(Int64 idTipoProforma)
        {
            string consulta = "";
            consulta += " select id, clave, mes_inicio ";
            consulta += " 	from tipo_proforma ";
            consulta += " 	where id = " + idTipoProforma.ToString();
            consulta += " 	and activo = 'true' ";

            try
            {
                Tipo_Proforma datosTipoProf = new Tipo_Proforma();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    datosTipoProf.id = Convert.ToInt64(rdr["id"]);
                    datosTipoProf.clave = Convert.ToString(rdr["clave"]);
                    datosTipoProf.mes_inicio = Convert.ToInt32(rdr["mes_inicio"]);
                }

                return datosTipoProf;
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

        public List<ProformaDetalle> CalculaDetalleProforma(int mesInicio, int idEmpresa, int idModeloNeg, int idProyecto, int anio, int idTipoCaptura)
        {
            ///obtener las variables
            ProformaDetalleDataAccessLayer objProfDetalle = new ProformaDetalleDataAccessLayer();
          
            // Obtiene lista de montos consolidados para ejercicio
            List<ProformaDetalle> lstGetProfDet= objProfDetalle.GetProformaCalculada(mesInicio, idEmpresa, idModeloNeg, idProyecto, anio, idTipoCaptura);
            // Obtiene lista de sumatorias para el acumulado
            List<ProformaDetalle> lstGetEjerc = objProfDetalle.GetEjercicioAnterior(mesInicio, idEmpresa, idModeloNeg, idProyecto, anio, idTipoCaptura);

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
        public int GuardaProforma(List<ProformaDetalle> detalles)
        {
            Proforma proforma=new Proforma();
            proforma.activo = true;
            proforma.anio=detalles[0].anio;
            //proforma.usuario=;
            proforma.modelo_negocio_id=detalles[0].modelo_negocio_id;
            //proforma.tipo_proforma_id;
            proforma.tipo_captura_id=detalles[0].tipo_captura_id;
            //proforma.centro_costo_id=detalles[0].;
            proforma.fecha_captura=new DateTime();
            AddProforma(proforma);
            detalles.ForEach(detalle =>
            {
                detalle.id_proforma = proforma.id;
                detalle.activo = true;
                new ProformaDetalleDataAccessLayer().AddProformaDetalle(detalle);    
            });
            
            // Toma los datos de pantalla
            // Inserta en la tabla proforma
            // Inserta en la tabla proforma_detalle
            return 0;
        }

    }
}
