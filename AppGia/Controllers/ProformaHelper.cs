﻿using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ProformaHelper
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();
        private QueryExecuterSQL _queryExecuterSql=new QueryExecuterSQL();

        public List<ProformaDetalle> buildProformaFromModeloAsTemplate(Int64 idCC, int anio, Int64 idTipoProforma,
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

            foreach (Rubros rubTot in rubTots)
            {
                try
                {
                    detCtas.Add(ConstruyeDetalleTotal(detCtas, rubTot, porcentaje));
                }
                catch (EvaluateException ee)
                {
                    
                    Console.WriteLine("WARN: No pudo evaluarse la expresion '"+rubTot.aritmetica+"' ERROR: "+ee.Message);
                    rubrosTotNoEvaluados.Add(rubTot);
                }
            }

            foreach (Rubros rubrosTotNoEvaluado in rubrosTotNoEvaluados)
            {
                try
                {
                    detCtas.Add(ConstruyeDetalleTotal(detCtas, rubrosTotNoEvaluado, porcentaje));
                }
                catch (EvaluateException ee)
                {
                    Console.WriteLine("ERROR: No pudo evaluarse la expresion '"+rubrosTotNoEvaluado.aritmetica+"' ERROR: "+ee);
                    
                }
            }

            return detCtas;
        }

        public ProformaDetalle ConstruyeDetalleTotal(List<ProformaDetalle> detalles, Rubros rubroTotal,
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


            detalles.ForEach(detalle =>
            {
                var claveRubro = detalle.clave_rubro;
                if (aritmetica.Contains(claveRubro))
                {
                    foreach(var key in aritmeticas.Keys)
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

            DataTable dt = new DataTable();
            foreach(var key in aritmeticas.Keys)
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
            List<Rubros> rubrosesreoder = reorderRubros(rubroses);
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

        public List<ProformaDetalle> getAjustes(Int64 idCC,Int64 idEmpresa,int anio)
        {
            List<ProformaDetalle> proformaDetalles=new List<ProformaDetalle>();
            DataTable ajustesDt = _queryExecuterSql.ExecuteQuerySQL("select ingreso, directo, indirecto, mes " +
                                                                    " from ajuste" +
                                                                    " where empresa = "+idEmpresa +
                                                                    " and centrocosto ="+ idCC+
                                                                    " and anio ="+anio);
            DataRow dataRow = _queryExecuter.ExecuteQueryUniqueresult("select modelo_negocio_id from centro_costo where id=" + idCC);
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
                        Int32 mesAjuste = Convert.ToInt32(mesData);
                        if (mesAjuste == 1)
                        {
                            detalle.enero_monto_resultado = ToDouble(detalle.campoEnAjustes);
                        }
                        //aun faltan los demas meses
                    }
                }
            });
            
            return null;
        }
        private Rubros BuscaRubroPorId(Int64 rubro_id)
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

        private List<Rubros> reorderRubros(List<Rubros> rubroses)
        {
            var rubrosReorder = new List<Rubros>();
            var padres = getRubrosPadresFromList(rubroses);
            for (var i = 0; i < padres.Count; i++)
            {
                var padre = padres[i];
                rubrosReorder.Add(padre);
                var hijos = getRubrosHijosFromList(padre, rubroses);
                for (var j = 0; j < hijos.Count; j++)
                {
                    rubrosReorder.Add(hijos[j]);
                }
            }

            return rubrosReorder;
        }

        private List<Rubros> getRubrosPadresFromList(List<Rubros> rubroses)
        {
            List<Rubros> padres = new List<Rubros>();
            rubroses.ForEach(rubros =>
            {
                if (rubros.hijos != null || rubros.aritmetica != null)
                {
                    padres.Add(rubros);
                }
            });
            return padres;
        }

        private List<Rubros> getRubrosHijosFromList(Rubros padre, List<Rubros> rubroses)
        {
            var hijos = new List<Rubros>();
            if (padre.hijos != null)
            {
                var arrhijos = padre.hijos.Split(',');
                for (var i = 0; i < arrhijos.Length; i++)
                {
                    var found = findRubroByIdInList(rubroses, ToInt64(arrhijos[i].Trim()));
                    if (found != null)
                    {
                        hijos.Add(found);
                    }
                }
            }

            return hijos;
        }

        private Rubros findRubroByIdInList(List<Rubros> rubroses, Int64 id)
        {
            for (var i = 0; i < rubroses.Count; i++)
            {
                var actual = rubroses[i];
                if (actual.id==id)
                {
                    return actual;
                }
            }

            return null;
        }
    }
}