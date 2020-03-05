using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ProformaHelper
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();
        private QueryExecuterSQL _queryExecuterSql=new QueryExecuterSQL();

        public List<ProformaDetalle> BuildProformaFromModeloAsTemplate(Int64 idCC, int anio, Int64 idTipoProforma,
            Int64 idTipoCaptura)
        {
            DataRow dataRow = _queryExecuter.ExecuteQueryUniqueresult(
                "select modelo_negocio_id,modelo_negocio_flujo_id from centro_costo where id=" + idCC);
            Int64 idModeloAproformar = -1;
            Int64 tipoCaptura = idTipoCaptura;
            if (tipoCaptura == TipoCapturaContable)
            {
                idModeloAproformar = ToInt64(dataRow["modelo_negocio_id"]);
            }
            else if (tipoCaptura == TipoCapturaFlujo)
            {
                idModeloAproformar = ToInt64(dataRow["modelo_negocio_flujo_id"]);
            }

            if (idModeloAproformar == -1)
            {
                throw new ArgumentException(
                    "No se pudo determinar el modelo con el que se proformara. El tipo de captura recibido fue " +
                    tipoCaptura);
            }

            List<Rubros> rubroses = GetRubrosFromModeloId(idModeloAproformar, false);

            List<ProformaDetalle> detallesAniosPosteriores =
                new ProformaDetalleDataAccessLayer().GetEjercicioPosterior(anio, idCC, idModeloAproformar,
                    idTipoCaptura, idTipoProforma);

            List<ProformaDetalle> proformaDetalles =
                buildProformaFromTemplate(rubroses, idCC, anio, idTipoProforma, idTipoCaptura);
            foreach (ProformaDetalle detalle in proformaDetalles)
            {
                foreach (ProformaDetalle posterior in detallesAniosPosteriores)
                {
                    if (detalle.rubro_id == posterior.rubro_id)
                    {
                        detalle.anios_posteriores_resultado = posterior.anios_posteriores_resultado;
                    }
                }
            }

            return proformaDetalles;
        }


        public List<ProformaDetalle> CompletaDetalles(List<ProformaDetalle> detCtas, CentroCostos centroCostos,
            Int64 idModeloNeg)
        {
            double porcentaje = centroCostos.porcentaje;
            List<Rubros> rubTots = GetRubrosFromModeloId(idModeloNeg, true);

            detCtas.ForEach(detalle => { detalle.clave_rubro = BuscaRubroPorId(detalle.rubro_id).clave; });
            detCtas.Sort((d1, d2) => { return d2.clave_rubro.Length.CompareTo(d1.clave_rubro.Length); });

            List<Rubros> rubrosTotNoEvaluados = new List<Rubros>();
            List<ProformaDetalle> detallesConstruidos = new List<ProformaDetalle>();

            foreach (Rubros rubTot in rubTots)
            {
                try
                {
                    detallesConstruidos.Add(ConstruyeDetalleTotal(detCtas, rubTot, porcentaje));
                }
                catch (EvaluateException ee)
                {
                    
                    Console.WriteLine("WARN: No pudo evaluarse la expresion '"+rubTot.aritmetica+"' ERROR: "+ee.Message);
                    rubrosTotNoEvaluados.Add(rubTot);
                }
            }
            detCtas.AddRange(detallesConstruidos);
            foreach (Rubros rubrosTotNoEvaluado in rubrosTotNoEvaluados)
            {
                try
                {
                    detCtas.Add(ConstruyeDetalleTotal(detCtas, rubrosTotNoEvaluado, 1.0));
                }
                catch (EvaluateException ee)
                {
                    Console.WriteLine("ERROR: No pudo evaluarse la expresion '"+rubrosTotNoEvaluado.aritmetica+"' ERROR: "+ee);
                    
                }
            }

            return detCtas;
        }

        private ProformaDetalle ConstruyeDetalleTotal(List<ProformaDetalle> detalles, Rubros rubroTotal,
            double porcentaje)
        {
            string aritmetica = rubroTotal.aritmetica;
            if (porcentaje != 1.0)
            {
                aritmetica = "(" + aritmetica + ") * " + porcentaje;
            }

            var aritmeticas = new Dictionary<string, string>();
            aritmeticas.Add("enero_monto", aritmetica);
            aritmeticas.Add("febrero_monto", aritmetica);
            aritmeticas.Add("marzo_monto", aritmetica);
            aritmeticas.Add("abril_monto", aritmetica);
            aritmeticas.Add("mayo_monto", aritmetica);
            aritmeticas.Add("junio_monto", aritmetica);
            aritmeticas.Add("julio_monto", aritmetica);
            aritmeticas.Add("agosto_monto", aritmetica);
            aritmeticas.Add("septiembre_monto", aritmetica);
            aritmeticas.Add("octubre_monto", aritmetica);
            aritmeticas.Add("noviembre_monto", aritmetica);
            aritmeticas.Add("diciembre_monto", aritmetica);
            aritmeticas.Add("ejercicio", aritmetica);
            aritmeticas.Add("acumulado", aritmetica);
            aritmeticas.Add("total", aritmetica);

            List<String> keys = new List<string>();
            foreach (var key in aritmeticas.Keys)
            {
                keys.Add(key);
            }
                
            detalles.ForEach(detalle =>
            {
                var claveRubro = detalle.clave_rubro;
                if (aritmetica.Contains(claveRubro))
                {
                    foreach(var key in keys)
                    {
                        aritmeticas[key] = aritmeticas[key]
                            .Replace(claveRubro, detalle[key+"_resultado"].ToString());
                    }
                }
            });
            ProformaDetalle detalleTotal = new ProformaDetalle();
            detalleTotal.rubro_id = rubroTotal.id;
            detalleTotal.nombre_rubro = rubroTotal.nombre;
            detalleTotal.aritmetica = aritmetica;
            detalleTotal.clave_rubro = rubroTotal.clave;
            detalleTotal.hijos = rubroTotal.hijos;

            DataTable dt = new DataTable();
            foreach(var key in keys)
            {
                detalleTotal[key+"_resultado"] = ToDouble(dt.Compute(aritmeticas[key], ""));
            }
            return detalleTotal;
        }

        public int getMesInicio(Int64 idTipoProforma)
        {
            DataRow dataRow =
                _queryExecuter.ExecuteQueryUniqueresult("select mes_inicio from tipo_proforma where id=" +
                                                        idTipoProforma);
            int mesInicio = ToInt32(dataRow["mes_inicio"]);
            return mesInicio;
        }

        private List<ProformaDetalle> buildProformaFromTemplate(List<Rubros> rubroses, Int64 idCC, int anio,
            Int64 idTipoProforma, Int64 idTipoCaptura)
        {
            List<Rubros> rubrosesreoder = reorderConceptos(rubroses);
            List<ProformaDetalle> detalles = new List<ProformaDetalle>();


            rubrosesreoder.ForEach(actual =>
            {
                ProformaDetalle detalle = new ProformaDetalle();
                detalle.mes_inicio = getMesInicio(idTipoProforma);
                detalle.modelo_negocio_id = actual.id_modelo_neg;
                detalle.anio = anio;
                detalle.centro_costo_id = idCC;
                detalle.tipo_proforma_id = idTipoProforma;
                detalle.tipo_captura_id = idTipoCaptura;

                detalle.activo = true;
                detalle.rubro_id = actual.id;
                detalle.nombre_rubro = actual.nombre;
                detalle.hijos = actual.hijos;
                detalle.enero_monto_resultado = 0;
                detalle.febrero_monto_resultado = 0;
                detalle.marzo_monto_resultado = 0;
                detalle.abril_monto_resultado = 0;
                detalle.mayo_monto_resultado = 0;
                detalle.junio_monto_resultado = 0;
                detalle.julio_monto_resultado = 0;
                detalle.agosto_monto_resultado = 0;
                detalle.septiembre_monto_resultado = 0;
                detalle.octubre_monto_resultado = 0;
                detalle.noviembre_monto_resultado = 0;
                detalle.diciembre_monto_resultado = 0;
                detalle.ejercicio_resultado = 0;
                detalle.aritmetica = actual.aritmetica;
                detalles.Add(detalle);
            });
            return detalles;
        }
        
        public List<ProformaDetalle> getAjustes(Int64 idCC,int anio,Int64 idTipoCaptura)
        {
            List<ProformaDetalle> proformaDetalles=new List<ProformaDetalle>();
            if (idTipoCaptura == TipoCapturaContable)//Los ajustes solo son para contable
            {
                Dictionary<string, string> mesValor = new Dictionary<string, string>();
                mesValor.Add("1", "enero_monto_resultado");
                mesValor.Add("2", "febrero_monto_resultado");
                mesValor.Add("3", "marzo_monto_resultado");
                mesValor.Add("4", "abril_monto_resultado");
                mesValor.Add("5", "mayo_monto_resultado");
                mesValor.Add("6", "junio_monto_resultado");
                mesValor.Add("7", "julio_monto_resultado");
                mesValor.Add("8", "agosto_monto_resultado");
                mesValor.Add("9", "septiembre_monto_resultado");
                mesValor.Add("10", "octubre_monto_resultado");
                mesValor.Add("11", "noviembre_monto_resultado");
                mesValor.Add("12", "diciembre_monto_resultado");
                Object empresaId =
                    _queryExecuter.ExecuteQueryUniqueresult("select empresa_id from centro_costo where id=" + idCC)[
                        "empresa_id"];

                DataTable ajustesDt = _queryExecuterSql.ExecuteQuerySQL("select ingreso, directo, indirecto, mes " +
                                                                        " from ajuste" +
                                                                        " where empresa = " + empresaId +
                                                                        " and centrocosto =" + idCC +
                                                                        " and anio =" + anio);
                DataRow dataRow =
                    _queryExecuter.ExecuteQueryUniqueresult("select modelo_negocio_id from centro_costo where id=" +
                                                            idCC);
                List<Rubros> rubroses = GetRubrosFromModeloId(Convert.ToInt64(dataRow["modelo_negocio_id"]), false);
                rubroses.ForEach(rubro =>
                {
                    ProformaDetalle detalle = new ProformaDetalle();
                    detalle.rubro_id = rubro.id;
                    detalle.campoEnAjustes = rubro.campoEnAjustes;
                    proformaDetalles.Add(detalle);
                });
                proformaDetalles.ForEach(detalle =>
                {
                    foreach (DataRow ajusteRow in ajustesDt.Rows)
                    {
                        Object mesData = ajusteRow["mes"];
                        if (mesData != null)
                        {
                            if (detalle.campoEnAjustes!=null&&detalle.campoEnAjustes.Trim().Length>0)
                            {
                                detalle[mesValor[mesData.ToString()]] = ToDouble(ajusteRow[detalle.campoEnAjustes]);
                            }
                        }
                    }
                });
            }

            return proformaDetalles;
        }

        public List<ProformaDetalle> setIdInterno(List<ProformaDetalle> detalles)
        {
            foreach (var detalle in detalles)
            {
                string uid=Guid.NewGuid().ToString();
                uid=uid.Substring(uid.Length-12);
                detalle.idInterno = uid;
            }

            return detalles;
        }

        public Boolean existePeridodoActivo(int anio, Int64 idTipoProforma, Int64 idTipoCaptura)
        {
            return _queryExecuter
                       .ExecuteQuery(
                           "select distinct anio_periodo from periodo where activo = true and estatus = 'true' and tipo_captura_id = " +
                           idTipoCaptura + " and tipo_proforma_id = " + idTipoProforma + " and anio_periodo=" + anio)
                       .Rows.Count > 0;

        }
        
        
        public Rubros BuscaRubroPorId(Int64 rubro_id)
        {
            string consulta = "";
            consulta += " select * ";
            consulta += " 	from rubro ";
            consulta += " 	where id = " + rubro_id;
            consulta += " 	and activo = 'true' ";
            DataRow rubroRow = _queryExecuter.ExecuteQueryUniqueresult(consulta);
            Rubros detRubros = transformRowToRubro(rubroRow);
            return detRubros;
        }

        private List<Rubros> GetRubrosFromModeloId(Int64 idModelo, Boolean totales)
        {
            string consulta = "";
            consulta += " select rub.* ";
            consulta += " 	from rubro rub ";
            consulta += " 	inner join tipo_rubro tip on rub.tipo_id = tip.id ";
            consulta += " 	where rub.id_modelo_neg = " + idModelo;
            if (totales)
            {
                consulta += " 	and tip.clave = 'RUBROS' ";
            }

            consulta += " 	and rub.activo = 'true' ";
            DataTable dataTable = _queryExecuter.ExecuteQuery(consulta);
            List<Rubros> lstRubrosTot = new List<Rubros>();
            foreach (DataRow rubrosRow in dataTable.Rows)
            {
                Rubros rubsObtenidos = transformRowToRubro(rubrosRow);
                lstRubrosTot.Add(rubsObtenidos);
            }

            return lstRubrosTot;
        }

        private Rubros transformRowToRubro(DataRow rubrosRow)
        {
            Rubros ru = new Rubros();
            ru.id = ToInt64(rubrosRow["id"]);
            ru.nombre = Convert.ToString(rubrosRow["nombre"]);
            ru.clave = Convert.ToString(rubrosRow["clave"]);
            ru.aritmetica = Convert.ToString(rubrosRow["aritmetica"]);
            ru.hijos = Convert.ToString(rubrosRow["hijos"]);
            ru.id_modelo_neg = ToInt64(rubrosRow["id_modelo_neg"]);
            ru.tipo_id = ToInt64(rubrosRow["tipo_id"]);
            ru.naturaleza = Convert.ToString(rubrosRow["naturaleza"]);

            return ru;
        }

        public List<T> reorderConceptos<T>(List<T> conceptos) where  T :IConceptoProforma
        {
            var rubrosReorder = new List<T>();
            var padres = getConceptosPadresFromList(conceptos);
            for (var i = 0; i < padres.Count; i++)
            {
                var padre = padres[i];
                rubrosReorder.Add(padre);
                var hijos = getConceptosHijosFromList(padre, conceptos);
                for (var j = 0; j < hijos.Count; j++)
                {
                    rubrosReorder.Add(hijos[j]);
                }
            }

            return rubrosReorder;
        }
        /*
         * Permite obtener la fecha de generacion de montos consolidados ya se mensual o semanal segun sea flujo o contable
         */
        public DateTime getLastFechaMontosConsol(int anio,Int64 idEmpresa,Int64 idModeloNegocio,Int64 idProyecto,Int64 idCenCos,Int64 idTipoCaptura)
        {
            string fechaHoy=DateTime.Today.ToString("dd/MM/yyyy");
            String consulta="";
            if (idTipoCaptura == Constantes.TipoCapturaContable)
            {
                consulta = "select max(fecha) as fecha from montos_consolidados where activo=true " +
                           " and fecha >=date_trunc('MONTH',to_date('"+fechaHoy+"','DD/MM/YYYY')) " +
                           " and fecha <= to_date('"+fechaHoy+"','DD/MM/YYYY') ";
            }
            else if(idTipoCaptura==TipoCapturaFlujo)
            {
                consulta = "select max(fecha) as fecha from montos_consolidados where activo=true " +
                           " and fecha >to_date('"+fechaHoy+"','DD/MM/YYYY')-7 " +
                           " and fecha <= to_date('"+fechaHoy+"','DD/MM/YYYY') ";
            }
            else
            {
                throw new DataException("El tipo de captura "+idTipoCaptura+" no esta soportado");
            }

            consulta += "	 and anio = " + anio; 
            consulta += "	 and empresa_id = " + idEmpresa; 
            consulta += "	 and modelo_negocio_id = " + idModeloNegocio; 
            consulta += "	 and proyecto_id = " + idProyecto;
            consulta += "	 and centro_costo_id = " + idCenCos; 
            consulta += "	 and tipo_captura_id = " + idTipoCaptura;
           Object objfecha= _queryExecuter.ExecuteQueryUniqueresult(consulta)["fecha"];
           if (objfecha != null)
           {
               return ToDateTime(objfecha);
           }

           throw new ApplicationException(String.Format(
               "No se encontraron registros para montos consolidados, con los datos de consulta: " +
               " anio='{0}',empresa_id='{1}',modelo_negocio_id='{2}',proyecto_id='{3}',centro_costo_id='{4}',tipo_captura_id='{5}'",
               anio, idEmpresa, idModeloNegocio, idProyecto, idCenCos, idTipoCaptura));
           

        }

        private List<T> getConceptosPadresFromList<T> (List<T> conceptos) where  T :IConceptoProforma
        {
            List<T> padres = new List<T>();
            conceptos.ForEach(rubros =>
            {
                if ((rubros.GetHijos() != null && rubros.GetHijos().Trim().Length > 0 ) || (rubros.GetAritmetica() != null && rubros.GetAritmetica().Trim().Length > 0))
                {
                    padres.Add(rubros);
                }
            });
            return padres;
        }

        private List<T> getConceptosHijosFromList<T>(T conceptoPadre, List<T> conceptos)where  T :IConceptoProforma
        {
            var hijos = new List<T>();
            if (conceptoPadre.GetHijos() != null)
            {
                var arrhijos = conceptoPadre.GetHijos().Split(',');
                for (var i = 0; i < arrhijos.Length; i++)
                {
                    if (arrhijos[i].Trim().Length > 0)
                    {
                        var found = findConceptoByIdInList(conceptos, ToInt64(arrhijos[i].Trim()));
                        if (found != null)
                        {
                            hijos.Add(found);
                        }

                    }
                }
            }

            return hijos;
        }

        private T findConceptoByIdInList<T>(List<T> conceptos, Int64 idConcepto)where  T :IConceptoProforma
        {
            for (var i = 0; i < conceptos.Count; i++)
            {
                var actual = conceptos[i];
                if (actual.GetIdConcepto()==idConcepto)
                {
                    return actual;
                }
            }
            return default;
        }
    }
}