﻿using System;
 using System.Collections.Generic;
 using System.Text;
 using AppGia.Dao.Etl;
 using AppGia.Models.Etl;
 using AppGia.Util;
 using Microsoft.Win32;
 using NLog;

 namespace WindowsService1.Util
{
    public class DSNConfig
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public DSN crearDSN(Int64 idEmpresa)
        {
            logger.Info("Obteniendo datos empresa");
            List<Empresa> lstCia = new EmpresaConexionDataAccessLayer().EmpresaConexionETL_List(idEmpresa);
            Empresa empresa = lstCia[0];
            //se deshabilita el cifrado de contrasenia
            //empresa.contrasenia_etl = Utilerias.DecryptStringFromBytes(empresa.contra_bytes, empresa.llave, empresa.apuntador);
            logger.Info("Contrasenia descifada '{0}'",empresa.contrasenia_etl);

            bool trustedConnection = false;

            try
            {
                string ODBC_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
                string driver = "SQL Anywhere 12"; 
                string DsnNombre = empresa.id + "_" + empresa.nombre.Substring(0, 5).TrimEnd().Replace(" ", "_") + "_" + empresa.host; 
                string Descri = "DNS_Sybase" + DsnNombre;
                string DireccionDriver = "C:\\Program Files\\SQL Anywhere 12\\Bin64\\dbodbc12.dll";
                var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBC_PATH + "ODBC Data Sources");


                if (datasourcesKey == null)
                {
                    throw new Exception("La clave de registro ODBC no existe");
                }
                //// Se crea el DSN en datasourcesKey aunque ya exista 
                datasourcesKey.SetValue(DsnNombre, driver);
                //// Borrado de DSN para Actualizar  datos en base de datos 
                datasourcesKey.DeleteValue(DsnNombre);
                /// Se crea DSN con datos actuales 
                datasourcesKey.SetValue(DsnNombre, driver);
                
                var dsnKey = Registry.LocalMachine.CreateSubKey(ODBC_PATH + DsnNombre);

                if (dsnKey == null)
                {
                    throw new Exception("No se creó la clave de registro ODBC para DSN");
                }

                dsnKey.SetValue("Database", empresa.bd_name);
                dsnKey.SetValue("Description", Descri);
                dsnKey.SetValue("Driver", DireccionDriver);
                dsnKey.SetValue("User", empresa.usuario_etl);
                dsnKey.SetValue("Host", empresa.host + ":" + empresa.puerto_compania);
                //dsnKey.SetValue("Server", empresa.host);
                dsnKey.SetValue("Server", "");
                dsnKey.SetValue("Database", empresa.bd_name);
                dsnKey.SetValue("Username", empresa.usuario_etl);
                dsnKey.SetValue("Password", empresa.contrasenia_etl);
                dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");

                StringBuilder sb=new StringBuilder("{");
                foreach (var key in dsnKey.GetValueNames())
                {
                    sb.Append(key).Append(":'").Append(dsnKey.GetValue(key)).Append("',");
                }
                logger.Info("%%%% dsnValues='{0}'",sb.Append("}").ToString().Replace(",}","}"));
                
                

                DSN dsn = new DSN();
                dsn.creado = true;
                dsn.nombreDSN = DsnNombre;
                logger.Info("DSN creado '{0}'",dsn.nombreDSN);
                return dsn;
            }
            catch (Exception ex)
            {
                logger.Error(ex,"Error en creacion de DSN");
                throw;
            }
        }
    }
    
}