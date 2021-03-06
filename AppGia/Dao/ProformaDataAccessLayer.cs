﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AppGia.Helpers;
using AppGia.Models;
using AppGia.Util;
using NLog;
using Npgsql;
using static AppGia.Util.Constantes;

namespace AppGia.Dao
{
    public class ProformaDataAccessLayer
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        private ProformaHelper _profHelper = new ProformaHelper();
        private QueryExecuter _queryExecuter = new QueryExecuter();

        public ProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public Proforma GetProforma(Int64 idProforma)
        {
            string consulta = "";
            consulta += " select ";
            consulta += " 		pf.id, pf.anio, pf.modelo_negocio_id, pf.tipo_captura_id, pf.tipo_proforma_id, ";
            consulta += " 		pf.centro_costo_id, pf.activo, pf.usuario, pf.fecha_captura,pf.fecha_actualizacion,pf.unidad_id, ";
            consulta += " 		upper(concat(py.nombre, ' | ' , mm.nombre, ' | ', tip.nombre)) as nombre_proforma ";
            consulta += " from proforma pf ";
            consulta += " inner join modelo_negocio mm on mm.id = pf.modelo_negocio_id ";
            consulta += " inner join centro_costo cc on cc.id = pf.centro_costo_id ";
            consulta += " inner join proyecto py on py.id = cc.proyecto_id ";
            consulta += " inner join tipo_proforma tip on pf.tipo_proforma_id = tip.id ";
            consulta += " where pf.activo = 'true' ";
            consulta += " and pf.id = " + idProforma;


            DataRow rdr = _queryExecuter.ExecuteQueryUniqueresult(consulta);

            Proforma proforma = new Proforma();
            proforma.id = Convert.ToInt64(rdr["id"]);
            proforma.anio = Convert.ToInt32(rdr["anio"]);
            proforma.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"]);
            proforma.tipo_captura_id = Convert.ToInt64(rdr["tipo_captura_id"]);
            proforma.tipo_proforma_id = Convert.ToInt64(rdr["tipo_proforma_id"]);
            proforma.centro_costo_id = Convert.ToInt64(rdr["centro_costo_id"]);
            proforma.activo = Convert.ToBoolean(rdr["activo"]);
            proforma.usuario = Convert.ToInt64(rdr["usuario"]);
            proforma.unidad_id = Convert.ToInt64(rdr["unidad_id"]);
            proforma.fecha_captura = Convert.ToDateTime(rdr["fecha_captura"]);
            if (rdr["fecha_actualizacion"] != DBNull.Value)
            {
                proforma.fecha_actualizacion = Convert.ToDateTime(rdr["fecha_actualizacion"]);
            }

            proforma.nombre_proforma = Convert.ToString(rdr["nombre_proforma"]);
            return proforma;
        }

        public List<Proforma> GetAllProformas()
        {
            string consulta =
                " select pf.id," +
                " upper(concat(pf.anio, ' | ', tp.nombre, ' | ', tc.clave, ' | ', '(', e.desc_id, ')', e.nombre, ' | '," +
                "    un.descripcion, ' | ', '(', cc.desc_id, ')', cc.nombre)) as nombre_proforma," +
                " pf.anio," +
                " pf.modelo_negocio_id," +
                " pf.tipo_captura_id," +
                " pf.tipo_proforma_id," +
                " pf.centro_costo_id," +
                " pf.activo," +
                " pf.usuario," +
                " pf.fecha_captura," +
                " pf.fecha_actualizacion" +
                "    from proforma pf" +
                " join unidad_negocio un on pf.unidad_id = un.id" +
                " join empresa e on pf.empresa_id = e.id" +
                " inner join modelo_negocio mn on mn.id = pf.modelo_negocio_id" +
                " inner join centro_costo cc on cc.id = pf.centro_costo_id" +
                " inner join proyecto pr on pr.id = cc.proyecto_id" +
                " inner join tipo_proforma tp on pf.tipo_proforma_id = tp.id" +
                " join tipo_captura tc on tc.id = pf.tipo_captura_id" +
                " where pf.activo = 'true' order by pf.fecha_captura desc,pf.fecha_actualizacion desc";

            try
            {
                List<Proforma> lstProforma = new List<Proforma>();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Proforma proforma = new Proforma();
                    proforma.id = Convert.ToInt64(rdr["id"]);
                    proforma.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"]);
                    proforma.tipo_captura_id = Convert.ToInt64(rdr["tipo_captura_id"]);
                    proforma.tipo_proforma_id = Convert.ToInt64(rdr["tipo_proforma_id"]);
                    proforma.centro_costo_id = Convert.ToInt64(rdr["centro_costo_id"]);
                    proforma.activo = Convert.ToBoolean(rdr["activo"]);
                    proforma.usuario = Convert.ToInt64(rdr["usuario"]);
                    proforma.fecha_captura = Convert.ToDateTime(rdr["fecha_captura"]);
                    if (rdr["fecha_actualizacion"] != DBNull.Value)
                    {
                        proforma.fecha_actualizacion = Convert.ToDateTime(rdr["fecha_actualizacion"]);
                    }

                    proforma.nombre_proforma = Convert.ToString(rdr["nombre_proforma"]);
                    lstProforma.Add(proforma);
                }

                return lstProforma;
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
            consulta +=
                " 	id, anio, modelo_negocio_id, tipo_captura_id, tipo_proforma_id, centro_costo_id, activo, usuario, fecha_captura,unidad_id,empresa_id ";
            consulta += " ) values ( ";
            consulta +=
                " 	nextval('seq_proforma'), @anio, @modelo_negocio_id, @tipo_captura_id, @tipo_proforma_id, @centro_costo_id, @activo, @usuario, @fecha_captura,@unidad_id,@empresa_id ";
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
                cmd.Parameters.AddWithValue("@unidad_id", proforma.unidad_id);
                cmd.Parameters.AddWithValue("@empresa_id", proforma.empresa_id);

                int regInsert = cmd.ExecuteNonQuery();

                cmd = new NpgsqlCommand("select currval('seq_proforma') as idproforma", con);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    proforma.id = Convert.ToInt64(reader["idproforma"]);
                }

                return regInsert;
            }
            finally
            {
                con.Close();
            }
        }


        public int UpdateProforma(Proforma proforma)
        {
            string consulta = "";
            consulta += " update proforma set activo = '" + proforma.activo + "', ";
            consulta += " 	usuario = " + proforma.usuario + ", fecha_actualizacion = @fecha_actualizacion ";
            consulta += " 	where id = " + proforma.id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                    cmd.Parameters.AddWithValue("@fecha_actualizacion", DateTime.Now);
                    int regActual = cmd.ExecuteNonQuery();
                    return regActual;
                }
            }
            finally
            {
                con.Close();
            }
        }

        public List<ProformaDetalle> manageBuildProforma(Int64 idCC, int anio, Int64 idTipoProforma,
            Int64 idTipoCaptura)
        {
            CentroCostos cc = ObtenerDatosCC(idCC);
            string proyeccion = cc.proyeccion;
            List<ProformaDetalle> detalles;
            if (proyeccion.Equals(ProyeccionBase))
            {
                detalles = GeneraProforma(idCC, anio, idTipoProforma, idTipoCaptura);
                detalles = _profHelper.reorderConceptos(detalles);
            }
            else if (proyeccion.Equals(ProyeccionMetodo))
            {
                detalles = _profHelper.BuildProformaFromModeloAsTemplate(cc, anio, idTipoProforma, idTipoCaptura);
            }
            else if (proyeccion.Equals(ProyeccionShadow))
            {
                Int64 idTipoProforma012 = getIdTipoProformaByClave(ClaveProforma012);
                detalles = _profHelper.BuildProformaFromModeloAsTemplate(cc, anio, idTipoProforma012, idTipoCaptura);
            }
            else
            {
                throw new ArgumentException("La proyeccion '" + proyeccion + "' no es soportada");
            }

            Boolean hayPeriodoActivo=_profHelper.existePeridodoActivo( anio,  idTipoProforma,  idTipoCaptura);
            detalles.ForEach(detalle => { detalle.editable = hayPeriodoActivo;});
            
            return _profHelper.setIdInterno(detalles);

        }
        
        
        // Metodo a invocar para crear la proforma (cambiar por lista)
        // Parametros de entrada: centro de costos, anio y tipo de proforma
        private List<ProformaDetalle> GeneraProforma(Int64 idCC, int anio, Int64 idTipoProforma, Int64 idTipoCaptura)
        {
            
            // Del centro de costos se obtienen empresa y proyecto
            CentroCostos cc = ObtenerDatosCC(idCC);
            if (anio >  DateTime.Now.Year)
            {
                return _profHelper.BuildProformaFromModeloAsTemplate(cc, anio, getIdTipoProformaByClave(ClaveProforma012), idTipoCaptura);
            }
            

            if (cc == null)
            {
                throw new InvalidDataException("No hay informacion del centro de costos " + idCC);
            }


            // Del tipo de proforma obtiene mes de inicio
            int mesInicio = _profHelper.getMesInicio(idTipoProforma);
            if (mesInicio < 0)
            {
                throw new InvalidDataException("Error en el mes de inicio de la proforma ");
            }

            Int64 idModeloNeg = idTipoCaptura == TipoCapturaContable
                ? cc.modelo_negocio_id
                : cc.modelo_negocio_flujo_id;
            // Obtiene detalle de la proforma calculada con montos, ejercicio y acuumulado
            List<ProformaDetalle> listDetProformaCalc = CalculaDetalleProforma(idCC, mesInicio,
                cc.empresa_id, idModeloNeg,
                cc.proyecto_id, anio, idTipoCaptura, idTipoProforma);

            if (listDetProformaCalc.Count == 0)
            {
                DateTime fechaProf = DateTime.Today;
                string nombreTipoCaptura = idTipoCaptura == TipoCapturaContable ? "contable" : "de flujo";
                throw new InvalidDataException("No existe informacion con fecha '" + fechaProf + "' para proforma " +
                                               nombreTipoCaptura + " de la empresa '" + cc.empresa_id +
                                               "' y modelo de negocio '" + idModeloNeg + "'");
            }

            // se contruyen los rubros totales
            List<ProformaDetalle> lstProformaCompleta =
                _profHelper.CompletaDetalles(listDetProformaCalc, cc, idModeloNeg);
            lstProformaCompleta.ForEach(detalle =>
            {
                detalle.centro_costo_id = idCC;
                detalle.anio = anio;
                detalle.empresa_id = cc.empresa_id;
                detalle.tipo_proforma_id = idTipoProforma;
                detalle.tipo_captura_id = idTipoCaptura;
                detalle.modelo_negocio_id = idModeloNeg;
            });
            listDetProformaCalc.ForEach(detalle => { detalle.mes_inicio = mesInicio;});
            if (idTipoCaptura == TipoCapturaContable)
            {
                ProformaCalc.roundMontosInDetalles(lstProformaCompleta);
            }
            return lstProformaCompleta;
        }


        public CentroCostos ObtenerDatosCC(Int64 idCenCos)
        {
            string consulta = "";
            consulta +=
                " select empresa_id, proyecto_id, modelo_negocio_id,modelo_negocio_flujo_id,porcentaje,proyeccion ";
            consulta += " 	from centro_costo ";
            consulta += " 	where id = " + idCenCos;
            consulta +=
                " 	and activo = 'true' and modelo_negocio_id is not null and  modelo_negocio_flujo_id is not null";

            try
            {
                CentroCostos datosCenCos = new CentroCostos();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                bool existenDatos = false;
                while (rdr.Read())
                {
                    existenDatos = true;
                    datosCenCos.empresa_id = Convert.ToInt64(rdr["empresa_id"]);
                    datosCenCos.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"]);
                    datosCenCos.modelo_negocio_flujo_id = Convert.ToInt64(rdr["modelo_negocio_flujo_id"]);
                    datosCenCos.proyecto_id = Convert.ToInt64(rdr["proyecto_id"]);
                    datosCenCos.proyeccion = rdr["proyeccion"].ToString();
                    datosCenCos.id = idCenCos;


                    Object porcentajeFromDb = rdr["porcentaje"];
                    if (porcentajeFromDb != null)
                    {
                        datosCenCos.porcentaje = Convert.ToDouble(porcentajeFromDb);
                    }
                    else
                    {
                        datosCenCos.porcentaje = 1.0;
                    }
                }

                if (existenDatos)
                {
                    return datosCenCos;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                con.Close();
            }
        }


  
        public List<ProformaDetalle> CalculaDetalleProforma(Int64 idCenCos, int mesInicio, Int64 idEmpresa,
            Int64 idModeloNeg, Int64 idProyecto, int anio, Int64 idTipoCaptura, Int64 idTipoProforma)
        {
            ///obtener las variables
            ProformaDetalleDataAccessLayer detalleAccesLayer = new ProformaDetalleDataAccessLayer();

            // Obtiene lista de montos consolidados para ejercicio
            log.Info("Calculando proforma ....");
            List<ProformaDetalle> detallesCalculados = detalleAccesLayer.GetProformaCalculada(idCenCos, mesInicio, idEmpresa,
                idModeloNeg, idProyecto, anio, idTipoCaptura);
            //en ocasiones no hay montos para ciertos rubros, eso nos obliga a llenar con 0 el detalle de ese rubro
            List<ProformaDetalle> detallesFromModel = _profHelper.buildProformaFromTemplate(
                _profHelper.GetRubrosFromModeloId(idModeloNeg, false), idCenCos, anio, idTipoProforma, idTipoCaptura);
            List<ProformaDetalle> mergeDetalle=mergeDetallesMontosVsDetallesModelo(detallesFromModel,detallesCalculados);

            
            // Obtiene lista de sumatorias para el acumulado
            List<ProformaDetalle> detallesAniosAnteriores = detalleAccesLayer.GetAcumuladoAnteriores(idCenCos, idEmpresa, idModeloNeg,
                idProyecto, anio, idTipoCaptura);
            // Obtiene montos para anios posteriores
            List<ProformaDetalle> detalleAniosPosteriores =
                detalleAccesLayer.GetEjercicioPosterior(anio, idCenCos, idModeloNeg, idTipoCaptura, idTipoProforma);

            //--> se colocan los anios anteriores
            _profHelper.manageAniosAnteriores(mergeDetalle,detallesAniosAnteriores);
            
            // Genera una lista para almacenar la informacion consultada
            foreach (ProformaDetalle detalleCalculado in mergeDetalle)
            {
                foreach (ProformaDetalle detalleAnioPost in detalleAniosPosteriores)
                {
                    if (detalleCalculado.rubro_id == detalleAnioPost.rubro_id)
                    {
                        // Si coincide el rubro se guardan los acumulados de anios posteriores
                        detalleCalculado.anios_posteriores_resultado = detalleAnioPost.anios_posteriores_resultado;
                    }
                }
            }
            
            return mergeDetalle;
        }

        private List<ProformaDetalle> mergeDetallesMontosVsDetallesModelo(List<ProformaDetalle> detallesFromModel,
            List<ProformaDetalle> detallesFromMontos)
        {
            List<ProformaDetalle> mergeDetalle = new List<ProformaDetalle>();
            detallesFromModel.ForEach(detalleModel =>
            {
                if (detalleModel.aritmetica.Length == 0)
                {
                    var found = detallesFromMontos.Find(detalleMonto =>
                    {
                        return detalleModel.rubro_id == detalleMonto.rubro_id;
                    });
                    if (found != null)
                    {
                        mergeDetalle.Add(found);
                    }
                    else
                    {
                        mergeDetalle.Add(detalleModel);
                    }
                }
            });
            return mergeDetalle;
        }

        public void validadNoDuplicateProforms(Proforma proforma)
        {
            var res=_queryExecuter.ExecuteQueryUniqueresult(
                "select count(1) as numexist " +
                " from proforma where activo=true " +
                " and anio=@anio and tipo_proforma_id=@tipo_proforma_id " +
                " and tipo_captura_id=@tipo_captura_id and empresa_id=@empresa_id  " +
                " and centro_costo_id=@centro_costo_id",
                new NpgsqlParameter("@anio", proforma.anio),
                new NpgsqlParameter("@tipo_proforma_id", proforma.tipo_proforma_id),
                new NpgsqlParameter("@tipo_captura_id", proforma.tipo_captura_id),
                new NpgsqlParameter("@empresa_id", proforma.empresa_id),
                new NpgsqlParameter("@centro_costo_id", proforma.centro_costo_id));
            if (Convert.ToInt16(res["numexist"]) > 0)
            {
                throw new DataException("Ya existe la proforma, si desea modificarla, vaya al menu, consultela y editela  ");
            }
        }
        // Metodo para almacenar una proforma
        public int GuardaProforma(List<ProformaDetalle> detalles)
        {
            Proforma proforma = new Proforma();
            DateTime fechaProc = DateTime.Now;
            proforma.activo = true;
            proforma.anio = detalles[0].anio;
            proforma.usuario = detalles[0].usuario;
            proforma.modelo_negocio_id = detalles[0].modelo_negocio_id;
            proforma.tipo_proforma_id = detalles[0].tipo_proforma_id;
            proforma.tipo_captura_id = detalles[0].tipo_captura_id;
            proforma.centro_costo_id = detalles[0].centro_costo_id;
            proforma.unidad_id = detalles[0].unidad_id;
            proforma.empresa_id = detalles[0].empresa_id;
            proforma.fecha_captura = fechaProc;
            validadNoDuplicateProforms(proforma);
            _profHelper.setMotoRealesAndProform(detalles);
            AddProforma(proforma);
            detalles.ForEach(detalle =>
            {
                detalle.id_proforma = proforma.id;
                detalle.activo = true;
                new ProformaDetalleDataAccessLayer().AddProformaDetalle(detalle);
            });

            return 0;
        }

        public int ActualizaProforma(List<ProformaDetalle> detalles)
        {
            Proforma proforma = new Proforma();
            DateTime fechaProc = DateTime.Now;
            proforma.activo = true;
            proforma.usuario = detalles[0].usuario;
            proforma.fecha_actualizacion = fechaProc;
            proforma.id = detalles[0].id_proforma;
            _profHelper.setMotoRealesAndProform(detalles);
            UpdateProforma(proforma);
            detalles.ForEach(detalle =>
            {
                detalle.id_proforma = proforma.id;
                detalle.activo = true;
                new ProformaDetalleDataAccessLayer().UpdateProformaDetalle(detalle);
            });

            return 0;
        }
        private Int64 getIdTipoProformaByClave(string claveProforma)
        {
            DataRow dataRow = _queryExecuter.ExecuteQueryUniqueresult("select id from tipo_proforma where clave='"+claveProforma+"'");
            return Convert.ToInt64(dataRow["id"]);
        }

        public int Delete(string id)
        {
            bool status = false;
            string delete = " update proforma set activo = '" + status + "' "
                          + " where id ='" + id + "'";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
            }
            catch
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }

    
}