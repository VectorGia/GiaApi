using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;
using NpgsqlTypes;

namespace AppGia.Controllers
{
    public class PreProformaDataAccessLayer
    {
        NpgsqlConnection con;
        NpgsqlCommand comP = new NpgsqlCommand();
        private  Conexion.Conexion conex = new Conexion.Conexion();

        public PreProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public int MontosConsolidados()
        {
            GeneraQry qry = new GeneraQry();
            DataTable dt =  Cia_CC();
            
            String tipo;
            Double cambiol, cambiop;
            DateTime fechaactual = DateTime.Today;
            int cantcol = 0;
            Int64 datos;


            foreach (DataRow r in dt.Rows)
            {
                EmpresaCC EmpCCProy = new EmpresaCC();
                EmpCCProy.id = Convert.ToInt64(r["id"]);
                EmpCCProy.empresa_id = Convert.ToInt64(r["empresa_id"]);
                EmpCCProy.proyecto_id = Convert.ToInt64(r["proyecto_id"]);


                try
                {
                    DataTable dpm = Proyecto_Modelo(EmpCCProy.proyecto_id);
                    foreach (DataRow p in dpm.Rows)
                    {
                        Int64 modelo = Convert.ToInt64(p["id"]);
                        Int64 tipo_captura = Convert.ToInt64(p["tipo_captura_id"]);
                        List<Rubros> lstModel = lstModeloNeg(modelo);
                        if (tipo_captura == 1)
                        {
                            datos = Sindatos(1);
                            foreach (Rubros rubros in lstModel)
                            {
                                String consulta = qry.getQuerySums(rubros.rangos_cuentas_incluidas,
                                    rubros.rango_cuentas_excluidas, EmpCCProy.empresa_id, "balanza", "cuenta_unificada",
                                    12, datos);
                                DataTable dr = Rubros(consulta);
                                tipo = Moneda(EmpCCProy.empresa_id);
                                cambiop = CambioPesos();
                                if (tipo != "MX")
                                {
                                    cambiol = CambioLocal(EmpCCProy.empresa_id);
                                }
                                else
                                {
                                    cambiol = 1;
                                    cambiop = 1;
                                }

                                foreach (DataRow rub in dr.Rows)
                                {
                                    MontosConsolidados montos = new MontosConsolidados();
                                    montos.activo = true;
                                    montos.enero_abono_resultado = ((Convert.ToDouble(rub["eneabonos"]) * cambiol) / cambiop);
                                    montos.enero_cargo_resultado = ((Convert.ToDouble(rub["enecargos"]) * cambiol) / cambiop);
                                    montos.enero_total_resultado = ((Convert.ToDouble(rub["enetotal"]) * cambiol) / cambiop);
                                    montos.febrero_abono_resultado = ((Convert.ToDouble(rub["febabonos"]) * cambiol) / cambiop);
                                    montos.febrero_cargo_resultado = ((Convert.ToDouble(rub["febcargos"]) * cambiol) / cambiop);
                                    montos.febrero_total_resultado = ((Convert.ToDouble(rub["febtotal"]) * cambiol) / cambiop);
                                    montos.marzo_abono_resultado = ((Convert.ToDouble(rub["marabonos"]) * cambiol) / cambiop);
                                    montos.marzo_cargo_resultado = ((Convert.ToDouble(rub["marcargos"]) * cambiol) / cambiop);
                                    montos.marzo_total_resultado = ((Convert.ToDouble(rub["martotal"]) * cambiol) / cambiop);
                                    montos.abril_abono_resultado = ((Convert.ToDouble(rub["abrabonos"]) * cambiol) / cambiop);
                                    montos.abril_cargo_resultado = ((Convert.ToDouble(rub["abrcargos"]) * cambiol) / cambiop);
                                    montos.abril_total_resultado = ((Convert.ToDouble(rub["abrtotal"]) * cambiol) / cambiop);
                                    montos.mayo_abono_resultado = ((Convert.ToDouble(rub["mayabonos"]) * cambiol) / cambiop);
                                    montos.mayo_cargo_resultado = ((Convert.ToDouble(rub["maycargos"]) * cambiol) / cambiop);
                                    montos.mayo_total_resultado = ((Convert.ToDouble(rub["maytotal"]) * cambiol) / cambiop);
                                    montos.junio_abono_resultado = ((Convert.ToDouble(rub["junabonos"]) * cambiol) / cambiop);
                                    montos.junio_cargo_resultado = ((Convert.ToDouble(rub["juncargos"]) * cambiol) / cambiop);
                                    montos.junio_total_resultado = ((Convert.ToDouble(rub["juntotal"]) * cambiol) / cambiop);
                                    montos.julio_abono_resultado = ((Convert.ToDouble(rub["julabonos"]) * cambiol) / cambiop);
                                    montos.julio_cargo_resultado = ((Convert.ToDouble(rub["julcargos"]) * cambiol) / cambiop);
                                    montos.julio_total_resultado = ((Convert.ToDouble(rub["jultotal"]) * cambiol) / cambiop);
                                    montos.agosto_abono_resultado = ((Convert.ToDouble(rub["agoabonos"]) * cambiol) / cambiop);
                                    montos.agosto_cargo_resultado = ((Convert.ToDouble(rub["agocargos"]) * cambiol) / cambiop);
                                    montos.agosto_total_resultado = ((Convert.ToDouble(rub["agototal"]) * cambiol) / cambiop);
                                    montos.septiembre_abono_resultado = ((Convert.ToDouble(rub["sepabonos"]) * cambiol) / cambiop);
                                    montos.septiembre_cargo_resultado = ((Convert.ToDouble(rub["sepcargos"]) * cambiol) / cambiop);
                                    montos.septiembre_total_resultado = ((Convert.ToDouble(rub["septotal"]) * cambiol) / cambiop);
                                    montos.octubre_abono_resultado = ((Convert.ToDouble(rub["octabonos"]) * cambiol) / cambiop);
                                    montos.octubre_cargo_resultado = ((Convert.ToDouble(rub["octcargos"]) * cambiol) / cambiop);
                                    montos.octubre_total_resultado = ((Convert.ToDouble(rub["octtotal"]) * cambiol) / cambiop);
                                    montos.noviembre_abono_resultado = ((Convert.ToDouble(rub["novabonos"]) * cambiol) / cambiop);
                                    montos.noviembre_cargo_resultado = ((Convert.ToDouble(rub["novcargos"]) * cambiol) / cambiop);
                                    montos.noviembre_total_resultado = ((Convert.ToDouble(rub["novtotal"]) * cambiol) / cambiop);
                                    montos.diciembre_abono_resultado = ((Convert.ToDouble(rub["dicabonos"]) * cambiol) / cambiop);
                                    montos.diciembre_cargo_resultado = ((Convert.ToDouble(rub["diccargos"]) * cambiol) / cambiop);
                                    montos.diciembre_total_resultado = ((Convert.ToDouble(rub["dictotal"]) * cambiol) / cambiop);
                                    montos.anio = Convert.ToInt32(rub["year"]);
                                    montos.fecha = fechaactual;
                                    montos.mes = fechaactual.Month;
                                    montos.valor_tipo_cambio_resultado = cambiop;
                                    montos.centro_costo_id = EmpCCProy.id;
                                    montos.empresa_id = EmpCCProy.empresa_id;
                                    montos.modelo_negocio_id = modelo;
                                    montos.proyecto_id = EmpCCProy.proyecto_id;
                                    montos.rubro_id = rubros.id;
                                    montos.tipo_captura_id = 1;

                                    cantcol += insertarMontos(montos);
                                }
                            }
                        }
                        else
                        {
                            datos = Sindatos(2);
                            foreach (Rubros rubros in lstModel)
                            {
                                String consulta = qry.getQuerySemanalSums(rubros.rangos_cuentas_incluidas,
                                    rubros.rango_cuentas_excluidas, EmpCCProy.empresa_id, "semanal", "itm::text", 2,
                                    datos);
                                DataTable dr = Rubros(consulta);
                                tipo = Moneda(EmpCCProy.empresa_id);
                                cambiop = CambioPesos();
                                if (tipo != "MX")
                                {
                                    cambiol = CambioLocal(EmpCCProy.empresa_id);
                                }
                                else
                                {
                                    cambiol = 1;
                                    cambiop = 1;
                                }

                                Dictionary<int, MontosConsolidados> montosPorAnio =
                                    new Dictionary<int, MontosConsolidados>();
                                foreach (DataRow rubf in dr.Rows)
                                {
                                    int year = Convert.ToInt32(rubf["year"]);
                                    if (!montosPorAnio.ContainsKey(year))
                                    {
                                        montosPorAnio.Add(year, new MontosConsolidados());
                                    }

                                    MontosConsolidados montos = montosPorAnio[year];
                                    Double mes = Convert.ToDouble(rubf["mes"]);

                                    switch (mes)
                                    {
                                        case 1:
                                            montos.enero_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 2:
                                            montos.febrero_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 3:
                                            montos.marzo_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 4:
                                            montos.abril_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 5:
                                            montos.mayo_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 6:
                                            montos.junio_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 7:
                                            montos.julio_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 8:
                                            montos.agosto_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 9:
                                            montos.septiembre_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 10:
                                            montos.octubre_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 11:
                                            montos.noviembre_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                        case 12:
                                            montos.diciembre_total_resultado =
                                                ((Convert.ToDouble(rubf["saldo"]) * cambiol) / cambiop);
                                            break;
                                    }
                                }

                                foreach (KeyValuePair<int, MontosConsolidados> kv in montosPorAnio)
                                {
                                    MontosConsolidados montos = kv.Value;
                                    montos.anio = kv.Key;
                                    montos.activo = true;
                                    montos.fecha = fechaactual;
                                    montos.mes = fechaactual.Month;
                                    montos.valor_tipo_cambio_resultado = cambiop;
                                    montos.centro_costo_id = EmpCCProy.id;
                                    montos.empresa_id = EmpCCProy.empresa_id;
                                    montos.modelo_negocio_id = modelo;
                                    montos.proyecto_id = EmpCCProy.proyecto_id;
                                    montos.rubro_id = rubros.id;
                                    montos.tipo_captura_id = 2;

                                    cantcol += insertarMontos(montos);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    con.Close();
                }
            }

            return cantcol;
        }

       

        public DataTable Modelos_Negocio(Int64 modelo_id)
        {
            string consulta = "SELECT * FROM rubro WHERE tipo_id = 2 AND id_modelo_neg = " + modelo_id;


            try
            {
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public List<Rubros> lstModeloNeg(Int64 modelo_id)
        {
            List<Rubros> lstModel = new List<Rubros>();
            DataTable dt = new DataTable();
            dt = Modelos_Negocio(modelo_id);
            foreach (DataRow r in dt.Rows)
            {
                Rubros modelosnegocio = new Rubros();
                modelosnegocio.id = Convert.ToInt64(r["id"]);
                modelosnegocio.activo = Convert.ToBoolean(r["activo"]);
                modelosnegocio.nombre = r["nombre"].ToString();
                modelosnegocio.aritmetica = r["aritmetica"].ToString();
                modelosnegocio.clave = r["clave"].ToString();
                modelosnegocio.naturaleza = r["naturaleza"].ToString();
                modelosnegocio.rango_cuentas_excluidas = r["rango_cuentas_excluidas"].ToString();
                modelosnegocio.rangos_cuentas_incluidas = r["rangos_cuentas_incluidas"].ToString();
                modelosnegocio.tipo_id = Convert.ToInt64(r["tipo_id"]);
                modelosnegocio.id_modelo_neg = Convert.ToInt64(r["id_modelo_neg"]);
                modelosnegocio.hijos = r["hijos"].ToString();

                lstModel.Add(modelosnegocio);
            }

            return lstModel;
        }

        public DataTable Cia_CC()
        {
            string consulta = "SELECT * FROM centro_costo WHERE activo = true";
            //+ " WHERE " + cod + "INT_ID_EMPRESA" + cod + " = " + id_empresa;

            try
            {
                //eve.WriteEntry("Se inicio el proceso de consulta empresa centro de costo", EventLogEntryType.Information);
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                //eve.WriteEntry(ex.Message, EventLogEntryType.Error);
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable Proyecto_Modelo(Int64 proyecto_id)
        {

            string consulta = "select mn.id,tipo_captura_id from proyecto p join modelo_negocio mn on p.modelo_negocio_id = mn.id where mn.activo = true and p.id= " + proyecto_id;

            //+ " WHERE " + cod + "INT_ID_EMPRESA" + cod + " = " + id_empresa;

            try
            {
                //eve.WriteEntry("Se inicio el proceso de consulta modelo empresa", EventLogEntryType.Information);
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable Rubros(String qry)
        {
            string consulta = qry;

            try
            {
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public String Moneda(Int64 empresa_id)
        {
            string consulta = "SELECT clave FROM moneda m, empresa e" +
                              " WHERE m.id = e.moneda_id" +
                              " AND e.id = " + empresa_id;
            String tipo = "";

            try
            {
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    tipo = r["clave"].ToString();
                }

                return tipo;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public Double CambioLocal(Int64 empresa_id)
        {
            string consulta = "SELECT t.valor FROM tipo_cambio t, moneda m, empresa e" +
                              " WHERE t.moneda_id = m.id" +
                              " AND e.moneda_id = m.id" +
                              " AND t.fecha = current_date" +
                              " AND e.id = " + empresa_id;
            Double tipo = 1;

            try
            {
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    tipo = Convert.ToDouble(r["valor"]);
                }

                return tipo;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public Double CambioPesos()
        {
            string consulta = "SELECT t.valor FROM tipo_cambio t, moneda m" +
                              " WHERE t.moneda_id = m.id" +
                              " AND t.fecha = current_date" +
                              " AND m.clave = 'MX'";
            Double cambiop = 1;

            try
            {
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    cambiop = Convert.ToDouble(r["valor"]);
                }

                return cambiop;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int insertarMontos(MontosConsolidados montos)
        {
            con.Open();
            var transaction = con.BeginTransaction();

            string addBalanza = "INSERT INTO montos_consolidados(" +
                                "id, activo, " +
                                "enero_abono_resultado, enero_cargo_resultado, enero_total_resultado, " +
                                "febrero_abono_resultado, febrero_cargo_resultado, febrero_total_resultado, " +
                                "marzo_abono_resultado,marzo_cargo_resultado,marzo_total_resultado, " +
                                "abril_abono_resultado, abril_cargo_resultado, abril_total_resultado, " +
                                "mayo_abono_resultado, mayo_cargo_resultado, mayo_total_resultado, " +
                                "junio_abono_resultado, junio_cargo_resultado, junio_total_resultado, " +
                                "julio_abono_resultado, julio_cargo_resultado, julio_total_resultado, " +
                                "agosto_abono_resultado, agosto_cargo_resultado, agosto_total_resultado, " +
                                "septiembre_abono_resultado, septiembre_cargo_resultado, septiembre_total_resultado, " +
                                "octubre_abono_resultado, octubre_cargo_resultado, octubre_total_resultado, " +
                                "noviembre_abono_resultado, noviembre_cargo_resultado, noviembre_total_resultado, " +
                                "diciembre_abono_resultado, diciembre_cargo_resultado, diciembre_total_resultado, " +
                                "anio, fecha,mes, " +
                                "valor_tipo_cambio_resultado, centro_costo_id, empresa_id, modelo_negocio_id, proyecto_id, rubro_id, tipo_captura_id)"
                                + "VALUES "
                                + "(NEXTVAL('seq_montos_consol')," +
                                "@activo," +
                                "@enero_abono_resultado," +
                                "@enero_cargo_resultado, " +
                                "@enero_total_resultado, " +
                                "@febrero_abono_resultado, " +
                                "@febrero_cargo_resultado, " +
                                "@febrero_total_resultado, " +
                                "@marzo_abono_resultado," +
                                "@marzo_cargo_resultado," +
                                "@marzo_total_resultado," +
                                "@abril_abono_resultado," +
                                "@abril_cargo_resultado," +
                                "@abril_total_resultado," +
                                "@mayo_abono_resultado," +
                                "@mayo_cargo_resultado," +
                                "@mayo_total_resultado," +
                                "@junio_abono_resultado," +
                                "@junio_cargo_resultado," +
                                "@junio_total_resultado," +
                                "@julio_abono_resultado," +
                                "@julio_cargo_resultado," +
                                "@julio_total_resultado," +
                                "@agosto_abono_resultado," +
                                "@agosto_cargo_resultado," +
                                "@agosto_total_resultado," +
                                "@septiembre_abono_resultado," +
                                "@septiembre_cargo_resultado," +
                                "@septiembre_total_resultado," +
                                "@octubre_abono_resultado," +
                                "@octubre_cargo_resultado," +
                                "@octubre_total_resultado," +
                                "@noviembre_abono_resultado," +
                                "@noviembre_cargo_resultado," +
                                "@noviembre_total_resultado," +
                                "@diciembre_abono_resultado," +
                                "@diciembre_cargo_resultado," +
                                "@diciembre_total_resultado," +
                                "@anio," +
                                "@fecha," +
                                "@mes," +
                                "@valor_tipo_cambio_resultado," +
                                "@centro_costo_id," +
                                "@empresa_id," +
                                "@modelo_negocio_id," +
                                "@proyecto_id," +
                                "@rubro_id, " +
                                "@tipo_captura_id)";

            try
            {
                {
                    int cantFilaAfect = 0;
                    NpgsqlCommand cmd = new NpgsqlCommand(addBalanza, con);
                    cmd.Parameters.AddWithValue("@activo", NpgsqlDbType.Boolean, montos.activo);
                    cmd.Parameters.AddWithValue("@enero_abono_resultado", NpgsqlDbType.Double,
                        montos.enero_abono_resultado);
                    cmd.Parameters.AddWithValue("@enero_cargo_resultado", NpgsqlDbType.Double,
                        montos.enero_cargo_resultado);
                    cmd.Parameters.AddWithValue("@enero_total_resultado", NpgsqlDbType.Double,
                        montos.enero_total_resultado);
                    cmd.Parameters.AddWithValue("@febrero_abono_resultado", NpgsqlDbType.Double,
                        montos.febrero_abono_resultado);
                    cmd.Parameters.AddWithValue("@febrero_cargo_resultado", NpgsqlDbType.Double,
                        montos.febrero_cargo_resultado);
                    cmd.Parameters.AddWithValue("@febrero_total_resultado", NpgsqlDbType.Double,
                        montos.febrero_total_resultado);
                    cmd.Parameters.AddWithValue("@marzo_abono_resultado", NpgsqlDbType.Double,
                        montos.marzo_abono_resultado);
                    cmd.Parameters.AddWithValue("@marzo_cargo_resultado", NpgsqlDbType.Double,
                        montos.marzo_cargo_resultado);
                    cmd.Parameters.AddWithValue("@marzo_total_resultado", NpgsqlDbType.Double,
                        montos.marzo_total_resultado);
                    cmd.Parameters.AddWithValue("@abril_abono_resultado", NpgsqlDbType.Double,
                        montos.abril_abono_resultado);
                    cmd.Parameters.AddWithValue("@abril_cargo_resultado", NpgsqlDbType.Double,
                        montos.abril_cargo_resultado);
                    cmd.Parameters.AddWithValue("@abril_total_resultado", NpgsqlDbType.Double,
                        montos.abril_total_resultado);
                    cmd.Parameters.AddWithValue("@mayo_abono_resultado", NpgsqlDbType.Double,
                        montos.mayo_abono_resultado);
                    cmd.Parameters.AddWithValue("@mayo_cargo_resultado", NpgsqlDbType.Double,
                        montos.mayo_cargo_resultado);
                    cmd.Parameters.AddWithValue("@mayo_total_resultado", NpgsqlDbType.Double,
                        montos.mayo_total_resultado);
                    cmd.Parameters.AddWithValue("@junio_abono_resultado", NpgsqlDbType.Double,
                        montos.junio_abono_resultado);
                    cmd.Parameters.AddWithValue("@junio_cargo_resultado", NpgsqlDbType.Double,
                        montos.junio_cargo_resultado);
                    cmd.Parameters.AddWithValue("@junio_total_resultado", NpgsqlDbType.Double,
                        montos.junio_total_resultado);
                    cmd.Parameters.AddWithValue("@julio_abono_resultado", NpgsqlDbType.Double,
                        montos.julio_abono_resultado);
                    cmd.Parameters.AddWithValue("@julio_cargo_resultado", NpgsqlDbType.Double,
                        montos.julio_cargo_resultado);
                    cmd.Parameters.AddWithValue("@julio_total_resultado", NpgsqlDbType.Double,
                        montos.julio_total_resultado);
                    cmd.Parameters.AddWithValue("@agosto_abono_resultado", NpgsqlDbType.Double,
                        montos.agosto_abono_resultado);
                    cmd.Parameters.AddWithValue("@agosto_cargo_resultado", NpgsqlDbType.Double,
                        montos.agosto_cargo_resultado);
                    cmd.Parameters.AddWithValue("@agosto_total_resultado", NpgsqlDbType.Double,
                        montos.agosto_total_resultado);
                    cmd.Parameters.AddWithValue("@septiembre_abono_resultado", NpgsqlDbType.Double,
                        montos.septiembre_abono_resultado);
                    cmd.Parameters.AddWithValue("@septiembre_cargo_resultado", NpgsqlDbType.Double,
                        montos.septiembre_cargo_resultado);
                    cmd.Parameters.AddWithValue("@septiembre_total_resultado", NpgsqlDbType.Double,
                        montos.septiembre_total_resultado);
                    cmd.Parameters.AddWithValue("@octubre_abono_resultado", NpgsqlDbType.Double,
                        montos.octubre_abono_resultado);
                    cmd.Parameters.AddWithValue("@octubre_cargo_resultado", NpgsqlDbType.Double,
                        montos.octubre_cargo_resultado);
                    cmd.Parameters.AddWithValue("@octubre_total_resultado", NpgsqlDbType.Double,
                        montos.octubre_total_resultado);
                    cmd.Parameters.AddWithValue("@noviembre_abono_resultado", NpgsqlDbType.Double,
                        montos.noviembre_abono_resultado);
                    cmd.Parameters.AddWithValue("@noviembre_cargo_resultado", NpgsqlDbType.Double,
                        montos.noviembre_cargo_resultado);
                    cmd.Parameters.AddWithValue("@noviembre_total_resultado", NpgsqlDbType.Double,
                        montos.noviembre_total_resultado);
                    cmd.Parameters.AddWithValue("@diciembre_abono_resultado", NpgsqlDbType.Double,
                        montos.diciembre_abono_resultado);
                    cmd.Parameters.AddWithValue("@diciembre_cargo_resultado", NpgsqlDbType.Double,
                        montos.diciembre_cargo_resultado);
                    cmd.Parameters.AddWithValue("@diciembre_total_resultado", NpgsqlDbType.Double,
                        montos.diciembre_total_resultado);
                    cmd.Parameters.AddWithValue("@anio", NpgsqlDbType.Integer, montos.anio);
                    cmd.Parameters.AddWithValue("@fecha", NpgsqlDbType.Date, montos.fecha);
                    cmd.Parameters.AddWithValue("@mes", NpgsqlDbType.Integer, montos.mes);
                    cmd.Parameters.AddWithValue("@valor_tipo_cambio_resultado", NpgsqlDbType.Double,
                        montos.valor_tipo_cambio_resultado);
                    cmd.Parameters.AddWithValue("@centro_costo_id", NpgsqlDbType.Bigint,
                        montos.centro_costo_id);
                    cmd.Parameters.AddWithValue("@empresa_id", NpgsqlDbType.Bigint, montos.empresa_id);
                    cmd.Parameters.AddWithValue("@modelo_negocio_id", NpgsqlDbType.Bigint,
                        montos.modelo_negocio_id);
                    cmd.Parameters.AddWithValue("@proyecto_id", NpgsqlDbType.Bigint, montos.proyecto_id);
                    cmd.Parameters.AddWithValue("@rubro_id", NpgsqlDbType.Bigint, montos.rubro_id);
                    cmd.Parameters.AddWithValue("@tipo_captura_id", NpgsqlDbType.Bigint,
                        montos.tipo_captura_id);

                    cantFilaAfect = cantFilaAfect + Convert.ToInt32(cmd.ExecuteNonQuery());
                    transaction.Commit();
                    con.Close();

                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public Int64 Sindatos(Int64 captura)
        {
            string consulta = "SELECT count(*) FROM montos_consolidados WHERE tipo_captura_id = " + captura;
            Int64 contador = 0;

            try
            {
                con.Open();
                comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                foreach (DataRow r in dt.Rows)
                {
                    contador = Convert.ToInt64(r["count"]);
                }

                return contador;
            }
            catch (Exception ex)
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