using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AppGia.Controllers;
using AppGia.Helpers;
using AppGia.Models;
using NLog;
using Npgsql;
using static System.Convert;

namespace AppGia.Dao
{
    public class ProformaDetalleDataAccessLayer
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        private ProformaHelper _profHelper = new ProformaHelper();
        private ProformaDataAccessLayer _proformaDataAccessLayer = new ProformaDataAccessLayer();
        private QueryExecuter _queryExecuter = new QueryExecuter();

        public ProformaDetalleDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public List<ProformaDetalle> GetProformaDetalle(Int64 idProforma)
        {
            string consulta = "";
            consulta += " select ";
            consulta += "   det.id, det.id_proforma, det.rubro_id, rub.nombre as nombre_rubro,rub.hijos,rub.aritmetica, rub.clave,rub.es_total_ingresos,";
            consulta += "   coalesce(ejercicio_resultado, 0) as ejercicio_resultado, ";
            consulta += "   coalesce(enero_monto_resultado, 0) as enero_monto_resultado, ";
            consulta += "   coalesce(febrero_monto_resultado, 0) as febrero_monto_resultado, ";
            consulta += "   coalesce(marzo_monto_resultado, 0) as marzo_monto_resultado, ";
            consulta += "   coalesce(abril_monto_resultado, 0) as abril_monto_resultado, ";
            consulta += "   coalesce(mayo_monto_resultado, 0) as mayo_monto_resultado, ";
            consulta += "   coalesce(junio_monto_resultado, 0) as junio_monto_resultado, ";
            consulta += "   coalesce(julio_monto_resultado, 0) as julio_monto_resultado, ";
            consulta += "   coalesce(agosto_monto_resultado, 0) as agosto_monto_resultado, ";
            consulta += "   coalesce(septiembre_monto_resultado, 0) as septiembre_monto_resultado, ";
            consulta += "   coalesce(octubre_monto_resultado, 0) as octubre_monto_resultado, ";
            consulta += "   coalesce(noviembre_monto_resultado, 0) as noviembre_monto_resultado, ";
            consulta += "   coalesce(diciembre_monto_resultado, 0) as diciembre_monto_resultado, ";
            consulta += "   coalesce(total_resultado, 0) as total_resultado, ";
            consulta += "   coalesce(acumulado_resultado, 0) as acumulado_resultado, ";
            consulta += "   coalesce(valor_tipo_cambio_resultado, 0) as valor_tipo_cambio_resultado ";
            consulta += " from proforma_detalle det ";
            consulta += " inner join rubro rub on det.rubro_id = rub.id ";
            consulta += " where id_proforma = " + idProforma;
            consulta += " and det.activo = 'true' ";


            List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();

            DataTable dataTable = _queryExecuter.ExecuteQuery(consulta.Trim());
            foreach (DataRow rdr in dataTable.Rows)
            {
                ProformaDetalle proforma_detalle = new ProformaDetalle();
                proforma_detalle.id = ToInt64(rdr["id"]);
                proforma_detalle.id_proforma = ToInt64(rdr["id_proforma"]);
                proforma_detalle.rubro_id = ToInt64(rdr["rubro_id"]);
                proforma_detalle.nombre_rubro = Convert.ToString(rdr["nombre_rubro"]);
                proforma_detalle.aritmetica = Convert.ToString(rdr["aritmetica"]);
                proforma_detalle.es_total_ingresos = ToBoolean(rdr["es_total_ingresos"]);
                proforma_detalle.ejercicio_resultado = ToDouble(rdr["ejercicio_resultado"]);
                proforma_detalle.enero_monto_resultado = ToDouble(rdr["enero_monto_resultado"]);
                proforma_detalle.febrero_monto_resultado = ToDouble(rdr["febrero_monto_resultado"]);
                proforma_detalle.marzo_monto_resultado = ToDouble(rdr["marzo_monto_resultado"]);
                proforma_detalle.abril_monto_resultado = ToDouble(rdr["abril_monto_resultado"]);
                proforma_detalle.mayo_monto_resultado = ToDouble(rdr["mayo_monto_resultado"]);
                proforma_detalle.junio_monto_resultado = ToDouble(rdr["junio_monto_resultado"]);
                proforma_detalle.julio_monto_resultado = ToDouble(rdr["julio_monto_resultado"]);
                proforma_detalle.agosto_monto_resultado = ToDouble(rdr["agosto_monto_resultado"]);
                proforma_detalle.septiembre_monto_resultado = ToDouble(rdr["septiembre_monto_resultado"]);
                proforma_detalle.octubre_monto_resultado = ToDouble(rdr["octubre_monto_resultado"]);
                proforma_detalle.noviembre_monto_resultado = ToDouble(rdr["noviembre_monto_resultado"]);
                proforma_detalle.diciembre_monto_resultado = ToDouble(rdr["diciembre_monto_resultado"]);
                proforma_detalle.total_resultado = ToDouble(rdr["total_resultado"]);
                proforma_detalle.acumulado_resultado = ToDouble(rdr["acumulado_resultado"]);
                proforma_detalle.valor_tipo_cambio_resultado = ToDouble(rdr["valor_tipo_cambio_resultado"]);
                //proforma_detalle.activo = ToBoolean(rdr["activo"]);
                proforma_detalle.hijos = rdr["hijos"].ToString();
                proforma_detalle.clave_rubro = rdr["clave"].ToString();

                lstProformaDetalle.Add(proforma_detalle);
            }

            Proforma pro = _proformaDataAccessLayer.GetProforma(idProforma);
            Boolean hayPeriodoActivo =
                _profHelper.existePeridodoActivo(pro.anio, pro.tipo_proforma_id, pro.tipo_captura_id);

            Int64 idEmpresa =
                ToInt64(_queryExecuter.ExecuteQueryUniqueresult(
                        "select empresa_id from centro_costo where id =" + pro.centro_costo_id)["empresa_id"]);

            List<ProformaDetalle> detallesAniosPosteriores = GetEjercicioPosterior(pro.anio, pro.centro_costo_id,
                pro.modelo_negocio_id, pro.tipo_captura_id, pro.tipo_proforma_id);
            
            lstProformaDetalle.ForEach(detalle =>
            {
                detalle.editable = hayPeriodoActivo;
                detalle.mes_inicio = _profHelper.getMesInicio(pro.tipo_proforma_id);
                detalle.modelo_negocio_id = pro.modelo_negocio_id;
                detalle.anio = pro.anio;
                detalle.centro_costo_id = pro.centro_costo_id;
                detalle.tipo_proforma_id = pro.tipo_proforma_id;
                detalle.tipo_captura_id = pro.tipo_captura_id;
                detalle.empresa_id = idEmpresa;
                detallesAniosPosteriores.ForEach(posterior =>
                {
                    if (detalle.rubro_id == posterior.rubro_id)
                    {
                        detalle.anios_posteriores_resultado = posterior.anios_posteriores_resultado;
                    }
                });
            });

            return _profHelper.reorderConceptos(lstProformaDetalle);
        }

        public int AddProformaDetalle(ProformaDetalle proforma_detalle)
        {
            string consulta = "";
            consulta += " insert into proforma_detalle ( ";
            consulta += "	 id, id_proforma, rubro_id, activo, ejercicio_resultado, ";
            consulta += "	 enero_monto_resultado, febrero_monto_resultado, ";
            consulta += "	 marzo_monto_resultado, abril_monto_resultado, ";
            consulta += "	 mayo_monto_resultado, junio_monto_resultado, ";
            consulta += "	 julio_monto_resultado, agosto_monto_resultado, ";
            consulta += "	 septiembre_monto_resultado, octubre_monto_resultado, ";
            consulta += "	 noviembre_monto_resultado, diciembre_monto_resultado, ";
            consulta += "	 total_resultado, acumulado_resultado, ";
            consulta += "	 valor_tipo_cambio_resultado,total_real_resultado,total_proformado_resultado ";
            consulta += " ) values ( ";
            consulta += "	 nextval('seq_proforma_detalle'), @id_proforma, @rubro_id, @activo, @ejercicio_resultado, ";
            consulta += "	  @enero_monto_resultado, @febrero_monto_resultado, ";
            consulta += "	  @marzo_monto_resultado, @abril_monto_resultado, ";
            consulta += "	  @mayo_monto_resultado, @junio_monto_resultado, ";
            consulta += "	  @julio_monto_resultado, @agosto_monto_resultado, ";
            consulta += "	  @septiembre_monto_resultado, @octubre_monto_resultado, ";
            consulta += "	  @noviembre_monto_resultado, @diciembre_monto_resultado, ";
            consulta += "	  @total_resultado, @acumulado_resultado, ";
            consulta += "	  @valor_tipo_cambio_resultado,@total_real_resultado,@total_proformado_resultado ";
            consulta += " ) ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                cmd.Parameters.AddWithValue("@id_proforma", proforma_detalle.id_proforma);
                cmd.Parameters.AddWithValue("@rubro_id", proforma_detalle.rubro_id);
                cmd.Parameters.AddWithValue("@activo", proforma_detalle.activo);
                cmd.Parameters.AddWithValue("@ejercicio_resultado", proforma_detalle.ejercicio_resultado);
                cmd.Parameters.AddWithValue("@enero_monto_resultado", proforma_detalle.enero_monto_resultado);
                cmd.Parameters.AddWithValue("@febrero_monto_resultado", proforma_detalle.febrero_monto_resultado);
                cmd.Parameters.AddWithValue("@marzo_monto_resultado", proforma_detalle.marzo_monto_resultado);
                cmd.Parameters.AddWithValue("@abril_monto_resultado", proforma_detalle.abril_monto_resultado);
                cmd.Parameters.AddWithValue("@mayo_monto_resultado", proforma_detalle.mayo_monto_resultado);
                cmd.Parameters.AddWithValue("@junio_monto_resultado", proforma_detalle.junio_monto_resultado);
                cmd.Parameters.AddWithValue("@julio_monto_resultado", proforma_detalle.julio_monto_resultado);
                cmd.Parameters.AddWithValue("@agosto_monto_resultado", proforma_detalle.agosto_monto_resultado);
                cmd.Parameters.AddWithValue("@septiembre_monto_resultado", proforma_detalle.septiembre_monto_resultado);
                cmd.Parameters.AddWithValue("@octubre_monto_resultado", proforma_detalle.octubre_monto_resultado);
                cmd.Parameters.AddWithValue("@noviembre_monto_resultado", proforma_detalle.noviembre_monto_resultado);
                cmd.Parameters.AddWithValue("@diciembre_monto_resultado", proforma_detalle.diciembre_monto_resultado);
                cmd.Parameters.AddWithValue("@total_resultado", proforma_detalle.total_resultado);
                cmd.Parameters.AddWithValue("@acumulado_resultado", proforma_detalle.acumulado_resultado);
                cmd.Parameters.AddWithValue("@valor_tipo_cambio_resultado", proforma_detalle.valor_tipo_cambio_resultado);
                cmd.Parameters.AddWithValue("@total_real_resultado", proforma_detalle.total_real_resultado);
                cmd.Parameters.AddWithValue("@total_proformado_resultado", proforma_detalle.total_proformado_resultado);

                int regInsert = cmd.ExecuteNonQuery();

                return regInsert;
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
            finally
            {
                con.Close();
            }
        }

        public int UpdateProformaDetalle(ProformaDetalle proformaDetalle)
        {
            string consulta = "";
            consulta += " update proforma_detalle set activo = @activo, ";
            consulta += "    enero_monto_resultado = @enero_monto_resultado, ";
            consulta += "    febrero_monto_resultado = @febrero_monto_resultado, ";
            consulta += "    marzo_monto_resultado = @marzo_monto_resultado, ";
            consulta += "    abril_monto_resultado = @abril_monto_resultado, ";
            consulta += "    mayo_monto_resultado = @mayo_monto_resultado, ";
            consulta += "    junio_monto_resultado = @junio_monto_resultado, ";
            consulta += "    julio_monto_resultado = @julio_monto_resultado, ";
            consulta += "    agosto_monto_resultado = @agosto_monto_resultado, ";
            consulta += "    septiembre_monto_resultado = @septiembre_monto_resultado, ";
            consulta += "    octubre_monto_resultado = @octubre_monto_resultado, ";
            consulta += "    noviembre_monto_resultado = @noviembre_monto_resultado, ";
            consulta += "    diciembre_monto_resultado = @diciembre_monto_resultado, ";
            consulta += "    acumulado_resultado = @acumulado_resultado, ";
            consulta += "    ejercicio_resultado = @ejercicio_resultado, ";
            consulta += "    total_real_resultado = @total_real_resultado, ";
            consulta += "    total_proformado_resultado = @total_proformado_resultado, ";
            consulta += "    total_resultado = @total_resultado ";
            consulta += " where id = @id ";

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                    cmd.Parameters.AddWithValue("@id", proformaDetalle.id);
                    cmd.Parameters.AddWithValue("@enero_monto_resultado", proformaDetalle.enero_monto_resultado);
                    cmd.Parameters.AddWithValue("@febrero_monto_resultado", proformaDetalle.febrero_monto_resultado);
                    cmd.Parameters.AddWithValue("@marzo_monto_resultado", proformaDetalle.marzo_monto_resultado);
                    cmd.Parameters.AddWithValue("@abril_monto_resultado", proformaDetalle.abril_monto_resultado);
                    cmd.Parameters.AddWithValue("@mayo_monto_resultado", proformaDetalle.mayo_monto_resultado);
                    cmd.Parameters.AddWithValue("@junio_monto_resultado", proformaDetalle.junio_monto_resultado);
                    cmd.Parameters.AddWithValue("@julio_monto_resultado", proformaDetalle.julio_monto_resultado);
                    cmd.Parameters.AddWithValue("@agosto_monto_resultado", proformaDetalle.agosto_monto_resultado);
                    cmd.Parameters.AddWithValue("@septiembre_monto_resultado",
                        proformaDetalle.septiembre_monto_resultado);
                    cmd.Parameters.AddWithValue("@octubre_monto_resultado", proformaDetalle.octubre_monto_resultado);
                    cmd.Parameters.AddWithValue("@noviembre_monto_resultado",
                        proformaDetalle.noviembre_monto_resultado);
                    cmd.Parameters.AddWithValue("@diciembre_monto_resultado",
                        proformaDetalle.diciembre_monto_resultado);
                    cmd.Parameters.AddWithValue("@acumulado_resultado", proformaDetalle.acumulado_resultado);
                    cmd.Parameters.AddWithValue("@ejercicio_resultado", proformaDetalle.ejercicio_resultado);
                    cmd.Parameters.AddWithValue("@total_resultado", proformaDetalle.total_resultado);
                    cmd.Parameters.AddWithValue("@total_real_resultado", proformaDetalle.total_real_resultado);
                    cmd.Parameters.AddWithValue("@total_proformado_resultado", proformaDetalle.total_proformado_resultado);
                    cmd.Parameters.AddWithValue("@activo", proformaDetalle.activo);

                    int regActual = cmd.ExecuteNonQuery();
                    return regActual;
                }
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
        public List<ProformaDetalle> GetProformaCalculada(Int64 idCenCos, int mesInicio, Int64 idEmpresa,
            Int64 idModeloNegocio, Int64 idProyecto, int anio, Int64 idTipoCaptura)
        {
            DateTime fechaCarga = _profHelper.getLastFechaMontosConsol(anio, idEmpresa, idModeloNegocio, idProyecto,
                idCenCos, idTipoCaptura);
            string consulta = "";
            consulta += " select ";
            consulta += "	 mon.id, anio, mes, empresa_id, modelo_negocio_id, ";
            consulta += "	 mon.centro_costo_id, mon.activo, ";
            consulta += "	 proyecto_id, rub.id as rubro_id, rub.nombre as nombre_rubro, rub.hijos as hijos, rub.es_total_ingresos ,";

            consulta += BuildMontosFieldsQuery(mesInicio);
            consulta += BuildEjercicioFieldQuery(mesInicio);

            consulta += "	 coalesce(valor_tipo_cambio_resultado, 0) as valor_tipo_cambio_resultado ";
            consulta += "	 from montos_consolidados mon ";
            consulta += "	 inner join rubro rub on mon.rubro_id = rub.id ";
            consulta += "	 where date_trunc('DAY', fecha) = date_trunc('DAY', '" + fechaCarga.ToString("yyyy-MM-dd") + "'::date) ";
            consulta += "	 and anio = " + anio;
            consulta += "	 and empresa_id = " + idEmpresa;
            consulta += "	 and modelo_negocio_id = " + idModeloNegocio;
            consulta += "	 and proyecto_id = " + idProyecto;
            consulta += "	 and centro_costo_id = " + idCenCos;
            consulta += "	 and tipo_captura_id = " + idTipoCaptura;
            consulta += "	 and mon.activo = 'true' ";
            consulta += "	 order by rub.id ";
            
            log.Info("ejecutando query:'{0}'",consulta);


            DataTable dataTable = _queryExecuter.ExecuteQuery(consulta.Trim());
            List<ProformaDetalle> lstProformaDetalle = new List<ProformaDetalle>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                ProformaDetalle proforma_detalle = new ProformaDetalle();
                proforma_detalle.mes_inicio = mesInicio;
                proforma_detalle.id_proforma = ToInt64(rdr["id"]);
                proforma_detalle.anio = ToInt32(rdr["anio"]);
                proforma_detalle.modelo_negocio_id = ToInt64(rdr["modelo_negocio_id"]);
                proforma_detalle.empresa_id = ToInt64(rdr["empresa_id"]);
                proforma_detalle.centro_costo_id = ToInt64(rdr["centro_costo_id"]);
                proforma_detalle.activo = ToBoolean(rdr["activo"]);
                proforma_detalle.rubro_id = ToInt64(rdr["rubro_id"]);
                proforma_detalle.nombre_rubro = (rdr["nombre_rubro"]).ToString().Trim();
                proforma_detalle.hijos = (rdr["hijos"]).ToString().Trim();
                
                foreach (var entry in ProformaHelper.getPonderacionMeses())
                {
                    string nombreCampo = entry.Value;
                    proforma_detalle[nombreCampo] = ToDouble(rdr[nombreCampo]);
                }
                
                proforma_detalle.ejercicio_resultado = ToDouble(rdr["ejercicio_resultado"]);

                lstProformaDetalle.Add(proforma_detalle);
            }

            return lstProformaDetalle;
        }

        private string BuildMontosFieldsQuery(int mesInicio)
        {
            Dictionary<string, string> meses = ProformaHelper.getPonderacionMeses();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var entry in meses)
            {
                int mesValor = ToInt16(entry.Key);
                string montoAtributo = entry.Value;
                if (mesValor <= mesInicio)
                {
                    string attrTotal = montoAtributo.Replace("monto", "total");
                    stringBuilder.Append("\n	 coalesce(").Append(attrTotal).Append(", 0) ");
                }
                else
                {
                    stringBuilder.Append("\n	 0 ");
                }

                stringBuilder.Append("as ").Append(montoAtributo).Append(", ");
            }

            return stringBuilder.Append("\n").ToString();
        }
        private string BuildEjercicioFieldQuery(int mesInicio)
        {
            if (mesInicio == 0)
            {
                return "	 0 as ejercicio_resultado, ";
            }
            
            Dictionary<string, string> meses = ProformaHelper.getPonderacionMeses();
            StringBuilder stringBuilder = new StringBuilder();
         
            foreach (var entry in meses)
            {
                int mesValor = ToInt16(entry.Key);
                string montoAtributo = entry.Value;
                
                if (mesValor <= mesInicio)
                {
                    string attrTotal = montoAtributo.Replace("monto", "total");
                    stringBuilder.Append(attrTotal).Append(" + ");
                }
            }
            

            return stringBuilder.Insert(0,"coalesce(").Append(", 0) as ejercicio_resultado, \n").ToString().Replace(" + ,"," ,");
        }
        

        // Calculo del ejercicio anterior
        public List<ProformaDetalle> GetAcumuladoAnteriores(Int64 idCenCos, Int64 idEmpresa, Int64 idModeloNegocio,
            Int64 idProyecto, int anio, Int64 idTipoCaptura)
        {
            string consulta = "";
            consulta += " select  ";
            consulta += "	 coalesce ( ";
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
            consulta += "	 , 0) as acumulado_resultado, cns.rubro_id as rubro_id, rub.nombre as nombre_rubro, rub.es_total_ingresos ";
            consulta += "	 from montos_consolidados cns ";
            consulta += "	 inner join rubro rub on cns.rubro_id = rub.id ";
            consulta += "	 where cns.id in ( ";
            consulta += "			 select max(id) as idMontoLast from montos_consolidados mon ";
            consulta += "				 where mon.anio < " + anio; // Anio a proformar
            consulta += "				 and mon.empresa_id = " + idEmpresa; // Empresa
            consulta += "				 and mon.modelo_negocio_id = " + idModeloNegocio; // Modelo de Negocio
            consulta += "				 and mon.proyecto_id = " + idProyecto; // Proyecto
            consulta += "				 and mon.centro_costo_id = " + idCenCos; // Centro de costos
            consulta += "				 and mon.tipo_captura_id = " + idTipoCaptura; // Tipo de captura
            consulta += "				 and mon.activo = 'true' ";
            consulta += "		 ) ";
            consulta += "    AND  cns.anio < " + anio;
            consulta += "    AND  cns.empresa_id=" + idEmpresa;
            consulta += "    AND  cns.modelo_negocio_id=" + idModeloNegocio;
            consulta += "    AND  cns.proyecto_id=" + idProyecto;
            consulta += "    AND  cns.centro_costo_id=" + idCenCos;
            consulta += "    AND  cns.tipo_captura_id=" + idTipoCaptura;
            consulta += "    AND  cns.activo=true";
            consulta += "	 group by cns.rubro_id, rub.nombre, rub.es_total_ingresos ";
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
                    proforma_detalle_ej_financ.acumulado_resultado = ToDouble(rdr["acumulado_resultado"]);
                    proforma_detalle_ej_financ.rubro_id = ToInt64(rdr["rubro_id"]);
                    proforma_detalle_ej_financ.nombre_rubro = rdr["nombre_rubro"].ToString();
                    proforma_detalle_ej_financ.es_total_ingresos = ToBoolean(rdr["es_total_ingresos"]);
                    lstProfDetalleEjercicioFinanc.Add(proforma_detalle_ej_financ);
                }

                return lstProfDetalleEjercicioFinanc;
            }
            finally
            {
                con.Close();
            }
        }

        //Calculo de años posteriores
        public List<ProformaDetalle> GetEjercicioPosterior(int anio, Int64 idCenCos, Int64 idModNeg, Int64 idTipoCaptura, Int64 idTipoProforma)
        {
            string consulta = "";
            consulta +=
                " select prf.centro_costo_id, prf.modelo_negocio_id, prf.tipo_captura_id, prf.tipo_proforma_id, prf.anio, ";
            consulta += " 	coalesce (";
            consulta += " 	sum(det.enero_monto_resultado) + ";
            consulta += " 	sum(det.febrero_monto_resultado) + ";
            consulta += " 	sum(det.marzo_monto_resultado) + ";
            consulta += " 	sum(det.abril_monto_resultado) + ";
            consulta += " 	sum(det.mayo_monto_resultado) + ";
            consulta += " 	sum(det.junio_monto_resultado) + ";
            consulta += " 	sum(det.julio_monto_resultado) + ";
            consulta += " 	sum(det.agosto_monto_resultado) + ";
            consulta += " 	sum(det.septiembre_monto_resultado) + ";
            consulta += " 	sum(det.octubre_monto_resultado) + ";
            consulta += " 	sum(det.noviembre_monto_resultado) + ";
            consulta += " 	sum(det.diciembre_monto_resultado) ";
            consulta += " 	, 0) as anios_posteriores_resultado, det.rubro_id as rubro_id ";
            consulta += " 	from proforma_detalle det ";
            consulta += " 	inner join proforma prf on det.id_proforma = prf.id ";
            consulta += " 	where prf.anio > " + anio; // Anios posteriores a la proforma actual
            consulta += " 	and prf.centro_costo_id = " + idCenCos;
            consulta += " 	and prf.modelo_negocio_id = " + idModNeg;
            consulta += " 	and prf.tipo_captura_id = " + idTipoCaptura;
           // consulta += " 	and prf.tipo_proforma_id = " + idTipoProforma;
            consulta += " 	and prf.activo = 'true' ";
            consulta +=
                " 	group by prf.centro_costo_id, prf.modelo_negocio_id, prf.tipo_captura_id, prf.tipo_proforma_id, prf.anio, det.rubro_id ";


            List<ProformaDetalle> lstProfDetalleAniosPost = new List<ProformaDetalle>();
            DataTable dataTable = _queryExecuter.ExecuteQuery(consulta.Trim());
            foreach (DataRow rdr in dataTable.Rows)
            {
                ProformaDetalle proforma_detalle_anios_post = new ProformaDetalle();
                proforma_detalle_anios_post.centro_costo_id = ToInt64(rdr["centro_costo_id"]);
                proforma_detalle_anios_post.modelo_negocio_id = ToInt64(rdr["modelo_negocio_id"]);
                proforma_detalle_anios_post.tipo_captura_id = ToInt64(rdr["tipo_captura_id"]);
                proforma_detalle_anios_post.tipo_proforma_id = ToInt64(rdr["tipo_proforma_id"]);
                proforma_detalle_anios_post.anio = ToInt32(rdr["anio"]);
                proforma_detalle_anios_post.anios_posteriores_resultado = ToDouble(rdr["anios_posteriores_resultado"]);
                proforma_detalle_anios_post.rubro_id = ToInt64(rdr["rubro_id"]);
                lstProfDetalleAniosPost.Add(proforma_detalle_anios_post);
            }


            return lstProfDetalleAniosPost;
        }
    }
}