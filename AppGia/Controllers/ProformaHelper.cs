﻿using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ProformaHelper
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();

        public List<ProformaDetalle> buildProformaFrom(Proforma proforma)
        {
            if (proforma.centro_costo_id == 0|| proforma.tipo_captura_id == 0||proforma.tipo_proforma_id == 0)
            {
                throw new ArgumentException("Alguno de los parametros es incorrecto. Revisar centro_costo_id, tipo_captura_id, tipo_proforma_id ");
            }
            DataRow dataRow = _queryExecuter.ExecuteQueryUniqueresult(
                "select modelo_negocio_id,modelo_negocio_flujo_id from centro_costo where id="+proforma.centro_costo_id);
            Int64 modeloAproformar=-1;
            Int64 tipoCaptura = proforma.tipo_captura_id;
            if (tipoCaptura == TipoCapturaContable)
            {
                modeloAproformar= Convert.ToInt64(dataRow["modelo_negocio_id"]);
            }else if (tipoCaptura == TipoCapturaFlujo)
            {
                modeloAproformar = Convert.ToInt64(dataRow["modelo_negocio_flujo_id"]);
            }

            if (modeloAproformar == -1)
            {
                throw new ArgumentException("No se pudo determinar el modelo con el que se proformara. El tipo de captura recibido fue "+tipoCaptura);
            }
            
            List<Rubros> rubroses = GetRubrosFromModeloId(modeloAproformar, false);


            return buildProformaFromTemplate(rubroses, proforma);
        }
        
        public List<ProformaDetalle> buildProformaFromTemplate(List<Rubros> rubroses, Proforma proforma)
        {
            List<Rubros> rubrosesreoder = reorderRubros(rubroses);
            List<ProformaDetalle> detalles = new List<ProformaDetalle>();
            DataRow dataRow = _queryExecuter.ExecuteQueryUniqueresult("select mes_inicio from tipo_proforma where id="+proforma.tipo_proforma_id);
            int mesInicio = Convert.ToInt32(dataRow["mes_inicio"]);
            
            rubrosesreoder.ForEach(actual =>
            {
                ProformaDetalle detalle = new ProformaDetalle();
                detalle.mes_inicio = mesInicio;
                detalle.modelo_negocio_id = actual.id_modelo_neg;
                detalle.anio = proforma.anio;
                detalle.centro_costo_id = proforma.centro_costo_id;
                detalle.tipo_proforma_id = proforma.tipo_proforma_id;
                detalle.tipo_captura_id = proforma.tipo_captura_id;

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

        public List<ProformaDetalle> CompletaDetalles(List<ProformaDetalle> detCtas, CentroCostos centroCostos,
            Int64 idModeloNeg)
        {
            List<Rubros> rubTots = GetRubrosFromModeloId(idModeloNeg,true);
            List<ProformaDetalle> totales = new List<ProformaDetalle>();
            foreach (Rubros rubTot in rubTots)
            {
                ProformaDetalle detTot =
                    ConstruyeDetalleTotal(detCtas, rubTot, centroCostos.porcentaje);
                totales.Add(detTot);
            }

            detCtas.AddRange(totales);
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
                detalle.clave_rubro = rubrosCta.clave;
                if (aritmetica.Contains(rubrosCta.clave))
                {
                    aritmeticas["enero"] = aritmeticas["enero"]
                        .Replace(rubrosCta.clave, detalle.enero_monto_resultado.ToString());
                    aritmeticas["febrero"] = aritmeticas["febrero"]
                        .Replace(rubrosCta.clave, detalle.febrero_monto_resultado.ToString());
                    aritmeticas["marzo"] = aritmeticas["marzo"]
                        .Replace(rubrosCta.clave, detalle.marzo_monto_resultado.ToString());
                    aritmeticas["abril"] = aritmeticas["abril"]
                        .Replace(rubrosCta.clave, detalle.abril_monto_resultado.ToString());
                    aritmeticas["mayo"] = aritmeticas["mayo"]
                        .Replace(rubrosCta.clave, detalle.mayo_monto_resultado.ToString());
                    aritmeticas["junio"] = aritmeticas["junio"]
                        .Replace(rubrosCta.clave, detalle.junio_monto_resultado.ToString());
                    aritmeticas["julio"] = aritmeticas["julio"]
                        .Replace(rubrosCta.clave, detalle.julio_monto_resultado.ToString());
                    aritmeticas["agosto"] = aritmeticas["agosto"]
                        .Replace(rubrosCta.clave, detalle.agosto_monto_resultado.ToString());
                    aritmeticas["septiembre"] = aritmeticas["septiembre"]
                        .Replace(rubrosCta.clave, detalle.septiembre_monto_resultado.ToString());
                    aritmeticas["octubre"] = aritmeticas["octubre"]
                        .Replace(rubrosCta.clave, detalle.octubre_monto_resultado.ToString());
                    aritmeticas["noviembre"] = aritmeticas["noviembre"]
                        .Replace(rubrosCta.clave, detalle.noviembre_monto_resultado.ToString());
                    aritmeticas["diciembre"] = aritmeticas["diciembre"]
                        .Replace(rubrosCta.clave, detalle.diciembre_monto_resultado.ToString());
                    aritmeticas["ejercicio"] = aritmeticas["ejercicio"]
                        .Replace(rubrosCta.clave, detalle.ejercicio_resultado.ToString());
                    aritmeticas["acumulado"] = aritmeticas["acumulado"]
                        .Replace(rubrosCta.clave, detalle.acumulado_resultado.ToString());
                    aritmeticas["total"] =
                        aritmeticas["total"].Replace(rubrosCta.clave, detalle.total_resultado.ToString());
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
            proformaDetalleTotal.septiembre_monto_resultado =
                Convert.ToDouble(dt.Compute(aritmeticas["septiembre"], ""));
            proformaDetalleTotal.octubre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["octubre"], ""));
            proformaDetalleTotal.noviembre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["noviembre"], ""));
            proformaDetalleTotal.diciembre_monto_resultado = Convert.ToDouble(dt.Compute(aritmeticas["diciembre"], ""));
            proformaDetalleTotal.ejercicio_resultado = Convert.ToDouble(dt.Compute(aritmeticas["ejercicio"], ""));
            proformaDetalleTotal.acumulado_resultado = Convert.ToDouble(dt.Compute(aritmeticas["acumulado"], ""));
            proformaDetalleTotal.total_resultado = Convert.ToDouble(dt.Compute(aritmeticas["total"], ""));
            return proformaDetalleTotal;
        }

        private Rubros BuscaRubroPorId(Int64 rubro_id)
        {
            string consulta = "";
            consulta += " select * ";
            consulta += " 	from rubro ";
            consulta += " 	where id = " + rubro_id;
            consulta += " 	and activo = 'true' ";
            DataRow rubroRow = _queryExecuter.ExecuteQueryUniqueresult(consulta);
            Rubros detRubros=transformRowToRubro(rubroRow);
            return detRubros;
        }

        private List<Rubros> GetRubrosFromModeloId(Int64 idModelo,Boolean totales)
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
            ru.id = Convert.ToInt64(rubrosRow["id"]);
            ru.nombre = Convert.ToString(rubrosRow["nombre"]);
            ru.clave = Convert.ToString(rubrosRow["clave"]);
            ru.aritmetica = Convert.ToString(rubrosRow["aritmetica"]);
            ru.hijos = Convert.ToString(rubrosRow["hijos"]);
            ru.id_modelo_neg = Convert.ToInt64(rubrosRow["id_modelo_neg"]);
            ru.tipo_id = Convert.ToInt64(rubrosRow["tipo_id"]);
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
                    var found = findRubroByIdInList(rubroses, Convert.ToInt64(arrhijos[i].Trim()));
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
                if (actual.id.Equals(id))
                {
                    return actual;
                }
            }

            return null;
        }
    }
}