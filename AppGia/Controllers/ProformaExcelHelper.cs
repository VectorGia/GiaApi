using System;
using System.Collections.Generic;
using AppGia.Models;
using AppGia.Util;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ProformaExcelHelper
    {
        private static int pos_ejercicio = 4;
        private static int pos_anios_posteriores = 17;
        private static int pos_id_proforma = 18;
        private static int pos_mes_inicio = 19;
        private static int pos_centro_costo_id = 20;
        private static int pos_anio = 21;
        private static int pos_tipo_proforma_id = 22;
        private static int pos_tipo_captura_id = 23;
        private static int pos_idInterno = 24;
        private static int pos_rubro_id = 25;
        private static int pos_clave_rubro = 26;
        private static int pos_tipo = 27;
        private static int pos_estilo = 27;
        
        
        
        private QueryExecuter _queryExecuter = new QueryExecuter();
        private ProformaDataAccessLayer _proformaDataAccessLayer = new ProformaDataAccessLayer();
        private ProformaDetalleDataAccessLayer _proformaDetalleDataAccessLayer = new ProformaDetalleDataAccessLayer();

        public IActionResult export(List<ProformaDetalle> lstGuardaProforma)
        {
            return buildProformaToExcel(lstGuardaProforma);
        }

        public List<ProformaDetalle> import(byte[] fileContents)
        {
            return manageDetalles(extractDetallesFromFile(fileContents));
        }

        private List<ProformaDetalle> extractDetallesFromFile(byte[] fileContents)
        {
            //TODO: leer el excel y obtener los detalles de proforma que los constituyen
            return new List<ProformaDetalle>();
        }

        private List<ProformaDetalle> manageDetalles(List<ProformaDetalle> detallesFromExcel)
        {
            ProformaDetalle datosProforma = detallesFromExcel[0];
            List<ProformaDetalle> detallesProformados = detallesFromExcel.FindAll(detalle =>
            {
                return detalle.tipo.Equals(TIPODETPROFORM);
            });
            List<ProformaDetalle> detallesReales = detallesFromExcel.FindAll(detalle =>
            {
                return detalle.tipo.Equals(TIPODETPROREAL);
            });
            List<ProformaDetalle> detallesProforma = new List<ProformaDetalle>();
            if (datosProforma.id_proforma > 0) //es una proforma guardada
            {
                detallesProforma = _proformaDetalleDataAccessLayer.GetProformaDetalle(datosProforma.id_proforma);
                applyValuesFrom(detallesProformados, detallesProforma, datosProforma.mes_inicio);
            }
            else //no es una proforma guardada
            {
                detallesProforma = _proformaDataAccessLayer.manageBuildProforma(datosProforma.centro_costo_id,
                    datosProforma.anio, datosProforma.tipo_proforma_id, datosProforma.tipo_captura_id);

                string proyeccion = _proformaDataAccessLayer.ObtenerDatosCC(datosProforma.centro_costo_id).proyeccion;
                if (proyeccion.Equals(ProyeccionMetodo))
                {
                    applyValuesFrom(detallesReales, detallesProforma, 0);
                }

                applyValuesFrom(detallesProformados, detallesProforma, datosProforma.mes_inicio);
            }

            return recalculate(detallesProforma);
        }

        private List<ProformaDetalle> recalculate(List<ProformaDetalle> proformaDetalles)
        {
            //TODO: HNA recalcular proforma
            return proformaDetalles;
        }


        private void applyValuesFrom(List<ProformaDetalle> source, List<ProformaDetalle> target, int mesInicio)
        {
            source.ForEach(dProformado =>
            {
                ProformaDetalle dFound = target.Find(it => it.rubro_id == dProformado.rubro_id);
                setDatosProformadosEnDetalle(dProformado, dFound, mesInicio);
            });
        }

        private void setDatosProformadosEnDetalle(ProformaDetalle profomado, ProformaDetalle original, int mesInicio)
        {
            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                if (entry.Value > mesInicio)
                {
                    original[entry.Key] = profomado[entry.Key];
                }
            }
        }
        
        private IActionResult buildProformaToExcel(List<ProformaDetalle> detalles)
        {
            int mesInicio=detalles[0].mes_inicio;
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                ExcelRange cells = workSheet.Cells;
                makeEncabezado(cells, new[]
                {
                    "Rubro", "Total", "Años Anteriores", "Ejercicio", "Enero", "Febrero",
                    "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre",
                    "Diciembre", "Años Posteriores"
                });

                //se guarda una relacion de idInterno->(tipo,posicionY)
                Dictionary<string,Dictionary<string,int>> paresProformaReal=new Dictionary<string, Dictionary<string, int>>();
                int position = 2;
                foreach (ProformaDetalle detalle in detalles)
                {
                    if (detalle.estilo.Equals(ESTILODETHIJO))
                    {
                        renderDetalleHijo(cells, position++, detalle,mesInicio,  paresProformaReal);    
                    }else if(detalle.estilo.Equals(ESTILODETPADRE))
                    {
                        renderDetallePadre(cells,position++,detalle);
                    }
                }

                buildFormulaRowsReal(cells, paresProformaReal);
                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                return new NotFoundResult();
            }

            return new FileContentResult(fileContents,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {FileDownloadName = "Proforma.xlsx"};
        }

        private void buildFormulaRowsReal(ExcelRange cells,
            Dictionary<string, Dictionary<string, int>> paresProformaReal)
        {
            foreach (var entry in paresProformaReal)
            {
                int posDetReal = entry.Value[TIPODETPROREAL];
                int posDetProform = entry.Value[TIPODETPROREAL];
                string formula = String.Format("=SUMA({0}:{1})+SUMA({2}:{3})",
                    cells[posDetReal, 5].Address, cells[posDetReal, 16].Address, cells[posDetProform, 5].Address,
                    cells[posDetProform, 16].Address);
                cells[posDetReal, pos_ejercicio].Formula = formula;
            }
        }
        private void renderDetallePadre(ExcelRange cells, int pos, ProformaDetalle det)
        {
            
            makeCellMonto(cells, pos, 1, det.nombre_rubro).Style.Font.Bold=true;
            makeCellMonto(cells, pos, 2, "=" + cells[pos, pos_ejercicio].Address + "+" + cells[pos, 3].Address).Style.Font.Bold=true;
            makeCellMonto(cells, pos, 3, det.acumulado_resultado).Style.Font.Bold=true;
            makeCellMonto(cells, pos, pos_ejercicio, "=SUMA(" + cells[pos, 5].Address + ":" + cells[pos, 16].Address + ")").Style.Font.Bold=true;
            int posicionEjercio = 4;
            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + posicionEjercio;
                if (ponderacion > 0)
                {
                    Object valorCelda = det[entry.Key];
                    makeCellMonto(cells, pos, posicionCelda, valorCelda).Style.Font.Bold=true;
                }
            }

            makeCellMonto(cells, pos, pos_anios_posteriores, det.anios_posteriores_resultado).Style.Font.Bold=true;
            renderDatosOcultos(cells,pos,det);
        }
        
        private void renderDetalleHijo(ExcelRange cells, int pos, ProformaDetalle det, int mesInicio, Dictionary<string,Dictionary<string,int>> paresProformaReal)
        {
            if (paresProformaReal[det.idInterno] == null)
            {
                paresProformaReal.Add(det.idInterno,new Dictionary<string, int>());
            }
            Dictionary<string,int> par=new Dictionary<string, int>();
            if (det.tipo.Equals(TIPODETPROFORM))
            {
                par.Add(TIPODETPROFORM,pos);
                makeCellMonto(cells, pos, 1, 0);
                makeCellMonto(cells, pos, 2, 0);
                makeCellMonto(cells, pos, 3, 0);
                makeCellMonto(cells, pos, 4, 0);
            }
            else if (det.tipo.Equals(TIPODETPROREAL))
            {
                par.Add(TIPODETPROREAL,pos);
                makeCellMonto(cells, pos, 1, det.nombre_rubro);
                makeCellMonto(cells, pos, 2, "=" + cells[pos, pos_ejercicio].Address + "+" + cells[pos, 3].Address);
                makeCellMonto(cells, pos, 3, det.acumulado_resultado);
                makeCellMonto(cells, pos, pos_ejercicio, "=SUMA(" + cells[pos, 5].Address + ":" + cells[pos, 16].Address + ")");
            }

            int posicionEjercio = 4;
            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + posicionEjercio;
                if (ponderacion > 0)
                {
                    Object valorCelda = det[entry.Key];
                    if (det.tipo.Equals(TIPODETPROFORM))
                    {
                        if (ponderacion <= mesInicio)
                        {
                            makeCellMonto(cells, pos, posicionCelda, 0.0);
                        }
                        else
                        {
                            makeCellMonto(cells, pos, posicionCelda, valorCelda).Style.Locked = false;
                        }
                    }
                    else if (det.tipo.Equals(TIPODETPROREAL))
                    {
                        if (ponderacion > mesInicio)
                        {
                            makeCellMonto(cells, pos, posicionCelda, 0.0);
                        }
                        else
                        {
                            makeCellMonto(cells, pos, posicionCelda, valorCelda);
                        }
                    }
                }
            }

            makeCellMonto(cells, pos, pos_anios_posteriores, det.anios_posteriores_resultado);
            renderDatosOcultos(cells,pos,det);
        }
        

        private ExcelRangeBase makeCellMonto(ExcelRange excelRange,int posY,int posX,object value)
        {
            ExcelRangeBase excelCell = excelRange[posY, posX];
            ExcelStyle style = excelCell.Style;
            if(value is String && value.ToString().StartsWith("="))
            {
                excelCell.Formula = value.ToString();
            }
            else
            {
                excelCell.Value = ToDouble(value);
                style.Numberformat.Format = "$ ###,###,###,###,###,##0.00";
            }
            style.Font.Size = 12;
            style.Border.Top.Style = ExcelBorderStyle.Hair;
            style.ShrinkToFit=true;
            style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            style.Locked = true;
            return excelCell;
        }
        private void makeEncabezado(ExcelRange cells, string[] nombresColumnas)
        {
            for (int i = 0; i < nombresColumnas.Length; i++)
            {
                int posicion = i + 1;
                cells[1, posicion].Value = nombresColumnas[i];
                cells[1, posicion].Style.Font.Size = 12;
                cells[1, posicion].Style.Font.Bold = true;
                cells[1, posicion].Style.Border.Top.Style = ExcelBorderStyle.Hair;
            }
        }

        private void renderDatosOcultos(ExcelRange cells, int pos, ProformaDetalle det)
        {
            makeCellMonto(cells, pos, pos_id_proforma, det.id_proforma).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_mes_inicio, det.mes_inicio).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_centro_costo_id, det.centro_costo_id).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_anio, det.anio).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_tipo_proforma_id, det.tipo_proforma_id).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_tipo_captura_id, det.tipo_captura_id).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_idInterno, det.idInterno).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_clave_rubro, det.clave_rubro).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_rubro_id, det.rubro_id).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_tipo, det.tipo).Style.Hidden = true;
            makeCellMonto(cells, pos, pos_tipo,  det.estilo).Style.Hidden = true;
           
        }
        private static Dictionary<string, Int32> getPonderacionCampos()
        {
            Dictionary<string, Int32> ponderacionCampos = new Dictionary<string, Int32>();
            ponderacionCampos.Add("total_resultado", -1);
            ponderacionCampos.Add("anios_posteriores_resultado", -1);
            ponderacionCampos.Add("ejercicio_resultado", -1);
            ponderacionCampos.Add("acumulado_resultado", -1);
            ponderacionCampos.Add("enero_monto_resultado", 1);
            ponderacionCampos.Add("febrero_monto_resultado", 2);
            ponderacionCampos.Add("marzo_monto_resultado", 3);
            ponderacionCampos.Add("abril_monto_resultado", 4);
            ponderacionCampos.Add("mayo_monto_resultado", 5);
            ponderacionCampos.Add("junio_monto_resultado", 6);
            ponderacionCampos.Add("julio_monto_resultado", 7);
            ponderacionCampos.Add("agosto_monto_resultado", 8);
            ponderacionCampos.Add("septiembre_monto_resultado", 9);
            ponderacionCampos.Add("octubre_monto_resultado", 10);
            ponderacionCampos.Add("noviembre_monto_resultado", 11);
            ponderacionCampos.Add("diciembre_monto_resultado", 12);
            return ponderacionCampos;
        }
    }
}