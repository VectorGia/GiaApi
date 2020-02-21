using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;
using NpgsqlTypes;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class PreProformaDataAccessLayer
    {
        private NpgsqlConnection con;
      
        private Conexion.Conexion conex = new Conexion.Conexion();
        private QueryExecuter _queryExecuter= new QueryExecuter();

        public PreProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }


        public int MontosConsolidados()
        {
            DataTable centrosCostoDt = GetCentrosCostro();
            DateTime fechaactual = DateTime.Today;
            int numInserts = 0;


            foreach (DataRow centrosCostoRow in centrosCostoDt.Rows)
            {
                EmpresaCC EmpProy = new EmpresaCC();
                EmpProy.id = Convert.ToInt64(centrosCostoRow["id"]);
                EmpProy.empresa_id = Convert.ToInt64(centrosCostoRow["empresa_id"]);
                EmpProy.proyecto_id = Convert.ToInt64(centrosCostoRow["proyecto_id"]);
                Int64 modeloId = Convert.ToInt64(centrosCostoRow["modelo_id"]);

                try
                {
                    DataTable modeloNegTable = _queryExecuter.ExecuteQuery("select * from modelo_negocio where id=" + modeloId);
                    //DataTable modeloNegDataTable = Proyecto_Modelo(EmpProy.proyecto_id);
                    foreach (DataRow modeloNegRow in modeloNegTable.Rows)
                    {
                        //Int64 modeloId = Convert.ToInt64(modeloNegRow["id"]);
                        Int64 tipoCaptura = Convert.ToInt64(modeloNegRow["tipo_captura_id"]);
                        List<Rubros> rubrosDeModelo = GetRubrosFromModeloId(modeloId);
                        if (tipoCaptura == TipoCapturaContable)
                        {
                            Int64 numRegistrosExistentes = getNumMontosOfTipoCaptura(TipoCapturaContable);
                            GeneraQry qry = new GeneraQry("balanza", "cuenta_unificada", 12);
                            foreach (Rubros rubro in rubrosDeModelo)
                            {
                                String consulta = qry.getQuerySums(rubro.rangos_cuentas_incluidas,
                                    rubro.rango_cuentas_excluidas, EmpProy.empresa_id, numRegistrosExistentes);
                                DataTable sumaMontosDt = _queryExecuter.ExecuteQuery(consulta);

                                foreach (DataRow rubroMontosRow in sumaMontosDt.Rows)
                                {
                                    numInserts += BuildMontosConsolContable(rubroMontosRow, EmpProy, modeloId, rubro.id,
                                        fechaactual);
                                }
                            }
                        }
                        else
                        {
                            Int64 numRegistrosExistentes = getNumMontosOfTipoCaptura(TipoCapturaFlujo);
                            foreach (Rubros rubro in rubrosDeModelo)
                            {
                                GeneraQry qry = new GeneraQry("semanal", "itm::text", 2);
                                String consulta = qry.getQuerySemanalSums(rubro.rangos_cuentas_incluidas,
                                    rubro.rango_cuentas_excluidas, EmpProy.empresa_id, numRegistrosExistentes);
                                DataTable sumaMontos = _queryExecuter.ExecuteQuery(consulta);
                                numInserts += BuildMontosFujo(sumaMontos, EmpProy, modeloId, rubro.id, fechaactual);
                            }
                        }
                    }
                }
                finally
                {
                    con.Close();
                }
            }

            return numInserts;
        }


        public DataTable findRubrosByIdModelo(Int64 modelo_id)
        {
            return _queryExecuter.ExecuteQuery("SELECT * FROM rubro WHERE tipo_id = 2 AND id_modelo_neg = " + modelo_id);
        }

        public List<Rubros> GetRubrosFromModeloId(Int64 modeloId)
        {
            List<Rubros> listaRubros = new List<Rubros>();
            DataTable rubrosDt = findRubrosByIdModelo(modeloId);
            foreach (DataRow rubrosRow in rubrosDt.Rows)
            {
                Rubros rubro = new Rubros();
                rubro.id = Convert.ToInt64(rubrosRow["id"]);
                rubro.activo = Convert.ToBoolean(rubrosRow["activo"]);
                rubro.nombre = rubrosRow["nombre"].ToString();
                rubro.aritmetica = rubrosRow["aritmetica"].ToString();
                rubro.clave = rubrosRow["clave"].ToString();
                rubro.naturaleza = rubrosRow["naturaleza"].ToString();
                rubro.rango_cuentas_excluidas = rubrosRow["rango_cuentas_excluidas"].ToString();
                rubro.rangos_cuentas_incluidas = rubrosRow["rangos_cuentas_incluidas"].ToString();
                rubro.tipo_id = Convert.ToInt64(rubrosRow["tipo_id"]);
                rubro.id_modelo_neg = Convert.ToInt64(rubrosRow["id_modelo_neg"]);
                rubro.hijos = rubrosRow["hijos"].ToString();
                listaRubros.Add(rubro);
            }

            return listaRubros;
        }

        public DataTable GetCentrosCostro()
        {
            return _queryExecuter.ExecuteQuery("SELECT * FROM centro_costo WHERE activo = true");
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

        public Int64 getNumMontosOfTipoCaptura(Int64 captura)
        {
            string consulta = "SELECT count(1) as numregs FROM montos_consolidados WHERE tipo_captura_id = " + captura;
            return Convert.ToInt64(_queryExecuter.ExecuteQuery(consulta).Rows[0]["numregs"]);
        }

        private int BuildMontosConsolContable(DataRow rubroMontosRow, EmpresaCC EmpProy, Int64 modeloId, Int64 rubroId,
            DateTime fechaactual)
        {
            MontosConsolidados montos = new MontosConsolidados();
            montos.activo = true;
            montos.enero_abono_resultado = Convert.ToDouble(rubroMontosRow["eneabonos"]);
            montos.enero_cargo_resultado = Convert.ToDouble(rubroMontosRow["enecargos"]);
            montos.enero_total_resultado = Convert.ToDouble(rubroMontosRow["enetotal"]);
            montos.febrero_abono_resultado = Convert.ToDouble(rubroMontosRow["febabonos"]);
            montos.febrero_cargo_resultado = Convert.ToDouble(rubroMontosRow["febcargos"]);
            montos.febrero_total_resultado = Convert.ToDouble(rubroMontosRow["febtotal"]);
            montos.marzo_abono_resultado = Convert.ToDouble(rubroMontosRow["marabonos"]);
            montos.marzo_cargo_resultado = Convert.ToDouble(rubroMontosRow["marcargos"]);
            montos.marzo_total_resultado = Convert.ToDouble(rubroMontosRow["martotal"]);
            montos.abril_abono_resultado = Convert.ToDouble(rubroMontosRow["abrabonos"]);
            montos.abril_cargo_resultado = Convert.ToDouble(rubroMontosRow["abrcargos"]);
            montos.abril_total_resultado = Convert.ToDouble(rubroMontosRow["abrtotal"]);
            montos.mayo_abono_resultado = Convert.ToDouble(rubroMontosRow["mayabonos"]);
            montos.mayo_cargo_resultado = Convert.ToDouble(rubroMontosRow["maycargos"]);
            montos.mayo_total_resultado = Convert.ToDouble(rubroMontosRow["maytotal"]);
            montos.junio_abono_resultado = Convert.ToDouble(rubroMontosRow["junabonos"]);
            montos.junio_cargo_resultado = Convert.ToDouble(rubroMontosRow["juncargos"]);
            montos.junio_total_resultado = Convert.ToDouble(rubroMontosRow["juntotal"]);
            montos.julio_abono_resultado = Convert.ToDouble(rubroMontosRow["julabonos"]);
            montos.julio_cargo_resultado = Convert.ToDouble(rubroMontosRow["julcargos"]);
            montos.julio_total_resultado = Convert.ToDouble(rubroMontosRow["jultotal"]);
            montos.agosto_abono_resultado = Convert.ToDouble(rubroMontosRow["agoabonos"]);
            montos.agosto_cargo_resultado = Convert.ToDouble(rubroMontosRow["agocargos"]);
            montos.agosto_total_resultado = Convert.ToDouble(rubroMontosRow["agototal"]);
            montos.septiembre_abono_resultado = Convert.ToDouble(rubroMontosRow["sepabonos"]);
            montos.septiembre_cargo_resultado = Convert.ToDouble(rubroMontosRow["sepcargos"]);
            montos.septiembre_total_resultado = Convert.ToDouble(rubroMontosRow["septotal"]);
            montos.octubre_abono_resultado = Convert.ToDouble(rubroMontosRow["octabonos"]);
            montos.octubre_cargo_resultado = Convert.ToDouble(rubroMontosRow["octcargos"]);
            montos.octubre_total_resultado = Convert.ToDouble(rubroMontosRow["octtotal"]);
            montos.noviembre_abono_resultado = Convert.ToDouble(rubroMontosRow["novabonos"]);
            montos.noviembre_cargo_resultado = Convert.ToDouble(rubroMontosRow["novcargos"]);
            montos.noviembre_total_resultado = Convert.ToDouble(rubroMontosRow["novtotal"]);
            montos.diciembre_abono_resultado = Convert.ToDouble(rubroMontosRow["dicabonos"]);
            montos.diciembre_cargo_resultado = Convert.ToDouble(rubroMontosRow["diccargos"]);
            montos.diciembre_total_resultado = Convert.ToDouble(rubroMontosRow["dictotal"]);
            montos.anio = Convert.ToInt32(rubroMontosRow["year"]);
            montos.fecha = fechaactual;
            montos.mes = fechaactual.Month;
            //montos.valor_tipo_cambio_resultado = cambiop;
            montos.centro_costo_id = EmpProy.id;
            montos.empresa_id = EmpProy.empresa_id;
            montos.modelo_negocio_id = modeloId;
            montos.proyecto_id = EmpProy.proyecto_id;
            montos.rubro_id = rubroId;
            montos.tipo_captura_id = TipoCapturaContable;

            return insertarMontos(montos);
        }

        private int BuildMontosFujo(DataTable sumaMontos, EmpresaCC EmpProy, Int64 modeloId, Int64 rubroId,
            DateTime fechaactual)
        {
            int numInserts = 0;
            Dictionary<int, MontosConsolidados> montosPorAnio =
                new Dictionary<int, MontosConsolidados>();
            foreach (DataRow rubf in sumaMontos.Rows)
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
                        montos.enero_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 2:
                        montos.febrero_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 3:
                        montos.marzo_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 4:
                        montos.abril_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 5:
                        montos.mayo_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 6:
                        montos.junio_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 7:
                        montos.julio_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 8:
                        montos.agosto_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 9:
                        montos.septiembre_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 10:
                        montos.octubre_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 11:
                        montos.noviembre_total_resultado = Convert.ToDouble(rubf["saldo"]);
                        break;
                    case 12:
                        montos.diciembre_total_resultado = Convert.ToDouble(rubf["saldo"]);
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
                //montos.valor_tipo_cambio_resultado = cambiop;
                montos.centro_costo_id = EmpProy.id;
                montos.empresa_id = EmpProy.empresa_id;
                montos.modelo_negocio_id = modeloId;
                montos.proyecto_id = EmpProy.proyecto_id;
                montos.rubro_id = rubroId;
                montos.tipo_captura_id = TipoCapturaFlujo;

                numInserts += insertarMontos(montos);
            }

            return numInserts;
        }
        
    }
}