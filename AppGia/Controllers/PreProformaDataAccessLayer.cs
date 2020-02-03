using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Conexion;
using AppGia.Util;
using System.Data;
using AppGia.Models;

namespace AppGia.Controllers
{
    
    public class PreProformaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public PreProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public int MontosConsolidados()
        {
            Modelos mn = new Modelos();
            GeneraQry qry = new GeneraQry();
            List<EmpresaCC> lstEmpCCProy = new List<EmpresaCC>();
            DataTable dt = new DataTable();
            DataTable dpm = new DataTable();
            DataTable dr = new DataTable();
            dt = mn.Cia_CC();
            Int64 modelo = 0;
            List<Rubros> lstModel = new List<Rubros>();
            String tipo;
            Double cambiol, cambiop;
            DateTime fechaactual = DateTime.Today;
            int cantcol;

            foreach (DataRow r in dt.Rows)
            {
                EmpresaCC EmpCCProy = new EmpresaCC();
                EmpCCProy.id = Convert.ToInt64(r["id"]);
                EmpCCProy.empresa_id = Convert.ToInt64(r["empresa_id"]);
                EmpCCProy.proyecto_id = Convert.ToInt64(r["proyecto_id"]);


                try
                {
                    //Thread.Sleep(timeTo);

                    dpm = mn.Proyecto_Modelo(EmpCCProy.proyecto_id);
                    foreach (DataRow p in dpm.Rows)
                    {
                        modelo = Convert.ToInt64(p["modelo_negocio_id"]);
                        lstModel = mn.lstModeloNeg(modelo);

                        foreach (Rubros rubros in lstModel)
                        {
                            String consulta = qry.getQuerySums(rubros.rangos_cuentas_incluidas, rubros.rango_cuentas_excluidas, EmpCCProy.empresa_id);
                            dr = mn.Rubros(consulta);
                            tipo = mn.Moneda(EmpCCProy.empresa_id);
                            cambiop = mn.CambioPesos();
                            if (tipo != "MX")
                            {
                                cambiol = mn.CambioLocal(EmpCCProy.empresa_id);

                            }
                            else
                            {
                                cambiol = 1;
                            }
                            foreach (DataRow rub in dr.Rows)
                            {
                                MontosConsolidados montos = new MontosConsolidados();
                                montos.activo = true;
                                montos.enero_abono_resultado = ((Convert.ToDouble(rub["eneabonos"]) * cambiol) * cambiop);
                                montos.enero_cargo_resultado = ((Convert.ToDouble(rub["enecargos"]) * cambiol) * cambiop);
                                montos.enero_total_resultado = ((Convert.ToDouble(rub["enetotal"]) * cambiol) * cambiop);
                                montos.febrero_abono_resultado = ((Convert.ToDouble(rub["febabonos"]) * cambiol) * cambiop);
                                montos.febrero_cargo_resultado = ((Convert.ToDouble(rub["febcargos"]) * cambiol) * cambiop);
                                montos.febrero_total_resultado = ((Convert.ToDouble(rub["febtotal"]) * cambiol) * cambiop);
                                montos.marzo_abono_resultado = ((Convert.ToDouble(rub["marabonos"]) * cambiol) * cambiop);
                                montos.marzo_cargo_resultado = ((Convert.ToDouble(rub["marcargos"]) * cambiol) * cambiop);
                                montos.marzo_total_resultado = ((Convert.ToDouble(rub["martotal"]) * cambiol) * cambiop);
                                montos.abril_abono_resultado = ((Convert.ToDouble(rub["abrabonos"]) * cambiol) * cambiop);
                                montos.abril_cargo_resultado = ((Convert.ToDouble(rub["abrcargos"]) * cambiol) * cambiop);
                                montos.abril_total_resultado = ((Convert.ToDouble(rub["abrtotal"]) * cambiol) * cambiop);
                                montos.mayo_abono_resultado = ((Convert.ToDouble(rub["mayabonos"]) * cambiol) * cambiop);
                                montos.mayo_cargo_resultado = ((Convert.ToDouble(rub["maycargos"]) * cambiol) * cambiop);
                                montos.mayo_total_resultado = ((Convert.ToDouble(rub["maytotal"]) * cambiol) * cambiop);
                                montos.junio_abono_resultado = ((Convert.ToDouble(rub["junabonos"]) * cambiol) * cambiop);
                                montos.junio_cargo_resultado = ((Convert.ToDouble(rub["juncargos"]) * cambiol) * cambiop);
                                montos.junio_total_resultado = ((Convert.ToDouble(rub["juntotal"]) * cambiol) * cambiop);
                                montos.julio_abono_resultado = ((Convert.ToDouble(rub["julabonos"]) * cambiol) * cambiop);
                                montos.julio_cargo_resultado = ((Convert.ToDouble(rub["julcargos"]) * cambiol) * cambiop);
                                montos.julio_total_resultado = ((Convert.ToDouble(rub["jultotal"]) * cambiol) * cambiop);
                                montos.agosto_abono_resultado = ((Convert.ToDouble(rub["agoabonos"]) * cambiol) * cambiop);
                                montos.agosto_cargo_resultado = ((Convert.ToDouble(rub["agocargos"]) * cambiol) * cambiop);
                                montos.agosto_total_resultado = ((Convert.ToDouble(rub["agototal"]) * cambiol) * cambiop);
                                montos.septiembre_abono_resultado = ((Convert.ToDouble(rub["sepabonos"]) * cambiol) * cambiop);
                                montos.septiembre_cargo_resultado = ((Convert.ToDouble(rub["sepcargos"]) * cambiol) * cambiop);
                                montos.septiembre_total_resultado = ((Convert.ToDouble(rub["septotal"]) * cambiol) * cambiop);
                                montos.octubre_abono_resultado = ((Convert.ToDouble(rub["octabonos"]) * cambiol) * cambiop);
                                montos.octubre_cargo_resultado = ((Convert.ToDouble(rub["octcargos"]) * cambiol) * cambiop);
                                montos.octubre_total_resultado = ((Convert.ToDouble(rub["octtotal"]) * cambiol) * cambiop);
                                montos.noviembre_abono_resultado = ((Convert.ToDouble(rub["novabonos"]) * cambiol) * cambiop);
                                montos.noviembre_cargo_resultado = ((Convert.ToDouble(rub["novcargos"]) * cambiol) * cambiop);
                                montos.noviembre_total_resultado = ((Convert.ToDouble(rub["novtotal"]) * cambiol) * cambiop);
                                montos.diciembre_abono_resultado = ((Convert.ToDouble(rub["dicabonos"]) * cambiol) * cambiop);
                                montos.diciembre_cargo_resultado = ((Convert.ToDouble(rub["diccargos"]) * cambiol) * cambiop);
                                montos.diciembre_total_resultado = ((Convert.ToDouble(rub["dictotal"]) * cambiol) * cambiop);
                                montos.anio = fechaactual.Year;
                                montos.fecha = fechaactual;
                                montos.mes = fechaactual.Month;
                                montos.valor_tipo_cambio_resultado = cambiop;
                                montos.centro_costo_id = EmpCCProy.id;
                                montos.empresa_id = EmpCCProy.empresa_id;
                                montos.modelo_negocio_id = modelo;
                                montos.proyecto_id = EmpCCProy.proyecto_id;
                                montos.rubro_id = rubros.id;
                                montos.tipo_captura_id = 1;

                                cantcol = mn.insertarMontos(montos);
                            }


                        }
                    }

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw;
                }
            }
        } 
    }
}
