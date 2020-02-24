using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AppGia.Models;
using Npgsql;
using static AppGia.Util.Constantes;

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
                     proforma.id=Convert.ToInt64(reader["idproforma"]);
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
            consulta += " 	usuario = " + proforma.usuario + ", fecha_captura = current_timestamp ";
            consulta += " 	where id = " + proforma.id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);

                    int regActual = cmd.ExecuteNonQuery();
                    return regActual;
                }
            }
            finally
            {
                con.Close();
            }
        }

        // Metodo a invocar para crear la proforma (cambiar por lista)
        // Parametros de entrada: centro de costos, anio y tipo de proforma
        public List<ProformaDetalle> GeneraProforma(Int64 idCC, int anio, Int64 idTipoProforma, Int64 idTipoCaptura)
        {
            // Del centro de costos se obtienen empresa y proyecto
            CentroCostos cc =  ObtenerDatosCC(idCC);
            
            if(cc == null )
            {
                throw new InvalidDataException("No hay informacion del centro de costos " + idCC);
            }
            

            // Del tipo de proforma obtiene mes de inicio
            Tipo_Proforma datTipoProf = ObtenerDatosTipoProf(idTipoProforma);
            if (datTipoProf.mes_inicio < 0)
            {
                throw new InvalidDataException("Error en el mes de inicio de la proforma ");
            }

            Int64 idModeloNeg = idTipoCaptura == TipoCapturaContable
                ? cc.modelo_negocio_id
                : cc.modelo_negocio_flujo_id;
            // Obtiene detalle de la proforma calculada con montos, ejercicio y acuumulado
            List<ProformaDetalle> listDetProformaCalc = CalculaDetalleProforma(idCC, datTipoProf.mes_inicio,
                cc.empresa_id, idModeloNeg,
                cc.proyecto_id, anio, idTipoCaptura, idTipoProforma);

            if (listDetProformaCalc.Count == 0)
            {
                DateTime fechaProf = DateTime.Today;
                string nombreTipoCaptura = idTipoCaptura==TipoCapturaContable?"contable":"de flujo";
                throw new InvalidDataException("No existe información con fecha '" + fechaProf + "' para proforma " + nombreTipoCaptura + " de la empresa '" + cc.empresa_id + "' y modelo de negocio '" + idModeloNeg+"'");
            }

            // Enlista la proforma
            List<ProformaDetalle> lstProformaCompleta = CompletaDetalles(listDetProformaCalc, cc,idModeloNeg);
            listDetProformaCalc.ForEach(detalle =>
            {
                detalle.centro_costo_id = idCC;
                detalle.anio = anio;
                detalle.tipo_proforma_id = idTipoProforma;
                detalle.tipo_captura_id = idTipoCaptura;
                detalle.modelo_negocio_id=idModeloNeg;
            });

            return lstProformaCompleta;
        }

        public ProformaDetalle ConstruyeDetalleTotal(List<ProformaDetalle> detalles, Rubros rubroTotal,double porcentaje)
        {
            string aritmetica = rubroTotal.aritmetica;
            if (porcentaje != 1.0)
            {
                aritmetica = "(" + aritmetica + ") * "+porcentaje;
            }
            var aritmeticas = new Dictionary<string, string>();
            aritmeticas.Add("enero", aritmetica);
            aritmeticas.Add("febrero", aritmetica);
            aritmeticas.Add("marzo", aritmetica);
            aritmeticas.Add("abril", aritmetica);
            aritmeticas.Add("mayo", aritmetica);
            aritmeticas.Add("junio", aritmetica);
            aritmeticas.Add("julio", aritmetica);
            aritmeticas.Add("agosto", aritmetica);
            aritmeticas.Add("septiembre", aritmetica);
            aritmeticas.Add("octubre", aritmetica);
            aritmeticas.Add("noviembre", aritmetica);
            aritmeticas.Add("diciembre", aritmetica);
            aritmeticas.Add("ejercicio", aritmetica);
            aritmeticas.Add("acumulado", aritmetica);
            aritmeticas.Add("total", aritmetica);

            detalles.ForEach(detalle =>
            {
                Rubros rubrosCta = BuscaRubroPorId(detalle.rubro_id);
                detalle.clave_rubro=rubrosCta.clave;
                if (aritmetica.Contains(rubrosCta.clave))
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
            proformaDetalleTotal.nombre_rubro = rubroTotal.nombre;
            proformaDetalleTotal.aritmetica = aritmetica;
            proformaDetalleTotal.clave_rubro = rubroTotal.clave;

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


        private List<ProformaDetalle> CompletaDetalles(List<ProformaDetalle> detCtas, CentroCostos centroCostos,Int64 idModeloNeg)
        {
            List<Rubros> rubTots = GetRubrosTotales(idModeloNeg);
            List<ProformaDetalle> totales = new List<ProformaDetalle>();
            foreach (Rubros rubTot in rubTots)
            {
                ProformaDetalle detTot = ConstruyeDetalleTotal(detCtas, rubTot,centroCostos.porcentaje);
                totales.Add(detTot);
            }
            detCtas.AddRange(totales);
            return detCtas;
        }

        private List<Rubros> GetRubrosTotales(Int64 idModelo)
        {
            string consulta = "";
            consulta += " select rub.id, rub.nombre, rub.clave, rub.aritmetica, rub.hijos ";
            consulta += " 	from rubro rub ";
            consulta += " 	inner join tipo_rubro tip on rub.tipo_id = tip.id ";
            consulta += " 	where rub.id_modelo_neg = " + idModelo;
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

                    rubsObtenidos.id = Convert.ToInt64(rdr["id"]);
                    rubsObtenidos.nombre = Convert.ToString(rdr["nombre"]);
                    rubsObtenidos.clave = Convert.ToString(rdr["clave"]);
                    rubsObtenidos.aritmetica = Convert.ToString(rdr["aritmetica"]);
                    rubsObtenidos.hijos = Convert.ToString(rdr["hijos"]);
                    lstRubrosTot.Add(rubsObtenidos);
                }

                return lstRubrosTot;
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
            consulta += " 	where id = " + rubro_id.ToString();
            consulta += " 	and activo = 'true' ";

            try
            {
                Rubros detRubros = new Rubros();
                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    detRubros.id_modelo_neg = Convert.ToInt64(rdr["id_modelo_neg"]);
                    detRubros.tipo_id = Convert.ToInt64(rdr["tipo_id"]);
                    detRubros.clave = Convert.ToString(rdr["clave"]);
                    detRubros.aritmetica = Convert.ToString(rdr["aritmetica"]);
                    detRubros.naturaleza = Convert.ToString(rdr["naturaleza"]);
                }

                return detRubros;
            }
            finally
            {
                con.Close();
            }

        }

        public CentroCostos ObtenerDatosCC(Int64 idCenCos)
        {
            string consulta = "";
            consulta += " select empresa_id, proyecto_id, modelo_negocio_id,modelo_negocio_flujo_id,porcentaje,proyeccion ";
            consulta += " 	from centro_costo ";
            consulta += " 	where id = " + idCenCos;
            consulta += " 	and activo = 'true' and modelo_negocio_id is not null and  modelo_negocio_flujo_id is not null";

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
                    if (rdr["id"] == null)
                        datosTipoProf.id = 0;
                    else
                        datosTipoProf.id = Convert.ToInt64(rdr["id"]);

                    if (rdr["clave"] == null)
                        datosTipoProf.clave = "";
                    else
                        datosTipoProf.clave = Convert.ToString(rdr["clave"]);

                    if (rdr["mes_inicio"] == null)
                        datosTipoProf.mes_inicio = -1;
                    else
                        datosTipoProf.mes_inicio = Convert.ToInt32(rdr["mes_inicio"]);

                }

                return datosTipoProf;
            }
            finally
            {
                con.Close();
            }
        }

        public List<ProformaDetalle> CalculaDetalleProforma(Int64 idCenCos, int mesInicio, Int64 idEmpresa, Int64 idModeloNeg, Int64 idProyecto, int anio, Int64 idTipoCaptura,Int64 idTipoProforma)
        {
            ///obtener las variables
            ProformaDetalleDataAccessLayer objProfDetalle = new ProformaDetalleDataAccessLayer();
          
            // Obtiene lista de montos consolidados para ejercicio
            List<ProformaDetalle> lstGetProfDet= objProfDetalle.GetProformaCalculada(idCenCos, mesInicio, idEmpresa, idModeloNeg, idProyecto, anio, idTipoCaptura);
            // Obtiene lista de sumatorias para el acumulado
            List<ProformaDetalle> lstGetEjerc = objProfDetalle.GetAcumuladoAnteriores(idCenCos, idEmpresa, idModeloNeg, idProyecto, anio, idTipoCaptura);
            // Obtiene montos para anios posteriores
            List<ProformaDetalle> lstGetPosterior = objProfDetalle.GetEjercicioPosterior(anio, idCenCos, idModeloNeg, idTipoCaptura, idTipoProforma);

            // Genera una lista para almacenar la informacion consultada
            foreach (ProformaDetalle itemProfDet in lstGetProfDet)
            {
                foreach (ProformaDetalle itemSumProfDet in lstGetEjerc)
                {
                    // Compara elementos para completar la lista
                    if(itemProfDet.rubro_id == itemSumProfDet.rubro_id)
                    {
                        // Si coincide el rubro se guardan los acumulados anteriores
                        itemProfDet.acumulado_resultado = itemSumProfDet.acumulado_resultado;
                        // Se actualiza el total como la suma del ejercicio + el acumulado
                        itemProfDet.total_resultado = itemSumProfDet.acumulado_resultado + itemProfDet.ejercicio_resultado;
                        break;
                    }
                }
                foreach(ProformaDetalle itemProfPost in lstGetPosterior)
                {
                    if(itemProfDet.rubro_id == itemProfPost.rubro_id)
                    {
                        // Si coincide el rubro se guardan los acumulados de anios posteriores
                        itemProfDet.anios_posteriores_resultado = itemProfPost.anios_posteriores_resultado;
                    }
                }
            }

            return lstGetProfDet;
        }
        
        // Metodo para almacenar una proforma
        public int GuardaProforma(List<ProformaDetalle> detalles)
        {
            Proforma proforma=new Proforma();
            DateTime fechaProc = DateTime.Today;
            proforma.activo = true;
            proforma.anio=detalles[0].anio;
            proforma.usuario = detalles[0].usuario;
            proforma.modelo_negocio_id=detalles[0].modelo_negocio_id;
            proforma.tipo_proforma_id = detalles[0].tipo_proforma_id;
            proforma.tipo_captura_id=detalles[0].tipo_captura_id;
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

    }
}
