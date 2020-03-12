using System.Collections.Generic;
using AppGia.Models;

namespace AppGia.Dao
{
    public class TestProformaDataAccessLayer
    {
        public TestProformaDataAccessLayer()
        {
            //constructor
        }

        public List<ProformaDetalle> GetAllUsuarios()
        {
            try
            {
                List<ProformaDetalle> lstproformadetalle = new List<ProformaDetalle>();

                for (int i = 0; i < 5; i++)
                {
                    ProformaDetalle proformadetalle = new ProformaDetalle();
                    proformadetalle.id = 1;
                    proformadetalle.id_proforma = 1;
                    proformadetalle.enero_monto_resultado = 100;
                    proformadetalle.febrero_monto_resultado = 200;
                    proformadetalle.marzo_monto_resultado = 300;
                    proformadetalle.abril_monto_resultado = 400;
                    proformadetalle.mayo_monto_resultado = 500;
                    proformadetalle.junio_monto_resultado = 600;
                    proformadetalle.julio_monto_resultado = 700;
                    proformadetalle.agosto_monto_resultado = 900;
                    proformadetalle.septiembre_monto_resultado = 100;
                    proformadetalle.octubre_monto_resultado = 200;
                    proformadetalle.noviembre_monto_resultado = 300;
                    proformadetalle.diciembre_monto_resultado = 400;
                    proformadetalle.acumulado_resultado = 500;
                    proformadetalle.valor_tipo_cambio_resultado = 600;
                    proformadetalle.rubro_id = 1;
                    proformadetalle.activo = true;
                    proformadetalle.ejercicio_resultado = 1000;
                    proformadetalle.total_resultado = 2000;

                    lstproformadetalle.Add(proformadetalle);
                }
                return lstproformadetalle;
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }
    }
}
