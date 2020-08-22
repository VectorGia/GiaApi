using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AppGia.Models;
using AppGia.Util;
using NLog;
using Npgsql;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Dao
{
    public class PreProformaDataAccessLayer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private BatchExecuter _batchExecuter;
        private static string insertMontosQUery = "INSERT INTO montos_consolidados(" +
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

        public static string inactivateExistentesContableQuery =
            "update " +
            " montos_consolidados cns" +
            " set activo= false" +
            " where cns.fecha" +
            " in (" +
            " select fecha" +
            " from montos_consolidados cns" +
            "     where empresa_id = @empresa_id" +
            " AND modelo_negocio_id = @modelo_negocio_id" +
            " AND proyecto_id = @proyecto_id" +
            " AND centro_costo_id = @centro_costo_id" +
            " AND tipo_captura_id = @tipo_captura_id" +
            " AND activo = true" +
            " and date_trunc('DAY', fecha)::date >= date_trunc('MONTH',@fechaEjecucion)::date)" +
            " and empresa_id = @empresa_id" +
            " AND modelo_negocio_id = @modelo_negocio_id" +
            " AND proyecto_id = @proyecto_id" +
            " AND centro_costo_id = @centro_costo_id" +
            " AND tipo_captura_id = @tipo_captura_id" +
            " AND activo = true "+
            " AND anio >=  @anioFechaEjecucion";

        public static string inactivateExistentesFlujoQuery =
            "update " +
            " montos_consolidados cns" +
            " set activo= false" +
            " where cns.fecha" +
            " in (" +
            " select fecha" +
            " from montos_consolidados cns" +
            "     where empresa_id = @empresa_id" +
            " AND modelo_negocio_id = @modelo_negocio_id" +
            " AND proyecto_id = @proyecto_id" +
            " AND centro_costo_id = @centro_costo_id" +
            " AND tipo_captura_id = @tipo_captura_id" +
            " AND activo = true" +
            " and date_trunc('DAY', fecha)::date > date_trunc('DAY', @fechaEjecucion)::date - 7 )" +
            " and empresa_id = @empresa_id" +
            " AND modelo_negocio_id = @modelo_negocio_id" +
            " AND proyecto_id = @proyecto_id" +
            " AND centro_costo_id = @centro_costo_id" +
            " AND tipo_captura_id = @tipo_captura_id" +
            " AND activo = true"+
            " AND anio >=  @anioFechaEjecucion";


        private QueryExecuter _queryExecuter = new QueryExecuter();


        public int MontosConsolidados()
        {
            return MontosConsolidados(true, true);
        }

        public int MontosConsolidados(Boolean contable, Boolean flujo)
        {
            _batchExecuter=new BatchExecuter(insertMontosQUery);
            StopWatch sw= new StopWatch("MontosConsolidados");
            sw.start("GetCentrosCostro");
            DataTable centrosCostoDt = GetCentrosCostro();
            sw.stop();
            sw.start("Manejo de modelos");
            DateTime fechaactual = DateTime.Now;
            int numInserts = 0;

            foreach (DataRow centrosCostoRow in centrosCostoDt.Rows)
            {
                CentroCostos centroCostos = new CentroCostos();
                centroCostos.id = ToInt64(centrosCostoRow["id"]);
                centroCostos.desc_id = centrosCostoRow["desc_id"].ToString();
                centroCostos.empresa_id = ToInt64(centrosCostoRow["empresa_id"]);
                centroCostos.proyecto_id = ToInt64(centrosCostoRow["proyecto_id"]);
                centroCostos.modelo_negocio_id = ToInt64(centrosCostoRow["modelo_negocio_id"]);
                centroCostos.modelo_negocio_flujo_id = ToInt64(centrosCostoRow["modelo_negocio_flujo_id"]);

                Empresa empresa = new EmpresaDataAccessLayer().GetEmpresaData(centroCostos.empresa_id);
                if (contable)
                {
                    _batchExecuter.addCommand(inactivateExistentesContableQuery, 
                        new NpgsqlParameter("@fechaEjecucion", DateTime.Now),
                        new NpgsqlParameter("@empresa_id", centroCostos.empresa_id),
                        new NpgsqlParameter("@modelo_negocio_id", centroCostos.modelo_negocio_id),
                        new NpgsqlParameter("@proyecto_id",  centroCostos.proyecto_id),
                        new NpgsqlParameter("@centro_costo_id", centroCostos.id),
                        new NpgsqlParameter("@tipo_captura_id", TipoCapturaContable),
                        new NpgsqlParameter("@aniofechaejecucion", DateTime.Now.Year)
                        );
                    manageModeloContable(centroCostos, empresa, fechaactual);
                }

                if (flujo)
                {
                    _batchExecuter.addCommand(inactivateExistentesFlujoQuery, 
                        new NpgsqlParameter("@fechaEjecucion", DateTime.Now),
                        new NpgsqlParameter("@empresa_id", centroCostos.empresa_id),
                        new NpgsqlParameter("@modelo_negocio_id", centroCostos.modelo_negocio_flujo_id),
                        new NpgsqlParameter("@proyecto_id",  centroCostos.proyecto_id),
                        new NpgsqlParameter("@centro_costo_id", centroCostos.id),
                        new NpgsqlParameter("@tipo_captura_id", TipoCapturaFlujo),
                        new NpgsqlParameter("@aniofechaejecucion", DateTime.Now.Year)
                    );
                    manageModeloFlujo(centroCostos, empresa, fechaactual);
                }
            }
            sw.stop();
            sw.start("inserts");
            numInserts += _batchExecuter.executeCommands();
            sw.stop();
            logger.Info(sw.prettyPrint());
            return numInserts;
        }

        private void manageModeloContable(CentroCostos centroCostos, Empresa empresa, DateTime fechaactual)
        {
            StopWatch swGral = new StopWatch($"manageModeloContable cc={centroCostos.desc_id} emp={empresa.desc_id} fec={fechaactual}");
            swGral.start();
            int numInserts = 0;
            Int64 modeloId = centroCostos.modelo_negocio_id;

            Modelo_Negocio mn = new ModeloNegocioDataAccessLayer().GetModelo(modeloId.ToString());
            if (!mn.activo)
            {
                return ;
            }

            List<Rubros> rubrosDeModelo = GetRubrosFromModeloId(modeloId);
            Int64 numRegistrosExistentes = getNumMontosOfTipoCaptura(TipoCapturaContable);
            GeneraQry qry = new GeneraQry("balanza", "cuenta_unificada", 12);
            ConcurrentDictionary<Int32, byte> aniosDefecto = new ConcurrentDictionary<Int32, byte>();
            ConcurrentQueue<Rubros> rubrosSinMontos = new ConcurrentQueue<Rubros>();

            Parallel.ForEach(rubrosDeModelo, (rubro) =>
            {
                StopWatch sw=new StopWatch(
                    $"consulta_contables cc.id='{centroCostos.id}',empr.id='{centroCostos.empresa_id}',proy.id='{centroCostos.proyecto_id}',modelo.id='{centroCostos.modelo_negocio_flujo_id}',rubro.id='{rubro.id}'");
                sw.start("getQuerySums");
                String consulta = qry.getQuerySums(rubro, centroCostos, empresa, numRegistrosExistentes);

                logger.Debug(
                    "consulta_contables cc.id='{0}',empr.id='{1}',proy.id='{2}',modelo.id='{3}',rubro.id='{4}', ===>> '{5}'",
                    centroCostos.id, centroCostos.empresa_id, centroCostos.proyecto_id, centroCostos.modelo_negocio_id,
                    rubro.id, consulta);

                DataTable sumaMontosDt = new QueryExecuter().ExecuteQuery(consulta);
                sw.stop();
                sw.start("BuildMontosConsolContable");

                if (sumaMontosDt.Rows.Count > 0)
                {
                    foreach (DataRow rubroMontosRow in sumaMontosDt.Rows)
                    {
                        BuildMontosConsolContable(rubroMontosRow, centroCostos, modeloId, rubro.id,
                            fechaactual);
                        aniosDefecto.TryAdd(ToInt32(rubroMontosRow["year"]), 1);
                    }
                }
                else
                {
                    rubrosSinMontos.Enqueue(rubro);
                }
                sw.stop();
                logger.Info(sw.prettyPrint());
            });
            
            foreach (var rubro in rubrosSinMontos)
            {
                foreach (var anio in aniosDefecto.Keys)
                {
                    BuildMontosConsolContable(centroCostos, modeloId, rubro.id,
                        fechaactual, anio);
                }
            }
            swGral.stop();
            logger.Info(swGral.prettyPrint());
        }

        private void manageModeloFlujo(CentroCostos centroCostos, Empresa empresa, DateTime fechaactual)
        {
            StopWatch swGral = new StopWatch($"manageModeloFlujo cc={centroCostos.desc_id} emp={empresa.desc_id} fec={fechaactual}");
            swGral.start();
            int numInserts = 0;
            Int64 modeloId = centroCostos.modelo_negocio_flujo_id;
            Modelo_Negocio mn = new ModeloNegocioDataAccessLayer().GetModelo(modeloId.ToString());
            if (!mn.activo)
            {
                return ;
            }

            List<Rubros> rubrosDeModelo = GetRubrosFromModeloId(modeloId);
            Int64 numRegistrosExistentes = getNumMontosOfTipoCaptura(TipoCapturaFlujo);
            ConcurrentQueue<Rubros> rubrosSinMontos = new ConcurrentQueue<Rubros>();
            ConcurrentDictionary<Int32, byte> aniosDefault = new ConcurrentDictionary<Int32, byte>();
            Parallel.ForEach(rubrosDeModelo, (rubro) =>
            {
                StopWatch sw=new StopWatch(
                    $"consulta_flujo cc.id='{centroCostos.id}',empr.id='{centroCostos.empresa_id}',proy.id='{centroCostos.proyecto_id}',modelo.id='{centroCostos.modelo_negocio_flujo_id}',rubro.id='{rubro.id}'");
                sw.start("getQuerySemanalSums");
                GeneraQry qry = new GeneraQry("semanal", "itm::text", 2);
                String consulta = qry.getQuerySemanalSums(rubro, centroCostos, empresa, numRegistrosExistentes);
                logger.Debug(
                    "consulta_flujo cc.id='{0}',empr.id='{1}',proy.id='{2}',modelo.id='{3}',rubro.id='{4}', ===>> '{5}'",
                    centroCostos.id, centroCostos.empresa_id, centroCostos.proyecto_id,
                    centroCostos.modelo_negocio_flujo_id, rubro.id, consulta);
                  DataTable sumaMontos = new QueryExecuter().ExecuteQuery(consulta);
                  sw.stop();
                  sw.start("BuildMontosFujo");

                if (sumaMontos.Rows.Count > 0)
                {
                     BuildMontosFujo(sumaMontos, centroCostos, modeloId, rubro.id, fechaactual,
                        aniosDefault);
                  
                }
                else
                {
                    rubrosSinMontos.Enqueue(rubro);
                }
                sw.stop();
                logger.Info(sw.prettyPrint());
            });

            foreach (var rubro in rubrosSinMontos)
            {
                foreach (var anio in aniosDefault.Keys)
                {
                    MontosConsolidados montos = new MontosConsolidados();
                    montos.anio = anio;
                    montos.activo = true;
                    montos.fecha = fechaactual;
                    montos.mes = fechaactual.Month;
                    montos.centro_costo_id = centroCostos.id;
                    montos.empresa_id = centroCostos.empresa_id;
                    montos.modelo_negocio_id = modeloId;
                    montos.proyecto_id = centroCostos.proyecto_id;
                    montos.rubro_id = rubro.id;
                    montos.tipo_captura_id = TipoCapturaFlujo;
                    insertarMontos(montos);
                }
            }
            swGral.stop();
            logger.Info(swGral.prettyPrint());
        }

        public DataTable findRubrosByIdModelo(Int64 modelo_id)
        {
            return _queryExecuter.ExecuteQuery("SELECT * FROM rubro WHERE activo=true and tipo_id = " +
                                               TipoRubroCuentas +
                                               " AND id_modelo_neg = " +
                                               modelo_id);
        }

        public List<Rubros> GetRubrosFromModeloId(Int64 modeloId)
        {
            List<Rubros> listaRubros = new List<Rubros>();
            DataTable rubrosDt = findRubrosByIdModelo(modeloId);
            foreach (DataRow rubrosRow in rubrosDt.Rows)
            {
                Rubros rubro = new Rubros();
                rubro.id = ToInt64(rubrosRow["id"]);
                rubro.activo = ToBoolean(rubrosRow["activo"]);
                rubro.nombre = rubrosRow["nombre"].ToString();
                rubro.aritmetica = rubrosRow["aritmetica"].ToString();
                rubro.clave = rubrosRow["clave"].ToString();
                rubro.rango_cuentas_excluidas = rubrosRow["rango_cuentas_excluidas"].ToString();
                rubro.rangos_cuentas_incluidas = rubrosRow["rangos_cuentas_incluidas"].ToString();
                rubro.tipo_id = ToInt64(rubrosRow["tipo_id"]);
                rubro.id_modelo_neg = ToInt64(rubrosRow["id_modelo_neg"]);
                rubro.hijos = rubrosRow["hijos"].ToString();
                rubro.naturaleza = rubrosRow["naturaleza"].ToString();
                listaRubros.Add(rubro);
            }

            return listaRubros;
        }

        public DataTable GetCentrosCostro()
        {
            return _queryExecuter.ExecuteQuery(
                "SELECT * FROM centro_costo WHERE activo = true and modelo_negocio_id is not null and modelo_negocio_flujo_id is not null");
        }


        public void insertarMontos(MontosConsolidados montos)
        {
            
             _batchExecuter.addCommand( new NpgsqlParameter("@activo", montos.activo),
                new NpgsqlParameter("@enero_abono_resultado", montos.enero_abono_resultado),
                new NpgsqlParameter("@enero_cargo_resultado", montos.enero_cargo_resultado),
                new NpgsqlParameter("@enero_total_resultado", montos.enero_total_resultado),
                new NpgsqlParameter("@febrero_abono_resultado", montos.febrero_abono_resultado),
                new NpgsqlParameter("@febrero_cargo_resultado", montos.febrero_cargo_resultado),
                new NpgsqlParameter("@febrero_total_resultado", montos.febrero_total_resultado),
                new NpgsqlParameter("@marzo_abono_resultado", montos.marzo_abono_resultado),
                new NpgsqlParameter("@marzo_cargo_resultado", montos.marzo_cargo_resultado),
                new NpgsqlParameter("@marzo_total_resultado", montos.marzo_total_resultado),
                new NpgsqlParameter("@abril_abono_resultado", montos.abril_abono_resultado),
                new NpgsqlParameter("@abril_cargo_resultado", montos.abril_cargo_resultado),
                new NpgsqlParameter("@abril_total_resultado", montos.abril_total_resultado),
                new NpgsqlParameter("@mayo_abono_resultado", montos.mayo_abono_resultado),
                new NpgsqlParameter("@mayo_cargo_resultado", montos.mayo_cargo_resultado),
                new NpgsqlParameter("@mayo_total_resultado", montos.mayo_total_resultado),
                new NpgsqlParameter("@junio_abono_resultado", montos.junio_abono_resultado),
                new NpgsqlParameter("@junio_cargo_resultado", montos.junio_cargo_resultado),
                new NpgsqlParameter("@junio_total_resultado", montos.junio_total_resultado),
                new NpgsqlParameter("@julio_abono_resultado", montos.julio_abono_resultado),
                new NpgsqlParameter("@julio_cargo_resultado", montos.julio_cargo_resultado),
                new NpgsqlParameter("@julio_total_resultado", montos.julio_total_resultado),
                new NpgsqlParameter("@agosto_abono_resultado", montos.agosto_abono_resultado),
                new NpgsqlParameter("@agosto_cargo_resultado", montos.agosto_cargo_resultado),
                new NpgsqlParameter("@agosto_total_resultado", montos.agosto_total_resultado),
                new NpgsqlParameter("@septiembre_abono_resultado", montos.septiembre_abono_resultado),
                new NpgsqlParameter("@septiembre_cargo_resultado", montos.septiembre_cargo_resultado),
                new NpgsqlParameter("@septiembre_total_resultado", montos.septiembre_total_resultado),
                new NpgsqlParameter("@octubre_abono_resultado", montos.octubre_abono_resultado),
                new NpgsqlParameter("@octubre_cargo_resultado", montos.octubre_cargo_resultado),
                new NpgsqlParameter("@octubre_total_resultado", montos.octubre_total_resultado),
                new NpgsqlParameter("@noviembre_abono_resultado", montos.noviembre_abono_resultado),
                new NpgsqlParameter("@noviembre_cargo_resultado", montos.noviembre_cargo_resultado),
                new NpgsqlParameter("@noviembre_total_resultado", montos.noviembre_total_resultado),
                new NpgsqlParameter("@diciembre_abono_resultado", montos.diciembre_abono_resultado),
                new NpgsqlParameter("@diciembre_cargo_resultado", montos.diciembre_cargo_resultado),
                new NpgsqlParameter("@diciembre_total_resultado", montos.diciembre_total_resultado),
                new NpgsqlParameter("@anio", montos.anio),
                new NpgsqlParameter("@fecha", montos.fecha),
                new NpgsqlParameter("@mes", montos.mes),
                new NpgsqlParameter("@valor_tipo_cambio_resultado", montos.valor_tipo_cambio_resultado),
                new NpgsqlParameter("@centro_costo_id", montos.centro_costo_id),
                new NpgsqlParameter("@empresa_id", montos.empresa_id),
                new NpgsqlParameter("@modelo_negocio_id", montos.modelo_negocio_id),
                new NpgsqlParameter("@proyecto_id", montos.proyecto_id),
                new NpgsqlParameter("@rubro_id", montos.rubro_id),
                new NpgsqlParameter("@tipo_captura_id", montos.tipo_captura_id));
        }

        public Int64 getNumMontosOfTipoCaptura(Int64 captura)
        {
            string consulta = "SELECT count(1) as numregs FROM montos_consolidados WHERE activo=true and tipo_captura_id = " + captura;
            return ToInt64(_queryExecuter.ExecuteQuery(consulta).Rows[0]["numregs"]);
        }

        private void BuildMontosConsolContable(CentroCostos centroCostos, Int64 modeloId,Int64 rubroId, DateTime fechaactual, Int32 anio)
        {
            MontosConsolidados montos = new MontosConsolidados();
            montos.activo = true;

            montos.anio = ToInt32(anio);
            montos.fecha = fechaactual;
            montos.mes = fechaactual.Month;
            montos.centro_costo_id = centroCostos.id;
            montos.empresa_id = centroCostos.empresa_id;
            montos.modelo_negocio_id = modeloId;
            montos.proyecto_id = centroCostos.proyecto_id;
            montos.rubro_id = rubroId;
            montos.tipo_captura_id = TipoCapturaContable;

             insertarMontos(montos);
        }

        private void BuildMontosConsolContable(DataRow rubroMontosRow, CentroCostos centroCostos, Int64 modeloId, Int64 rubroId, DateTime fechaactual)
        {
            MontosConsolidados montos = new MontosConsolidados();
            montos.activo = true;
            montos.enero_abono_resultado = ToDouble(rubroMontosRow["eneabonos"]);
            montos.enero_cargo_resultado = ToDouble(rubroMontosRow["enecargos"]);
            montos.enero_total_resultado = ToDouble(rubroMontosRow["enetotal"]);
            montos.febrero_abono_resultado = ToDouble(rubroMontosRow["febabonos"]);
            montos.febrero_cargo_resultado = ToDouble(rubroMontosRow["febcargos"]);
            montos.febrero_total_resultado = ToDouble(rubroMontosRow["febtotal"]);
            montos.marzo_abono_resultado = ToDouble(rubroMontosRow["marabonos"]);
            montos.marzo_cargo_resultado = ToDouble(rubroMontosRow["marcargos"]);
            montos.marzo_total_resultado = ToDouble(rubroMontosRow["martotal"]);
            montos.abril_abono_resultado = ToDouble(rubroMontosRow["abrabonos"]);
            montos.abril_cargo_resultado = ToDouble(rubroMontosRow["abrcargos"]);
            montos.abril_total_resultado = ToDouble(rubroMontosRow["abrtotal"]);
            montos.mayo_abono_resultado = ToDouble(rubroMontosRow["mayabonos"]);
            montos.mayo_cargo_resultado = ToDouble(rubroMontosRow["maycargos"]);
            montos.mayo_total_resultado = ToDouble(rubroMontosRow["maytotal"]);
            montos.junio_abono_resultado = ToDouble(rubroMontosRow["junabonos"]);
            montos.junio_cargo_resultado = ToDouble(rubroMontosRow["juncargos"]);
            montos.junio_total_resultado = ToDouble(rubroMontosRow["juntotal"]);
            montos.julio_abono_resultado = ToDouble(rubroMontosRow["julabonos"]);
            montos.julio_cargo_resultado = ToDouble(rubroMontosRow["julcargos"]);
            montos.julio_total_resultado = ToDouble(rubroMontosRow["jultotal"]);
            montos.agosto_abono_resultado = ToDouble(rubroMontosRow["agoabonos"]);
            montos.agosto_cargo_resultado = ToDouble(rubroMontosRow["agocargos"]);
            montos.agosto_total_resultado = ToDouble(rubroMontosRow["agototal"]);
            montos.septiembre_abono_resultado = ToDouble(rubroMontosRow["sepabonos"]);
            montos.septiembre_cargo_resultado = ToDouble(rubroMontosRow["sepcargos"]);
            montos.septiembre_total_resultado = ToDouble(rubroMontosRow["septotal"]);
            montos.octubre_abono_resultado = ToDouble(rubroMontosRow["octabonos"]);
            montos.octubre_cargo_resultado = ToDouble(rubroMontosRow["octcargos"]);
            montos.octubre_total_resultado = ToDouble(rubroMontosRow["octtotal"]);
            montos.noviembre_abono_resultado = ToDouble(rubroMontosRow["novabonos"]);
            montos.noviembre_cargo_resultado = ToDouble(rubroMontosRow["novcargos"]);
            montos.noviembre_total_resultado = ToDouble(rubroMontosRow["novtotal"]);
            montos.diciembre_abono_resultado = ToDouble(rubroMontosRow["dicabonos"]);
            montos.diciembre_cargo_resultado = ToDouble(rubroMontosRow["diccargos"]);
            montos.diciembre_total_resultado = ToDouble(rubroMontosRow["dictotal"]);
            montos.anio = ToInt32(rubroMontosRow["year"]);
            montos.fecha = fechaactual;
            montos.mes = fechaactual.Month;
            //montos.valor_tipo_cambio_resultado = cambiop;
            montos.centro_costo_id = centroCostos.id;
            montos.empresa_id = centroCostos.empresa_id;
            montos.modelo_negocio_id = modeloId;
            montos.proyecto_id = centroCostos.proyecto_id;
            montos.rubro_id = rubroId;
            montos.tipo_captura_id = TipoCapturaContable;

            insertarMontos(montos);
        }

        private void BuildMontosFujo(DataTable sumaMontos, CentroCostos centroCostos, Int64 modeloId, Int64 rubroId,
            DateTime fechaactual, ConcurrentDictionary<Int32, byte> aniosdefault)
        {
            int numInserts = 0;
            Dictionary<int, MontosConsolidados> montosPorAnio =
                new Dictionary<int, MontosConsolidados>();

            foreach (DataRow rubf in sumaMontos.Rows)
            {
                int year = ToInt32(rubf["year"]);
                aniosdefault.TryAdd(year, 1);
                if (!montosPorAnio.ContainsKey(year))
                {
                    montosPorAnio.Add(year, new MontosConsolidados());
                }

                MontosConsolidados montos = montosPorAnio[year];
                Double mes = ToDouble(rubf["mes"]);

                switch (mes)
                {
                    case 1:
                        montos.enero_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 2:
                        montos.febrero_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 3:
                        montos.marzo_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 4:
                        montos.abril_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 5:
                        montos.mayo_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 6:
                        montos.junio_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 7:
                        montos.julio_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 8:
                        montos.agosto_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 9:
                        montos.septiembre_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 10:
                        montos.octubre_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 11:
                        montos.noviembre_total_resultado = ToDouble(rubf["saldo"]);
                        break;
                    case 12:
                        montos.diciembre_total_resultado = ToDouble(rubf["saldo"]);
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
                montos.centro_costo_id = centroCostos.id;
                montos.empresa_id = centroCostos.empresa_id;
                montos.modelo_negocio_id = modeloId;
                montos.proyecto_id = centroCostos.proyecto_id;
                montos.rubro_id = rubroId;
                montos.tipo_captura_id = TipoCapturaFlujo;

                 insertarMontos(montos);
            }
            
        }
    }
}