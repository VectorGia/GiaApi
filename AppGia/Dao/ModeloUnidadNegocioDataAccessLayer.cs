using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Util;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class ModeloUnidadNegocioDataAccessLayer
    {
        private QueryExecuter _queryExecuter=new QueryExecuter();

        public int Add(ModeloUnidadNegocio modeloUnidadNegocio)
        {
            
            string ddl =
                "insert into modelo_unidad (id_modelo, id_unidad, activo) " +
                "VALUES (@id_modelo, @id_unidad, true)";
            return _queryExecuter.execute(ddl,
                new NpgsqlParameter("@id_modelo", modeloUnidadNegocio.idModelo),
                new NpgsqlParameter("@id_unidad", modeloUnidadNegocio.idUnidad));
       
        }

        public int delete(ModeloUnidadNegocio modeloUnidadNegocio)
        {
            
            string ddl =
                "update modelo_unidad set activo=false " +
                " where id_modelo=@id_modelo and id_unidad=@id_unidad";
            
            return _queryExecuter.execute(ddl,
                new NpgsqlParameter("@id_modelo", modeloUnidadNegocio.idModelo),
                new NpgsqlParameter("@id_unidad", modeloUnidadNegocio.idUnidad));
        }
        public int deleteAllModelo(Int64 idModelo)
        {
            
            string ddl =
                "update modelo_unidad set activo=false " +
                " where id_modelo=@id_modelo ";
            
            return _queryExecuter.execute(ddl,
                new NpgsqlParameter("@id_modelo", idModelo));
        }
        public List<ModeloUnidadNegocio> findAll()
        {
            DataTable dataTable = 
                _queryExecuter.ExecuteQuery("select id_unidad,id_modelo,activo from modelo_unidad where activo=true ");
            List<ModeloUnidadNegocio> modeloUnidadNegocios=new List<ModeloUnidadNegocio>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();
                modeloUnidadNegocio.idModelo=Convert.ToInt64(rdr["id_modelo"]);
                modeloUnidadNegocio.idUnidad=Convert.ToInt64(rdr["id_unidad"]);
                modeloUnidadNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                modeloUnidadNegocios.Add(modeloUnidadNegocio);
            }
            return modeloUnidadNegocios;
        }
        
        public List<ModeloUnidadNegocio> findByIdModelo(Int64 idModelo)
        {
            DataTable dataTable =

                _queryExecuter.ExecuteQuery("select mu.id_unidad, mu.id_modelo, mu.activo,un.descripcion,mn.nombre"+
            " from modelo_unidad mu join unidad_negocio un on mu.id_unidad = un.id"+
            " join modelo_negocio mn on mu.id_modelo = mn.id"+
            " where mu.activo = true"+
            " and mu.id_modelo =@id_modelo",
                    new NpgsqlParameter("@id_modelo", idModelo));
            List<ModeloUnidadNegocio> modeloUnidadNegocios=new List<ModeloUnidadNegocio>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();
                modeloUnidadNegocio.idModelo=Convert.ToInt64(rdr["id_modelo"]);
                modeloUnidadNegocio.idUnidad=Convert.ToInt64(rdr["id_unidad"]);
                modeloUnidadNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                modeloUnidadNegocio.descripcionUnidad = rdr["descripcion"].ToString();
                modeloUnidadNegocio.descripcionModelo = rdr["nombre"].ToString();
                modeloUnidadNegocios.Add(modeloUnidadNegocio);
            }
            return modeloUnidadNegocios;
        }

        public ModeloUnidadNegocio findByIdModeloAndIdUnidad(Int64 idModelo, Int64 idUnidad)
        {
            DataRow rdr =
                _queryExecuter.ExecuteQueryUniqueresult(
                    "select id_unidad,id_modelo,activo from modelo_unidad where activo=true " +
                    " and id_modelo=@id_modelo " +
                    " and id_unidad=@id_unidad",
                    new NpgsqlParameter("@id_modelo", idModelo),
                    new NpgsqlParameter("@id_unidad", idUnidad));


            ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();
            modeloUnidadNegocio.idModelo = Convert.ToInt64(rdr["id_modelo"]);
            modeloUnidadNegocio.idUnidad = Convert.ToInt64(rdr["id_unidad"]);
            modeloUnidadNegocio.activo = Convert.ToBoolean(rdr["activo"]);


            return modeloUnidadNegocio;
        }

        public List<ModeloUnidadNegocio> findByIdUnidad(Int64 idUnidad)
        {
            DataTable dataTable = 
                _queryExecuter.ExecuteQuery("select id_unidad,id_modelo,activo from modelo_unidad where activo=true " +
                                            " and id_unidad=@id_unidad " ,
                    new NpgsqlParameter("@id_unidad", idUnidad));
            List<ModeloUnidadNegocio> modeloUnidadNegocios=new List<ModeloUnidadNegocio>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();
                modeloUnidadNegocio.idModelo=Convert.ToInt64(rdr["id_modelo"]);
                modeloUnidadNegocio.idUnidad=Convert.ToInt64(rdr["id_unidad"]);
                modeloUnidadNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                modeloUnidadNegocios.Add(modeloUnidadNegocio);
            }
            return modeloUnidadNegocios;
        }

    }
}
