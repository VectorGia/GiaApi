using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AppGia.Controllers;
using AppGia.Helpers;
using AppGia.Models;
using Npgsql;
using static AppGia.Util.Constantes;

namespace AppGia.Dao
{
    public class ProformaDataAccessLayer
    {
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
            consulta += " 		pf.centro_costo_id, pf.activo, pf.usuario, pf.fecha_captura, ";
            consulta += " 		upper(concat(py.nombre, ' - ' , mm.nombre, ' - ', tip.nombre)) as nombre_proforma ";
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
            proforma.fecha_captura = Convert.ToDateTime(rdr["fecha_captura"]);
            proforma.nombre_proforma = Convert.ToString(rdr["nombre_proforma"]);
            return proforma;
        }

        public List<Proforma> GetAllProformas()
        {
            string consulta = "";
            consulta += " select ";
            consulta += " 		pf.id, pf.anio, pf.modelo_negocio_id, pf.tipo_captura_id, pf.tipo_proforma_id, ";
            consulta += " 		pf.centro_costo_id, pf.activo, pf.usuario, pf.fecha_captura, ";
            consulta += " 		upper(concat(py.nombre, ' - ' , mm.nombre, ' - ', tip.nombre)) as nombre_proforma ";
            consulta += " from proforma pf ";
            consulta += " inner join modelo_negocio mm on mm.id = pf.modelo_negocio_id ";
            consulta += " inner join centro_costo cc on cc.id = pf.centro_costo_id ";
            consulta += " inner join proyecto py on py.id = cc.proyecto_id ";
            consulta += " inner join tipo_proforma tip on pf.tipo_proforma_id = tip.id ";
            consulta += " where pf.activo = 'true' ";

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
                " 	id, anio, modelo_negocio_id, tipo_captura_id, tipo_proforma_id, centro_costo_id, activo, usuario, fecha_captura ";
            consulta += " ) values ( ";
            consulta +=
                " 	nextval('seq_proforma'), @anio, @modelo_negocio_id, @tipo_captura_id, @tipo_proforma_id, @centro_costo_id, @activo, @usuario, @fecha_captura ";
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
                    cmd.Parameters.AddWithValue("@fecha_actualizacion", new DateTime());
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
            string proyeccion = ObtenerDatosCC(idCC).proyeccion;
            List<ProformaDetalle> detalles = null;
            if (proyeccion.Equals(ProyeccionBase))
            {
                detalles = GeneraProforma(idCC, anio, idTipoProforma, idTipoCaptura);
                detalles = _profHelper.reorderConceptos(detalles);
            }

            if (proyeccion.Equals(ProyeccionMetodo))
            {
                detalles = _profHelper.BuildProformaFromModeloAsTemplate(idCC, anio, idTipoProforma, idTipoCaptura);
            }

            if (proyeccion.Equals(ProyeccionShadow))
            {
                Int64 idTipoProforma012 = getIdTipoProformaByClave(ClaveProforma012);
                detalles = _profHelper.BuildProformaFromModeloAsTemplate(idCC, anio, idTipoProforma012, idTipoCaptura);
            }

            Boolean hayPeriodoActivo=_profHelper.existePeridodoActivo( anio,  idTipoProforma,  idTipoCaptura);
            detalles.ForEach(detalle => { detalle.editable = hayPeriodoActivo;});
            

            if (detalles != null)
            {
                return _profHelper.setIdInterno(detalles);
            }

            throw new ArgumentException("La proyeccion '" + proyeccion + "' no es soportada");
        }
        
        // Metodo a invocar para crear la proforma (cambiar por lista)
        // Parametros de entrada: centro de costos, anio y tipo de proforma
        private List<ProformaDetalle> GeneraProforma(Int64 idCC, int anio, Int64 idTipoProforma, Int64 idTipoCaptura)
        {
            
            if (anio >  DateTime.Now.Year)
            {
                return _profHelper.BuildProformaFromModeloAsTemplate(idCC, anio, getIdTipoProformaByClave(ClaveProforma012), idTipoCaptura);
            }
            
            // Del centro de costos se obtienen empresa y proyecto
            CentroCostos cc = ObtenerDatosCC(idCC);

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
                throw new InvalidDataException("No existe información con fecha '" + fechaProf + "' para proforma " +
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
                detalle.tipo_proforma_id = idTipoProforma;
                detalle.tipo_captura_id = idTipoCaptura;
                detalle.modelo_negocio_id = idModeloNeg;
            });
            listDetProformaCalc.ForEach(detalle => { detalle.mes_inicio = mesInicio;});
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
            List<ProformaDetalle> detallesCalculados = detalleAccesLayer.GetProformaCalculada(idCenCos, mesInicio, idEmpresa,
                idModeloNeg, idProyecto, anio, idTipoCaptura);
            // Obtiene lista de sumatorias para el acumulado
            List<ProformaDetalle> detallesAniosAnteriores = detalleAccesLayer.GetAcumuladoAnteriores(idCenCos, idEmpresa, idModeloNeg,
                idProyecto, anio, idTipoCaptura);
            // Obtiene montos para anios posteriores
            List<ProformaDetalle> detalleAniosPosteriores =
                detalleAccesLayer.GetEjercicioPosterior(anio, idCenCos, idModeloNeg, idTipoCaptura, idTipoProforma);

         
            // Genera una lista para almacenar la informacion consultada
            foreach (ProformaDetalle detalleCalculado in detallesCalculados)
            {
                detalleCalculado.total_resultado = detalleCalculado.ejercicio_resultado;
                foreach (ProformaDetalle detalleAnioAnt in detallesAniosAnteriores)
                {
                    // Compara elementos para completar la lista
                    if (detalleCalculado.rubro_id == detalleAnioAnt.rubro_id)
                    {
                        // Si coincide el rubro se guardan los acumulados anteriores
                        detalleCalculado.acumulado_resultado = detalleAnioAnt.acumulado_resultado;
                        // Se actualiza el total como la suma del ejercicio + el acumulado
                        detalleCalculado.total_resultado += detalleAnioAnt.acumulado_resultado;
                        break;
                    }
                }
                
                foreach (ProformaDetalle detalleAnioPost in detalleAniosPosteriores)
                {
                    if (detalleCalculado.rubro_id == detalleAnioPost.rubro_id)
                    {
                        // Si coincide el rubro se guardan los acumulados de anios posteriores
                        detalleCalculado.anios_posteriores_resultado = detalleAnioPost.anios_posteriores_resultado;
                    }
                }
            }

            return detallesCalculados;
        }

        // Metodo para almacenar una proforma
        public int GuardaProforma(List<ProformaDetalle> detalles)
        {
            Proforma proforma = new Proforma();
            DateTime fechaProc = DateTime.Today;
            proforma.activo = true;
            proforma.anio = detalles[0].anio;
            proforma.usuario = detalles[0].usuario;
            proforma.modelo_negocio_id = detalles[0].modelo_negocio_id;
            proforma.tipo_proforma_id = detalles[0].tipo_proforma_id;
            proforma.tipo_captura_id = detalles[0].tipo_captura_id;
            proforma.centro_costo_id = detalles[0].centro_costo_id;
            proforma.fecha_captura = fechaProc;
            AddProforma(proforma);
            detalles.ForEach(detalle =>
            {
                detalle.id_proforma = proforma.id;
                detalle.activo = true;
                new ProformaDetalleDataAccessLayer().AddProformaDetalle(detalle);
            });

            return 0;
        }

        public int ActualizaProforma(List<ProformaDetalle> profDetalle)
        {
            Proforma proforma = new Proforma();
            DateTime fechaProc = DateTime.Now;
            proforma.activo = true;
            proforma.usuario = profDetalle[0].usuario;
            proforma.fecha_captura = fechaProc;
            UpdateProforma(proforma);
            profDetalle.ForEach(detalle =>
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
    }
}