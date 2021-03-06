﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AppGia.Dao;
using AppGia.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Helpers
{
    public class ProformaExcelHelper
    {
        private static int NUM_MESES = 12;
        private static string sheetName = "proforma";
        private static int pos_nrubro = 1;

        private static int pos_total = 2;
        private static int pos_porctotal = 3;
        private static int pos_aant = 4;
        private static int pos_porcaant = 5;
        private static int pos_ejercicio = 6;
        private static int pos_porcejercicio = 7;
        private static int pos_apost = 20;
        private static int pos_porcapost = 21;
        private static int POS_COL_ENERO = pos_apost-NUM_MESES;
     
        
        
        private static int pos_id_proforma = 118;
        private static int pos_mes_inicio = 119;
        private static int pos_centro_costo_id = 120;
        private static int pos_anio = 121;
        private static int pos_tipo_proforma_id = 122;
        private static int pos_tipo_captura_id = 123;
        private static int pos_idInterno = 124;
        private static int pos_rubro_id = 125;
        private static int pos_clave_rubro = 126;
        private static int pos_tipo = 127;
        private static int pos_estilo = 128;
        private static int pos_aritmetica = 129;
        private static int pos_id_detalle = 130;
        private static int pos_es_total_ingresos = 131;

        private static int posrow_encabezado = 1;
        private static int posrow_inicio_data = 2;


        private ProformaDataAccessLayer _proformaDataAccessLayer = new ProformaDataAccessLayer();
        private ProformaDetalleDataAccessLayer _proformaDetalleDataAccessLayer = new ProformaDetalleDataAccessLayer();
        List<Int32> listPosColsPorc=new List<int>();

        public ProformaExcelHelper()
        {
            listPosColsPorc.Add(pos_porctotal); 
            listPosColsPorc.Add(pos_porcaant);
            listPosColsPorc.Add(pos_porcejercicio);
            listPosColsPorc.Add(pos_porcapost);
        }
        
        
        public byte[] export(List<ProformaDetalle> lstGuardaProforma)
        {
            return buildProformaToExcel(lstGuardaProforma);
        }

        public List<ProformaDetalle> import(byte[] fileContents)
        {
            return manageDetalles(extractDetallesFromFile(fileContents));
        }

        private List<ProformaDetalle> extractDetallesFromFile(byte[] fileContents)
        {
            List<ProformaDetalle> detalles = new List<ProformaDetalle>();
            using (MemoryStream memStream = new MemoryStream(fileContents))
            {
                ExcelPackage package = new ExcelPackage(memStream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                package.Workbook.Calculate();
                ExcelRange cells = worksheet.Cells;

                for (int i = posrow_inicio_data; i <= worksheet.Dimension.End.Row; i++)
                {
                    String idInterno = cells[i, pos_idInterno].Value.ToString();
                    if (idInterno != null && idInterno.Length > 0)
                    {
                        detalles.Add(transform(cells, i));
                    }
                }
            }


            return detalles;
        }

        private ProformaDetalle transform(ExcelRange cells, int posRow)
        {
            ProformaDetalle det = new ProformaDetalle();
            det.nombre_rubro = cells[posRow, pos_nrubro].Value.ToString();
            det.es_total_ingresos = ToBoolean(cells[posRow, pos_es_total_ingresos].Value);
            cells[posRow, pos_total].Calculate();
            det.total_resultado = ToDouble(cells[posRow, pos_total].Value);
            det.acumulado_resultado = ToDouble(cells[posRow, pos_aant].Value);
            det.ejercicio_resultado = ToDouble(cells[posRow, pos_ejercicio].Value);

            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + (POS_COL_ENERO-1);
                if (ponderacion > 0)
                {
                    det[entry.Key] = ToDouble(cells[posRow, posicionCelda].Value);
                }
            }

            det.anios_posteriores_resultado = ToDouble(cells[posRow, pos_apost].Value);

            det.id_proforma = ToInt64(cells[posRow, pos_id_proforma].Value);
            det.mes_inicio = ToInt32(cells[posRow, pos_mes_inicio].Value);
            det.centro_costo_id = ToInt64(cells[posRow, pos_centro_costo_id].Value);
            det.anio = ToInt32(cells[posRow, pos_anio].Value);
            det.tipo_proforma_id = ToInt64(cells[posRow, pos_tipo_proforma_id].Value);
            det.tipo_captura_id = ToInt64(cells[posRow, pos_tipo_captura_id].Value);
            det.idInterno = cells[posRow, pos_idInterno].Value.ToString();
            det.clave_rubro = cells[posRow, pos_clave_rubro].Value.ToString();
            det.rubro_id = ToInt64(cells[posRow, pos_rubro_id].Value);
            det.tipo = cells[posRow, pos_tipo].Value.ToString();
            det.estilo = cells[posRow, pos_estilo].Value.ToString();
            det.aritmetica = cells[posRow, pos_aritmetica].Value.ToString();
            det.id = ToInt64(cells[posRow, pos_id_detalle].Value);
            det.es_total_ingresos = ToBoolean(cells[posRow, pos_es_total_ingresos].Value);


            return det;
        }

        private List<ProformaDetalle> manageDetalles(List<ProformaDetalle> detallesFromExcel)
        {
            //return detallesFromExcel;
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

            return detallesProforma;
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

        private byte[] buildProformaToExcel(List<ProformaDetalle> detalles)
        {
            int mesInicio = detalles[0].mes_inicio;
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                ExcelRange cells = workSheet.Cells;
                makeEncabezado(cells, new[]
                {
                    "Rubro", "  Total  ","%", "Años Anteriores","%", "Ejercicio","%", "Enero", "Febrero",
                    "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre",
                    "Diciembre", "Años Posteriores","%"
                });

             
                
                /*se guarda una relacion de claveRubro->(tipo,posicionY), que permite saber las posiciones reales o proformadas 
                  de undetalles, en el caso de un detalle padre solo tiene real y no proforma
                */
                Dictionary<string, Dictionary<string, int>> paresProformaRealProfor =
                    new Dictionary<string, Dictionary<string, int>>();
                List<int> positionsTotales = new List<int>();
                int position = posrow_inicio_data;
                int posRowTotalIngresos = -1;
                foreach (ProformaDetalle detalle in detalles)
                {
                    int posRow = position++;
                    if (!paresProformaRealProfor.ContainsKey(detalle.clave_rubro))
                    {
                        paresProformaRealProfor.Add(detalle.clave_rubro, new Dictionary<string, int>());
                    }

                    if (detalle.estilo.Equals(ESTILODETHIJO))
                    {
                        renderDetalleHijo(cells, posRow, detalle, mesInicio, paresProformaRealProfor);
                    }
                    else if (detalle.estilo.Contains(ESTILODETPADRE))
                    {
                        renderDetallePadre(cells, posRow, detalle, paresProformaRealProfor);
                        positionsTotales.Add(posRow);
                        if (detalle.estilo.Equals(ESTILODETPADREINGRESOS))
                        {
                            posRowTotalIngresos = posRow;
                        }
                    }
                }

                buildFormulasEjercicio(cells, paresProformaRealProfor);
                buildFormulasAritmetica(cells, positionsTotales, paresProformaRealProfor);
                for (int i = posrow_inicio_data; i < workSheet.Dimension.End.Row; i++)
                {
                    cells[i, pos_total].Calculate();
                }
                buildFormulasPorcentajes(cells, paresProformaRealProfor,posRowTotalIngresos,positionsTotales);
                setBordersInworkSheet(workSheet);

                for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                {
                    if (listPosColsPorc.Contains(i))
                    {
                        workSheet.Column(i).Width = 6; 
                        workSheet.Column(i).Style.Numberformat.Format = "0 %";
                    }
                    else
                    {
                        workSheet.Column(i).Width = 20; 
                    }
                }
                workSheet.View.FreezePanes(1, POS_COL_ENERO-1);
               
                cells.Worksheet.Protection.SetPassword("TXu6Wm.Bt.^M)?Je");
                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                throw new ApplicationException("No fue posible exportar a excel");
            }

            return fileContents;
        }

        private void renderDetallePadre(ExcelRange cells, int pos, ProformaDetalle det,
            Dictionary<string, Dictionary<string, int>> paresProformaReal)
        {
            Dictionary<string, int> par = paresProformaReal[det.clave_rubro];
            par.Add(TIPODETPROREAL, pos);

            makeCellValue(cells, pos, pos_nrubro, det.nombre_rubro);
            makeCellValue(cells, pos, pos_total, 0);
            makeCellValue(cells, pos, pos_porctotal, 0);
            makeCellValue(cells, pos, pos_aant, det.acumulado_resultado);
            makeCellValue(cells, pos, pos_porcaant, 0);
            makeCellValue(cells, pos, pos_ejercicio, 0);
            makeCellValue(cells, pos, pos_porcejercicio, 0);
            makeCellValue(cells, pos, pos_porcapost, 0);
            
            

            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + (POS_COL_ENERO-1);
                if (ponderacion > 0)
                {
                    Object valorCelda = 0;
                    makeCellValue(cells, pos, posicionCelda, valorCelda);
                }
            }

            makeCellValue(cells, pos, pos_apost, det.anios_posteriores_resultado);
            for (int i = pos_nrubro; i <= pos_porcapost; i++)
            {
                cells[pos, i].Style.Font.Bold = true;
                setCellColor(cells[pos, i].Style, Color.Black, ColorTranslator.FromHtml("#adc6ea"));
            }

            renderDatosOcultos(cells, pos, det);
        }

        private void renderDetalleHijo(ExcelRange cells, int pos, ProformaDetalle det, int mesInicio,
            Dictionary<string, Dictionary<string, int>> paresProformaReal)
        {
            Dictionary<string, int> par = paresProformaReal[det.clave_rubro];
            if (det.tipo.Equals(TIPODETPROFORM))
            {
                par.Add(TIPODETPROFORM, pos);
                makeCellValue(cells, pos, pos_nrubro, det.nombre_rubro + " proform");
                makeCellValue(cells, pos, pos_total, 0.0);
                makeCellValue(cells, pos, pos_porctotal, 0.0);
                makeCellValue(cells, pos, pos_aant, 0.0);
                makeCellValue(cells, pos, pos_porcaant, 0.0);
                makeCellValue(cells, pos, pos_ejercicio, 0.0);
                makeCellValue(cells, pos, pos_porcejercicio, 0.0);
                makeCellValue(cells, pos, pos_porcapost, 0.0);
            }
            else if (det.tipo.Equals(TIPODETPROREAL))
            {
                par.Add(TIPODETPROREAL, pos);
                makeCellValue(cells, pos, pos_nrubro, det.nombre_rubro + " real");
                makeCellValue(cells, pos, pos_total, 0.0);
                makeCellValue(cells, pos, pos_porctotal, 0.0);
                makeCellValue(cells, pos, pos_aant, det.acumulado_resultado);
                makeCellValue(cells, pos, pos_porcaant, 0.0);
                makeCellValue(cells, pos, pos_ejercicio, 0.0);
                makeCellValue(cells, pos, pos_porcejercicio, 0.0);
                makeCellValue(cells, pos, pos_porcapost, 0.0);
            }

            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + (POS_COL_ENERO-1);
                if (ponderacion > 0)
                {
                    Object valorCelda = det[entry.Key];
                    if (det.tipo.Equals(TIPODETPROFORM))
                    {
                        if (ponderacion <= mesInicio)
                        {
                            makeCellValue(cells, pos, posicionCelda, 0.0);
                        }
                        else
                        {
                            applyStyleEditable(makeCellValue(cells, pos, posicionCelda, valorCelda));
                        }
                    }
                    else if (det.tipo.Equals(TIPODETPROREAL))
                    {
                        if (ponderacion > mesInicio)
                        {
                            makeCellValue(cells, pos, posicionCelda, 0.0);
                        }
                        else
                        {
                            makeCellValue(cells, pos, posicionCelda, valorCelda);
                        }
                    }
                }
            }

            makeCellValue(cells, pos, pos_apost, det.anios_posteriores_resultado);
            renderDatosOcultos(cells, pos, det);
        }

        private void buildFormulasAritmetica(ExcelRange cells, List<int> positionsTotales,
            Dictionary<string, Dictionary<string, int>> paresProformaRealProfor)
        {
            positionsTotales.ForEach(posRow =>
            {
                for (int i = 2; i < pos_apost; i++)
                {
                    string aritmetica = cells[posRow, pos_aritmetica].Value.ToString();
                    foreach (var entry in paresProformaRealProfor)
                    {
                        string claveRubro = entry.Key;
                        if (aritmetica.Contains(claveRubro))
                        {
                            int posDetReal = entry.Value[TIPODETPROREAL];
                            string addrReal = cells[posDetReal, i].Address;
                            string replacement = "(" + addrReal;
                            if (entry.Value.ContainsKey(TIPODETPROFORM))
                            {
                                int posDetProform = entry.Value[TIPODETPROFORM];
                                string addrProform = cells[posDetProform, i].Address;
                                replacement += "+" + addrProform;
                            }

                            replacement += ")";
                            aritmetica = aritmetica.Replace(claveRubro, replacement);
                        }
                    }

                    string formula = aritmetica;
                    makeCellFormula(cells, posRow, i, formula);
                    cells[posRow, i].Calculate();
                }
            });
        }

        private void buildFormulasEjercicio(ExcelRange cells_,
            Dictionary<string, Dictionary<string, int>> paresProformaReal)
        {
            ExcelRange c = cells_;
            int posColIni = POS_COL_ENERO;
            int posColFin = pos_apost - 1;
            foreach (var entry in paresProformaReal)
            {
                int posReal = entry.Value[TIPODETPROREAL];
                if (entry.Value.ContainsKey(TIPODETPROREAL) && entry.Value.ContainsKey(TIPODETPROFORM))
                {
                    int posProf = entry.Value[TIPODETPROFORM];
                    ApplyFormulaEjercicio(c, posReal, posColIni, posColFin);
                    ApplyFormulaEjercicio(c, posProf, posColIni, posColFin);
                    ApplyFormulaTotal(c, posReal,posColIni,posColFin);
                    ApplyFormulaTotal(c, posProf,posColIni,posColFin);
                }
            }
            c.Worksheet.Calculate();
        }

        private void buildFormulasPorcentajes(ExcelRange cells,
            Dictionary<string, Dictionary<string, int>> paresProformaReal, int posRowTotalIngresos,
            List<int> positionsTotales)
        {
            if (posRowTotalIngresos > 0)
            {
                foreach (var entry in paresProformaReal)
                {
                    int posReal = entry.Value[TIPODETPROREAL];
                    if (entry.Value.ContainsKey(TIPODETPROREAL) && entry.Value.ContainsKey(TIPODETPROFORM))
                    {
                        int posProf = entry.Value[TIPODETPROFORM];

                        listPosColsPorc.ForEach(posPorcentaje =>
                        {
                            ApplyFormulaPorcentaje(cells, posRowTotalIngresos, posReal, posPorcentaje - 1, posPorcentaje);
                            ApplyFormulaPorcentaje(cells, posRowTotalIngresos, posProf, posPorcentaje - 1, posPorcentaje);
                        });
                    }
                }
                positionsTotales.ForEach(posrow =>
                {
                    listPosColsPorc.ForEach(posPorcentaje =>
                    {
                        ApplyFormulaPorcentaje(cells, posRowTotalIngresos, posrow, posPorcentaje - 1, posPorcentaje);
                    });
                });
            }
        }

        private void ApplyFormulaEjercicio(ExcelRange c,int posRow,int posColIni,int posColFin)
        {
            string formula = $"SUM({_A(c[posRow, posColIni])}:{_A(c[posRow, posColFin])})";
            ApplyFormula(c,posRow,pos_ejercicio,formula);
        }
        private void ApplyFormulaTotal(ExcelRange c,int posRow,int posColIni,int posColFin)
        {
            string formula =
                $"SUM({_A(c[posRow, posColIni])}:{_A(c[posRow, posColFin])})+{_A(c[posRow, pos_aant])}+{_A(c[posRow, pos_apost])}";

            ApplyFormula(c,posRow,pos_total,formula);
        }
        private void ApplyFormulaPorcentaje(ExcelRange c,int posRowIngresos,int posRow,int posCol,int posColTarget)
        {
            string formula =
                $"{_A(c[posRow, posCol])}/{_A(c[posRowIngresos, posCol])}";

            ApplyFormula(c,posRow,posColTarget,formula);
            //ExcelRangeBase excelCell = c[posRow, posColTarget];
        }


        private void ApplyFormula(ExcelRange cells, int posRow, int posCol, string formula)
        {
            cells[posRow, posCol].Formula = formula;
            cells[posRow, posCol].Calculate(); 
        }
       
        private String _A(ExcelRange excelRange)
        {
            return excelRange.Address;
        }

        private ExcelRangeBase makeCellValue(ExcelRange excelRange, int posY, int posX, object value)
        {
            ExcelRangeBase excelCell = excelRange[posY, posX];
            ExcelStyle style = excelCell.Style;
            if (value is String)
            {
                excelCell.Value = value.ToString();
            }
            else if (value is Int64)
            {
                excelCell.Value = ToInt64(value);
            }
            else if (value is Int32)
            {
                excelCell.Value = ToInt32(value);
            }
            else if (value is Boolean)
            {
                excelCell.Value = ToBoolean(value);
            }
            else
            {
                excelCell.Value = ToDouble(value);
                style.Numberformat.Format = "$ ###,###,###,###,###,##0.00";
            }

            return applyStyleDefault(excelCell);
        }

        private ExcelRangeBase makeCellFormula(ExcelRange excelRange, int posY, int posX, string formula)
        {
            excelRange[posY, posX].Value = 0;
            excelRange[posY, posX].Formula = formula;
            excelRange[posY, posX].Style.Numberformat.Format = "$ ###,###,###,###,###,##0.00";
            //excelRange[posY, posX].Style.Hidden = true;    //Hide the formula
            //return applyStyleDefault(excelRange[posY, posX]);
            return excelRange[posY, posX];
        }

        private void makeEncabezado(ExcelRange cells, string[] nombresColumnas)
        {
            for (int i = 0; i < nombresColumnas.Length; i++)
            {
                int posicion = i + posrow_encabezado;
                cells[posrow_encabezado, posicion].Value = nombresColumnas[i];
                applyStyleDefault(cells[posrow_encabezado, posicion]).Style.Font.Bold = true;
                setCellColor(cells[posrow_encabezado, posicion].Style, Color.Azure, Color.DarkBlue);
            }
        }

        private void renderDatosOcultos(ExcelRange cells, int posY, ProformaDetalle det)
        {
            applyStyleOculto(makeCellValue(cells, posY, pos_id_proforma, det.id_proforma));
            applyStyleOculto(makeCellValue(cells, posY, pos_mes_inicio, det.mes_inicio));
            applyStyleOculto(makeCellValue(cells, posY, pos_centro_costo_id, det.centro_costo_id));
            applyStyleOculto(makeCellValue(cells, posY, pos_anio, det.anio));
            applyStyleOculto(makeCellValue(cells, posY, pos_tipo_proforma_id, det.tipo_proforma_id));
            applyStyleOculto(makeCellValue(cells, posY, pos_tipo_captura_id, det.tipo_captura_id));
            applyStyleOculto(makeCellValue(cells, posY, pos_idInterno, det.idInterno));
            applyStyleOculto(makeCellValue(cells, posY, pos_clave_rubro, det.clave_rubro));
            applyStyleOculto(makeCellValue(cells, posY, pos_rubro_id, det.rubro_id));
            applyStyleOculto(makeCellValue(cells, posY, pos_tipo, det.tipo == null ? "" : det.tipo));
            applyStyleOculto(makeCellValue(cells, posY, pos_estilo, det.estilo));
            applyStyleOculto(makeCellValue(cells, posY, pos_aritmetica, det.aritmetica == null ? "" : det.aritmetica));
            applyStyleOculto(makeCellValue(cells, posY, pos_id_detalle, det.id));
            applyStyleOculto(makeCellValue(cells, posY, pos_es_total_ingresos, det.es_total_ingresos));
        }

        private ExcelRangeBase applyStyleDefault(ExcelRangeBase excelCell)
        {
            ExcelStyle style = excelCell.Style;
            style.Locked = true; //por defecto todas la columnas bloqueadas a edicion
            style.Font.Size = 11;
            style.Border.Top.Style = ExcelBorderStyle.Hair;
            style.ShrinkToFit = true;
            style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            setCellColor(style, Color.DimGray, ColorTranslator.FromHtml("#eaeded"));
            return excelCell;
        }

        private void applyStyleOculto(ExcelRangeBase excelCell)
        {
            ExcelStyle style = excelCell.Style;
            setCellColor(style, Color.White, Color.White);
        }

        private void applyStyleEditable(ExcelRangeBase excelCell)
        {
            ExcelStyle style = excelCell.Style;
            style.Locked = false;
            setCellColor(style, Color.Black, ColorTranslator.FromHtml("#ffffff"));
        }

        private void setCellColor(ExcelStyle style, Color fontColor, Color backColor)
        {
            style.Font.Color.SetColor(fontColor);
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(backColor);
        }

        private void setBordersInworkSheet(ExcelWorksheet ws)
        {
            int numRows = ws.Dimension.End.Row;
            setBorderColor(ws.Cells[posrow_inicio_data, posrow_inicio_data, numRows, pos_porcapost]);
            
        }

        private void setBorderColor(ExcelRange Rng)
        {
            Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            Rng.Style.Border.Left.Color.SetColor(Color.Black);
            Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            Rng.Style.Border.Right.Color.SetColor(Color.Black);
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