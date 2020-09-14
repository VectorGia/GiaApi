using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using AppGia.Models;
using NLog;
using static System.Convert;

namespace AppGia.Helpers
{
    public class ProformaCalc
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
     

        public void recalculateAll(List<ProformaDetalle> detalles, bool calcularTotales)
        {
            sumColumns(detalles, "ejercicio_resultado", new[]
            {
                "enero_monto_resultado",
                "febrero_monto_resultado", "marzo_monto_resultado", "abril_monto_resultado",
                "mayo_monto_resultado", "junio_monto_resultado", "julio_monto_resultado", "agosto_monto_resultado",
                "septiembre_monto_resultado", "octubre_monto_resultado", "noviembre_monto_resultado",
                "diciembre_monto_resultado"
            });
            sumColumns(detalles, "total_resultado",
                new[] {"ejercicio_resultado", "acumulado_resultado", "anios_posteriores_resultado"});
            if (calcularTotales)
            {
                calculaDetTot(detalles, this.getDetallesTotales(detalles));
            }
        }

        private void sumColumns(List<ProformaDetalle> detalles, string targetColumn,
            string[] columnsNames)
        {
            for (var i = 0; i < detalles.Count; i++)
            {
                sumColumnsForDetalle(detalles[i], targetColumn, columnsNames);
            }
        }

        private void sumColumnsForDetalle(ProformaDetalle detalle, string targetColumn,
            string[] columnsNames)
        {
            var suma = 0.0;
            for (var j = 0; j < columnsNames.Length; j++)
            {
                var colName = columnsNames[j];
                try
                {
                    suma += ToDouble(detalle[colName]);
                }
                catch (Exception e)
                {
                    logger.Warn("La columna " + colName + "del detalle " + detalle.nombre_rubro + ", no es numero");
                }
            }

            detalle[targetColumn] = suma;
        }

        private List<ProformaDetalle> getDetallesTotales(List<ProformaDetalle> detalles)
        {
            var detallesTotales = new List<ProformaDetalle>();
            detalles.ForEach(detalle =>
            {
                if (!string.IsNullOrEmpty(detalle.aritmetica))
                {
                    detallesTotales.Add(detalle);
                }
            });
            return detallesTotales;
        }


        private void calculaDetTot(List<ProformaDetalle> detalles,
            List<ProformaDetalle> detallesTotales)
        {
            //logger.Info("---->> detalles=%o, detallesTotales=%o", detalles, detallesTotales);
            var detallesTotalesNoEvaluados = new List<ProformaDetalle>();
            var detallesTotalesEvaluados = new List<ProformaDetalle>();

            detallesTotales.ForEach(detalleTotal =>
            {
                var evaluado = evaluaDetalleTotal(detalleTotal, detalles);
                if (!evaluado)
                {
                    detallesTotalesNoEvaluados.Add(detalleTotal);
                }
                else
                {
                    detallesTotalesEvaluados.Add(detalleTotal);
                }
            });
            var allDetalles = new List<ProformaDetalle>();
            allDetalles.AddRange(detalles);
            allDetalles.AddRange(detallesTotalesEvaluados);
            detallesTotalesNoEvaluados.ForEach(detalleTotal => { evaluaDetalleTotal(detalleTotal, allDetalles); });
        }

        private bool evaluaDetalleTotal(ProformaDetalle detalleTotal, List<ProformaDetalle> allDetalles)
        {
            var evaluado = false;
            var aritmeticas = new Dictionary<string, string>();
            aritmeticas["enero_monto"] = detalleTotal.aritmetica;
            aritmeticas["febrero_monto"] = detalleTotal.aritmetica;
            aritmeticas["marzo_monto"] = detalleTotal.aritmetica;
            aritmeticas["abril_monto"] = detalleTotal.aritmetica;
            aritmeticas["mayo_monto"] = detalleTotal.aritmetica;
            aritmeticas["junio_monto"] = detalleTotal.aritmetica;
            aritmeticas["julio_monto"] = detalleTotal.aritmetica;
            aritmeticas["agosto_monto"] = detalleTotal.aritmetica;
            aritmeticas["septiembre_monto"] = detalleTotal.aritmetica;
            aritmeticas["octubre_monto"] = detalleTotal.aritmetica;
            aritmeticas["noviembre_monto"] = detalleTotal.aritmetica;
            aritmeticas["diciembre_monto"] = detalleTotal.aritmetica;
            aritmeticas["ejercicio"] = detalleTotal.aritmetica;
            aritmeticas["acumulado"] = detalleTotal.aritmetica;
            aritmeticas["anios_posteriores"] = detalleTotal.aritmetica;
            aritmeticas["total"] = detalleTotal.aritmetica;
            NumberFormatInfo nfi = new NumberFormatInfo();
            List<String> keys = new List<string>();
            foreach (var key in aritmeticas.Keys)
            {
                keys.Add(key);
            }
            allDetalles.ForEach(detalle =>
            {
                var detalleClave = detalle.clave_rubro;
                if (detalleTotal.aritmetica.Contains(detalleClave))
                {
                    foreach (var key in keys)
                    {
                        aritmeticas[key] = aritmeticas[key].Replace(detalleClave,
                            ((Double) detalle[key + "_resultado"]).ToString(nfi));
                    }
                }
            });
            DataTable dt = new DataTable();
            foreach (var key in keys)
            {
                try
                {
                    detalleTotal[key + "_resultado"] = ToDouble(dt.Compute(aritmeticas[key], ""));
                    evaluado = true;
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error de evaluacion de la expresion");
                    evaluado = false;
                    break;
                }
            }

            return evaluado;
        }

        public static void roundMontosInDetalles(List<ProformaDetalle> detalles)
        {
            roundMontosInDetalles(detalles, new[]
            {
                "total_resultado", "anios_posteriores_resultado", "acumulado_resultado",
                "ejercicio_resultado", "enero_monto_resultado",
                "febrero_monto_resultado", "marzo_monto_resultado", "abril_monto_resultado",
                "mayo_monto_resultado", "junio_monto_resultado", "julio_monto_resultado", "agosto_monto_resultado",
                "septiembre_monto_resultado", "octubre_monto_resultado", "noviembre_monto_resultado",
                "diciembre_monto_resultado"
            });
        }
        public  static void roundMontosInDetalles(List<ProformaDetalle> detalles, string[] columnsNames)
        {
            /*detalles.ForEach(detalle =>
            {
                for (var j = 0; j < columnsNames.Length; j++)
                {
                    var colName = columnsNames[j];
                    logger.Trace("'{0}':{1}->{2}",colName,detalle[colName],Math.Round(ToDouble(detalle[colName])));
                    detalle[colName]= Math.Round(ToDouble(detalle[colName]));
                } 
            });*/
            
        }
    }
}