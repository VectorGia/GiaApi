using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;
using NpgsqlTypes;

namespace AppGia.Dao
{
    public class RelacionRolDataAccessLayer
    {
        private QueryExecuter _queryExecuter = new QueryExecuter();


        public IEnumerable<RelacionRol> GetAllRelacionRol()
        {
            string query =
                " select relrol.*,cr.str_nombre_rol as nombrerol" +
                " from tab_relacion_rol relrol join cat_rol cr on relrol.idrol = cr.int_idrol_p" +
                " where relrol.estatus_logico = true" +
                " order by relrol.id, relrol.pantalla, relrol.permiso";

            List<RelacionRol> lstRelacionRol = new List<RelacionRol>();

            foreach (DataRow rdr in _queryExecuter.ExecuteQuery(query).Rows)
            {
                RelacionRol relacionRol = new RelacionRol();
                relacionRol.id = Convert.ToInt32(rdr["id"]);
                relacionRol.idRol = Convert.ToInt32(rdr["idrol"]);
                relacionRol.nombreRol = Convert.ToString(rdr["nombrerol"]);
                relacionRol.estatus = Convert.ToBoolean(rdr["estatus_logico"]);
                relacionRol.fecModif = Convert.ToDateTime(rdr["fec_modif"]);
                relacionRol.permiso = Convert.ToString(rdr["pantalla"]);
                relacionRol.pantalla = Convert.ToString(rdr["permiso"]);
                lstRelacionRol.Add(relacionRol);
            }

            return lstRelacionRol;
        }


        public int delete(Int32 id)
        {
            return _queryExecuter.execute("update tab_relacion_rol set estatus_logico=false where id=@id",
                new NpgsqlParameter("@id", id));
        }

        public int insert(RelacionRol relacionrol)

        {
            String insert = " insert into tab_relacion_rol " +
                            " (idrol, pantalla, permiso, estatus_logico, fec_modif) " +
                            " values " +
                            " (@idrol, @pantalla, @permiso, true, @fec_modif)";
            return _queryExecuter.execute(insert, new NpgsqlParameter("@idrol", relacionrol.idRol),
                new NpgsqlParameter("@pantalla", relacionrol.pantalla),
                new NpgsqlParameter("@permiso", relacionrol.permiso),
                new NpgsqlParameter("@fec_modif", new DateTime()));
        }

        public List<Dictionary<string, string>> getRelacionesByUserName(string username)
        {
            string query =
                "select distinct " +
                " cr.str_nombre_rol  as rol," +
                " relrol.pantalla, " +
                " relrol.permiso" +
                " from tab_usuario usr" +
                " join tab_relacion_usuario relu on relu.int_idusuario_p=usr.id" +
                " join cat_rol cr on relu.int_idrol_p = cr.int_idrol_p" +
                " join tab_relacion_rol relrol on relrol.idrol=cr.int_idrol_p" +
                " where usr.estatus=true" +
                " and cr.bool_estatus_logico_rol=true" +
                " and relu.bool_estatus_logico_relusu=true" +
                " and relrol.estatus_logico=true" +
                " and usr.user_name=@user_name";
            DataTable dataTable = new QueryExecuter().ExecuteQuery(query, new NpgsqlParameter("@user_name", username));
            List<Dictionary<string, string>> relaciones = new List<Dictionary<string, string>>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("pantalla", rdr["pantalla"].ToString());
                dictionary.Add("permiso", rdr["permiso"].ToString());
                dictionary.Add("rol", rdr["rol"].ToString());
                relaciones.Add(dictionary);
            }

            return relaciones;
        }
    }
}