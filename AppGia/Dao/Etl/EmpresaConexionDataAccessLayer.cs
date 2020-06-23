﻿using System;
 using System.Collections.Generic;
 using System.Data;
 using System.Data.Odbc;
 using AppGia.Models.Etl;
 using Npgsql;

 namespace AppGia.Dao.Etl
{
    public class EmpresaConexionDataAccessLayer
    {
        Conexion.Conexion conex = new Conexion.Conexion();

        NpgsqlConnection conP = new NpgsqlConnection();
        NpgsqlCommand comP = new NpgsqlCommand();

        OdbcConnection odbcCon;

        public EmpresaConexionDataAccessLayer()
        {
            conP = conex.ConnexionDB();
        }

        /// <summary>
        /// Metodo para obtener los valores de la cadena de conexion de la empresa
        /// </summary>
        /// <param name="compania"></param>
        /// <returns></returns>
        private DataTable EmpresaConexionETL(Int64 idEmpresa)
        {
            string add = "SELECT  id ,"
                         + "  usuario_etl ,"
                         + " nombre ,"
                         + " host ,"
                         + " puerto_compania ,"
                         + " bd_name ,"
                         + " contra_bytes ,"
                         + " llave ,"
                         + " apuntador, contrasenia_etl "
                         + " FROM empresa"
                         + " WHERE  id  = " + idEmpresa;
            try
            {
                conP.Open();
                comP = new NpgsqlCommand(add, conP);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            finally
            {
                conP.Close();
            }
        }

        /// <summary>
            /// Se convierte el DataTable en una Lista Generica
            /// </summary>
            /// <param name="compania"></param>
            /// <returns>Lista del tipo Compania</returns>
            public List<Empresa> EmpresaConexionETL_List(Int64 idEmpresa)
            {
                List<Empresa> lst = new List<Empresa>();
                DataTable dt = new DataTable();
                dt = EmpresaConexionETL(idEmpresa);

                foreach (DataRow r in dt.Rows)
                {
                    Empresa cia = new Empresa();
                    cia.usuario_etl = r["usuario_etl"].ToString();
                    cia.host = r["host"].ToString();
                    cia.puerto_compania = Convert.ToInt32(r["puerto_compania"]);
                    cia.bd_name = r["bd_name"].ToString();
                    cia.id = Convert.ToInt32(r["id"]);
                    cia.nombre = Convert.ToString(r["nombre"]);
                    cia.contra_bytes = r["contra_bytes"] as byte[];
                    cia.llave = r["llave"] as byte[];
                    cia.apuntador = r["apuntador"] as byte[];
                    cia.contrasenia_etl = Convert.ToString(r["contrasenia_etl"]);
                    lst.Add(cia);
                }

                return lst;
            }
        }
    }