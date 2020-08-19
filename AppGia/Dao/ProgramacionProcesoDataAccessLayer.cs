using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Util;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class ProgramacionProcesoDataAccessLayer
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();

        public List<ProgramacionProceso> GetAll()
        {
            string cadena = "select clave,descripcion,cron_expresion,id_usuario from programacion_proceso";
            List<ProgramacionProceso> programacionProcesos = new List<ProgramacionProceso>();
            {
                DataTable dataTable = _queryExecuter.ExecuteQuery(cadena);
                foreach (DataRow row in dataTable.Rows)
                {
                    programacionProcesos.Add(transform(row));
                }
            }
            return programacionProcesos;
        }

        private ProgramacionProceso transform(DataRow dataRow)
        {
            ProgramacionProceso programacionProceso = new ProgramacionProceso();
            programacionProceso.clave = dataRow["clave"].ToString().Trim();
            programacionProceso.descripcion = dataRow["descripcion"].ToString().Trim();
            programacionProceso.cronExpresion = dataRow["cron_expresion"].ToString().Trim();
            programacionProceso.idusuario = Convert.ToInt64(dataRow["id_usuario"]);
            return programacionProceso;
        }

        public ProgramacionProceso GetByClave(string clave)
        {
            DataRow dataRow = _queryExecuter.ExecuteQueryUniqueresult(
                "select clave,descripcion,cron_expresion,id_usuario from programacion_proceso where clave=@clave",
                new NpgsqlParameter("@clave", clave));
            return dataRow != null ? transform(dataRow) : null;
        }

        public int Add(ProgramacionProceso programacionProceso)
        {
            string ddl =
                "INSERT INTO " +
                " programacion_proceso (clave, descripcion, cron_expresion, id_usuario) " +
                " VALUES " +
                " (@clave, @descripcion, @cron_expresion, @id_usuario)";
            return _queryExecuter.execute(ddl,
                new NpgsqlParameter("@clave", programacionProceso.clave),
                new NpgsqlParameter("@descripcion", programacionProceso.descripcion),
                new NpgsqlParameter("@cron_expresion", programacionProceso.clave),
                new NpgsqlParameter("@id_usuario", programacionProceso.descripcion));
        }

        public int Update(ProgramacionProceso programacionProceso)
        {
            string ddl =
                "update programacion_proceso " +
                " set cron_expresion=@cron_expresion," +
                " id_usuario=@id_usuario " +
                " where clave=@clave ";

            return _queryExecuter.execute(ddl,
                new NpgsqlParameter("@clave", programacionProceso.clave),
                new NpgsqlParameter("@cron_expresion", programacionProceso.cronExpresion),
                new NpgsqlParameter("@id_usuario", programacionProceso.idusuario));
        }

        public int Delete(string clave)
        {
            string ddl =
                "delete from programacion_proceso where clave=@clave ";

            return _queryExecuter.execute(ddl,
                new NpgsqlParameter("@clave", clave));
        }

        public void manageProgramacionProceso(ProgramacionProceso pp)
        {
            if (GetByClave(pp.clave) == null)
            {
                Add(new ProgramacionProceso(pp.clave, pp.descripcion,
                    pp.cronExpresion, pp.idusuario));
            }
            else
            {
                Update(new ProgramacionProceso(pp.clave, pp.descripcion,
                    pp.cronExpresion, pp.idusuario));
            }
        }
    }
}