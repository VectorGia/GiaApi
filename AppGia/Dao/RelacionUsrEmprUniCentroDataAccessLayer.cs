using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;

namespace AppGia.Dao
{
    public class RelacionUsrEmprUniCentroDataAccessLayer
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();


        public List<RelacionUsrEmprUniCentro> findAll()
        {
            List<RelacionUsrEmprUniCentro> result = new List<RelacionUsrEmprUniCentro>();
            DataTable dataTable = _queryExecuter.ExecuteQuery(
                "select rel.*,usr.user_name,emp.nombre as empresa,un.descripcion as unidad,cc.nombre as centro" +
                " from relacion_usr_emp_uni_cc rel join tab_usuario usr on usr.id=rel.id_usuario" +
                " join empresa emp on emp.id=rel.id_empresa" +
                " join unidad_negocio un on un.id=rel.id_unidad" +
                " join centro_costo cc on cc.id=rel.id_centrocosto" +
                " where rel.activo=true");
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(transform(row));
            }

            return result;
        }

        public RelacionUsrEmprUniCentro findById(Int32 id)
        {
            DataRow row = _queryExecuter.ExecuteQueryUniqueresult("select * from relacion_usr_emp_uni_cc where id=@id",
                new NpgsqlParameter("@id", id));
            return transform(row);
        }

        public Int32 add(RelacionUsrEmprUniCentro relacionUsrEmprUniCentro)
        {
            return _queryExecuter.execute(
                "insert into relacion_usr_emp_uni_cc " +
                " (activo, id_usuario, id_empresa, id_unidad, id_centrocosto) " +
                " VALUES " +
                " (true,@id_usuario,@id_empresa,@id_unidad,@id_centrocosto)",
                new NpgsqlParameter("@id_usuario", relacionUsrEmprUniCentro.id_usuario),
                new NpgsqlParameter("@id_empresa", relacionUsrEmprUniCentro.id_empresa),
                new NpgsqlParameter("@id_unidad", relacionUsrEmprUniCentro.id_unidad),
                new NpgsqlParameter("@id_centrocosto", relacionUsrEmprUniCentro.id_centrocosto));
        }

        public int update(RelacionUsrEmprUniCentro relacionUsrEmprUniCentro)
        {
            return _queryExecuter.execute(
                "update relacion_usr_emp_uni_cc " +
                "set id_usuario=@id_usuario, " +
                "id_empresa=@id_empresa, " +
                "id_unidad=@id_unidad, " +
                "id_centrocosto=@id_centrocosto " +
                "where id = @id",
                new NpgsqlParameter("@id", relacionUsrEmprUniCentro.id),
                new NpgsqlParameter("@id_usuario", relacionUsrEmprUniCentro.id_usuario),
                new NpgsqlParameter("@id_empresa", relacionUsrEmprUniCentro.id_empresa),
                new NpgsqlParameter("@id_unidad", relacionUsrEmprUniCentro.id_unidad),
                new NpgsqlParameter("@id_centrocosto", relacionUsrEmprUniCentro.id_centrocosto));
        }

        public int delete(Int32 id)
        {
            return _queryExecuter.execute(
                "update relacion_usr_emp_uni_cc " +
                "set activo=false " +
                "where id = @id",
                new NpgsqlParameter("@id", id));
        }

        private RelacionUsrEmprUniCentro transform(DataRow dataRow)
        {
            RelacionUsrEmprUniCentro relacionUsrEmprUniCentro = new RelacionUsrEmprUniCentro();
            relacionUsrEmprUniCentro.id = Convert.ToInt32(dataRow["id"]);
            relacionUsrEmprUniCentro.activo = Convert.ToBoolean(dataRow["activo"]);
            relacionUsrEmprUniCentro.id_usuario = Convert.ToInt32(dataRow["id_usuario"]);
            relacionUsrEmprUniCentro.id_empresa = Convert.ToInt64(dataRow["id_empresa"]);
            relacionUsrEmprUniCentro.id_unidad = Convert.ToInt64(dataRow["id_unidad"]);
            relacionUsrEmprUniCentro.id_centrocosto = Convert.ToInt64(dataRow["id_centrocosto"]);

            relacionUsrEmprUniCentro.user_name = Convert.ToString(dataRow["user_name"]);
            relacionUsrEmprUniCentro.empresa = Convert.ToString(dataRow["empresa"]);
            relacionUsrEmprUniCentro.unidad = Convert.ToString(dataRow["unidad"]);
            relacionUsrEmprUniCentro.centro = Convert.ToString(dataRow["centro"]);

            return relacionUsrEmprUniCentro;
        }
    }
}