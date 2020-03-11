using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AppGia.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static System.Convert;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ProformaExcelHelper
    {
        private static string sheetName="proforma";
        private static int pos_total = 2;
        private static int pos_ejercicio = 4;
        private static int pos_anios_posteriores = 17;
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
        
        
        
        private ProformaDataAccessLayer _proformaDataAccessLayer = new ProformaDataAccessLayer();
        private ProformaDetalleDataAccessLayer _proformaDetalleDataAccessLayer = new ProformaDetalleDataAccessLayer();

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
            List<ProformaDetalle> detalles=new List<ProformaDetalle>();
            using (MemoryStream memStream = new MemoryStream(fileContents))
            {
                ExcelPackage package = new ExcelPackage(memStream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                package.Workbook.Calculate();
                ExcelRange cells = worksheet.Cells;
                
                for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
                {
                    String idInterno = cells[i, pos_idInterno].Value.ToString();
                    if (idInterno!=null&&idInterno.Length > 0)
                    {
                     detalles.Add(transform(cells,i));
                    }
                }
            }

          
            return detalles;
        }

        private ProformaDetalle transform(ExcelRange cells,int posRow)
        {
            ProformaDetalle det = new ProformaDetalle();
            det.nombre_rubro = cells[posRow, 1].Value.ToString();
            cells[posRow, 2].Calculate();
            det.total_resultado = ToDouble(cells[posRow, 2].Value);
            det.acumulado_resultado = ToDouble(cells[posRow, 3].Value);
            det.ejercicio_resultado =ToDouble(cells[posRow, pos_ejercicio].Value);
            
            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + pos_ejercicio;
                if (ponderacion > 0)
                {
                    det[entry.Key] = ToDouble(cells[posRow, posicionCelda].Value);
                }
            }
            det.anios_posteriores_resultado =ToDouble(cells[posRow, pos_anios_posteriores].Value);
            
            det.id_proforma =ToInt64(cells[posRow, pos_id_proforma].Value);
            det.mes_inicio =ToInt32(cells[posRow, pos_mes_inicio].Value);
            det.centro_costo_id =ToInt64(cells[posRow, pos_centro_costo_id].Value);
            det.anio =ToInt32(cells[posRow, pos_anio].Value);
            det.tipo_proforma_id =ToInt64(cells[posRow, pos_tipo_proforma_id].Value);
            det.tipo_captura_id =ToInt64(cells[posRow, pos_tipo_captura_id].Value);
            det.idInterno =cells[posRow, pos_idInterno].Value.ToString();
            det.clave_rubro =cells[posRow, pos_clave_rubro].Value.ToString();
            det.rubro_id =ToInt64(cells[posRow, pos_rubro_id].Value);
            det.tipo =cells[posRow, pos_tipo].Value.ToString();
            det.estilo =cells[posRow, pos_estilo].Value.ToString();
            det.aritmetica =cells[posRow, pos_aritmetica].Value.ToString();
            
            
            return det;
        }

        private List<ProformaDetalle> manageDetalles(List<ProformaDetalle> detallesFromExcel)
        {
           return detallesFromExcel;
            /*ProformaDetalle datosProforma = detallesFromExcel[0];
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

            return detallesProforma;*/
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
            int mesInicio=detalles[0].mes_inicio;
            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                ExcelRange cells = workSheet.Cells;
                makeEncabezado(cells, new[]
                {
                    "Rubro", "  Total  ", "Años Anteriores", "Ejercicio", "Enero", "Febrero",
                    "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre",
                    "Diciembre", "Años Posteriores"
                });

                /*se guarda una relacion de claveRubro->(tipo,posicionY), que permite saber las posiciones reales o proformadas 
                  de undetalles, en el caso de un detalle padre solo tiene real y no proforma
                */
                Dictionary<string,Dictionary<string,int>> paresProformaRealProfor=new Dictionary<string, Dictionary<string, int>>();
                List<int> positionsTotales=new List<int>();
                int position = 2;
                foreach (ProformaDetalle detalle in detalles)
                {
                    int posRow = position++;
                    if (!paresProformaRealProfor.ContainsKey(detalle.clave_rubro))
                    {
                        paresProformaRealProfor.Add(detalle.clave_rubro,new Dictionary<string, int>());
                    }
                    if (detalle.estilo.Equals(ESTILODETHIJO))
                    {
                        renderDetalleHijo(cells, posRow, detalle,mesInicio,  paresProformaRealProfor);    
                    }else if(detalle.estilo.Equals(ESTILODETPADRE))
                    {
                        renderDetallePadre(cells,posRow,detalle,paresProformaRealProfor);
                        positionsTotales.Add(posRow);
                    }
                }
                buildFormulasEjercicio(cells, paresProformaRealProfor);
                buildFormulasAritmetica(cells, positionsTotales, paresProformaRealProfor);
              //  cells.Calculate();
                for (int i = 2; i < workSheet.Dimension.End.Row; i++)
                {
                    cells[i, pos_total].Calculate();
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
               setBordersInworkSheet(workSheet);
                /*for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }*/
                for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                {
                    workSheet.Column(i).Width=20;
                }
                cells.Worksheet.Protection.SetPassword("TXu6Wm.Bt.^M)?Je");
                //workSheet.Protection.IsProtected = false;
                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                throw new ApplicationException("No fue posible exportar a excel");
            }

            return fileContents;
        }
        
        private void renderDetallePadre(ExcelRange cells, int pos, ProformaDetalle det,Dictionary<string,Dictionary<string,int>> paresProformaReal)
        {
            Dictionary<string,int> par=paresProformaReal[det.clave_rubro];
            par.Add(TIPODETPROREAL,pos);
            
            makeCellValue(cells, pos, 1, det.nombre_rubro).Style.Font.Bold=true;
            setCellColor(cells[pos, 1].Style,Color.Black,ColorTranslator.FromHtml("#FAFAD2"));
       

            /*string formula = String.Format("SUM({0}:{1})", cells[pos, pos_ejercicio].Address, cells[pos, 3].Address);
            makeCellFormula(cells, pos, pos_total,  formula).Style.Font.Bold=true;*/
            makeCellValue(cells, pos, pos_total, 0).Style.Font.Bold=true;
            makeCellValue(cells, pos, 3, det.acumulado_resultado).Style.Font.Bold=true;
            makeCellValue(cells, pos, pos_ejercicio, 0).Style.Font.Bold=true;
           
            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + pos_ejercicio;
                if (ponderacion > 0)
                {
                    Object valorCelda = 0;
                    makeCellValue(cells, pos, posicionCelda, valorCelda).Style.Font.Bold=true;
                }
            }

            makeCellValue(cells, pos, pos_anios_posteriores, det.anios_posteriores_resultado).Style.Font.Bold=true;
            renderDatosOcultos(cells,pos,det);
        }
        
        private void renderDetalleHijo(ExcelRange cells, int pos, ProformaDetalle det, int mesInicio, Dictionary<string,Dictionary<string,int>> paresProformaReal)
        {
            Dictionary<string,int> par=paresProformaReal[det.clave_rubro];
            if (det.tipo.Equals(TIPODETPROFORM))
            {
                par.Add(TIPODETPROFORM,pos);
                makeCellValue(cells, pos, 1, det.nombre_rubro + " proform" );
                makeCellValue(cells, pos, pos_total, 0.0);
                makeCellValue(cells, pos, 3, 0.0);
                makeCellValue(cells, pos, pos_ejercicio, 0.0);
            }
            else if (det.tipo.Equals(TIPODETPROREAL))
            {
                par.Add(TIPODETPROREAL,pos);
                makeCellValue(cells, pos, 1, det.nombre_rubro + " real");
                /*string formula = String.Format("SUM({0}:{1})", cells[pos, pos_ejercicio].Address, cells[pos, 3].Address);
                makeCellFormula(cells, pos, pos_total,  formula).Style.Font.Bold=true;*/
                makeCellValue(cells, pos, pos_total, 0.0);
                
                makeCellValue(cells, pos, 3, det.acumulado_resultado);
                makeCellValue(cells, pos, pos_ejercicio, 0.0);
            }
            
            foreach (KeyValuePair<string, Int32> entry in getPonderacionCampos())
            {
                int ponderacion = entry.Value;
                int posicionCelda = ponderacion + pos_ejercicio;
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

            makeCellValue(cells, pos, pos_anios_posteriores, det.anios_posteriores_resultado);
            renderDatosOcultos(cells,pos,det);
        }

        private void buildFormulasAritmetica(ExcelRange cells,List<int> positionsTotales,Dictionary<string,Dictionary<string,int>> paresProformaRealProfor)
        {
            positionsTotales.ForEach(posRow =>
            {
          
                for (int i = 2; i < 18; i++)
                {
                    string aritmetica = cells[posRow, pos_aritmetica].Value.ToString();
                    foreach (var entry in paresProformaRealProfor)
                    {
                        string claveRubro = entry.Key;
                        if (aritmetica.Contains(claveRubro))
                        {
                           
                            int posDetReal = entry.Value[TIPODETPROREAL];
                            string addrReal = cells[posDetReal, i].Address;
                            string replacement = "("+addrReal;
                            if (entry.Value.ContainsKey(TIPODETPROFORM))
                            {
                                int posDetProform = entry.Value[TIPODETPROFORM];
                                string addrProform = cells[posDetProform, i].Address;
                                replacement+="+"+addrProform;
                            }
                            replacement+=")";
                            aritmetica=aritmetica.Replace(claveRubro, replacement);
                        }
                    }
                    string formula = aritmetica;
                    makeCellFormula(cells, posRow, i, formula);
                    cells[posRow, i].Calculate();
                }
            });
        }
        private void buildFormulasEjercicio(ExcelRange cells,
            Dictionary<string, Dictionary<string, int>> paresProformaReal)
        {
            foreach (var entry in paresProformaReal)
            {
                int posDetReal = entry.Value[TIPODETPROREAL];
                if (entry.Value.ContainsKey(TIPODETPROREAL)&&entry.Value.ContainsKey(TIPODETPROFORM))
                {
                    int posDetProform = entry.Value[TIPODETPROFORM];
                    
                    string formula = String.Format("SUM({0}:{1})+SUM({2}:{3})",
                        cells[posDetReal, 5].Address, cells[posDetReal, 16].Address, cells[posDetProform, 5].Address,
                        cells[posDetProform, 16].Address);
                    cells[posDetReal, pos_ejercicio].Formula = formula;
                    cells[posDetReal, pos_ejercicio].Calculate();
                    
                    /*formula = String.Format("SUM({0}:{1})", cells[posDetReal, pos_ejercicio].Address,
                        cells[posDetReal, 3].Address);*/
                    //cells[posDetReal, 3].Address

                    formula=formula+"+"+cells[posDetReal, 3].Address;
                    cells[posDetReal, pos_total].Formula = formula;
                    cells[posDetReal, pos_total].Calculate();
                }
                //makeCellFormula(cells, pos, pos_total,  formula).Style.Font.Bold=true;
                
            }

            cells.Worksheet.Calculate();
        }
        private ExcelRangeBase makeCellValue(ExcelRange excelRange,int posY,int posX,object value)
        {
            ExcelRangeBase excelCell = excelRange[posY, posX];
            ExcelStyle style = excelCell.Style;
            if(value is String)
            {
                excelCell.Value = value.ToString();
            }
            else if (value is Int64 )
            {
                excelCell.Value = ToInt64(value);
            }else if (value is Int32 )
            {
                excelCell.Value = ToInt32(value);
            }else if (value is Boolean )
            {
                excelCell.Value = ToBoolean(value);
            }else
            {
                excelCell.Value = ToDouble(value);
                style.Numberformat.Format = "$ ###,###,###,###,###,##0.00";
            }
            return applyStyleDefault(excelCell);
        }
        private ExcelRangeBase makeCellFormula(ExcelRange excelRange,int posY,int posX,string formula)
        {
            excelRange[posY, posX].Value = 0;
            excelRange[posY, posX].Formula = formula;
            excelRange[posY, posX].Style.Numberformat.Format = "$ ###,###,###,###,###,##0.00";
            //excelRange[posY, posX].Style.Hidden = true;    //Hide the formula
            return applyStyleDefault(excelRange[posY, posX]);
        }
        
        private void makeEncabezado(ExcelRange cells, string[] nombresColumnas)
        {
            for (int i = 0; i < nombresColumnas.Length; i++)
            {
                int posicion = i + 1;
                cells[1, posicion].Value = nombresColumnas[i];
                applyStyleDefault(cells[1, posicion]).Style.Font.Bold = true;
                setCellColor(cells[1, posicion].Style, Color.Azure, Color.Blue);
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
            applyStyleOculto(makeCellValue(cells, posY, pos_tipo, det.tipo==null?"":det.tipo));
            applyStyleOculto(makeCellValue(cells, posY, pos_estilo,  det.estilo));
            applyStyleOculto(makeCellValue(cells, posY, pos_aritmetica,  det.aritmetica==null?"":det.aritmetica));
          

        }

        private ExcelRangeBase applyStyleDefault(ExcelRangeBase excelCell)
        {
            ExcelStyle style = excelCell.Style;
            style.Locked = true;//por defecto todas la columnas bloqueadas a edicion
            style.Font.Size = 11;
            style.Border.Top.Style = ExcelBorderStyle.Hair;
            style.ShrinkToFit=true;
            style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            setCellColor(style, Color.Black, ColorTranslator.FromHtml("#eaeded"));
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
        private void setCellColor(ExcelStyle style,Color fontColor,Color backColor)
        {
            style.Font.Color.SetColor(fontColor);
            style.Fill.PatternType = ExcelFillStyle.Solid;
            style.Fill.BackgroundColor.SetColor(backColor);
        }

        private void setBordersInworkSheet(ExcelWorksheet ws)
        {
            int numRows=ws.Dimension.End.Row;
            setBorderColor(ws.Cells[2, 2,numRows, 2]);
            setBorderColor(ws.Cells[2, 3,numRows, 3]);
            setBorderColor(ws.Cells[2, 4, numRows, 4]);
            setBorderColor(ws.Cells[2, pos_anios_posteriores,numRows, pos_anios_posteriores]);
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