﻿using AppGia.Models;
using AppGia.Util.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class DSNConfig
    {
        public DSNConfig()
        {
            //Constructor
        }

        public DSN crearDSN(int id_compania)
        {
            //Obtener los datos de la Tab_Compania para crear el DSN
            ETLDataAccesLayer eTLDataAccesLayer = new ETLDataAccesLayer();
            List<Empresa> lstCia = eTLDataAccesLayer.CadenaConexionETL_lst(id_compania);

            Empresa cia = new Empresa();
            cia.usuario_etl = lstCia[0].usuario_etl;
            cia.contrasenia_etl = lstCia[0].contrasenia_etl;
            cia.host = lstCia[0].host;
            cia.puerto_compania = lstCia[0].puerto_compania;
            cia.bd_name = lstCia[0].bd_name;
            cia.id = lstCia[0].id;

            string ODBC_PATH = string.Empty;
            string driver = string.Empty;
            string DsnNombre = string.Empty;
            string Descri = string.Empty;
            string DireccionDriver = string.Empty;
            bool trustedConnection = false;

            try
            {
                ODBC_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
                driver = "SQL Anywhere 12"; //Nombre del Driver
                DsnNombre =cia.nombre+"_"+cia.id+"_"+cia.host; //nombre con el que se va identificar el DSN
                Descri = "DNS_Sybase" + DsnNombre;
                DireccionDriver = "C:\\Program Files\\SQL Anywhere 12\\Bin64\\dbodbc12.dll";
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

                dsnKey.SetValue("Database", cia.bd_name);
                dsnKey.SetValue("Description", Descri);
                dsnKey.SetValue("Driver", DireccionDriver);
                dsnKey.SetValue("User", cia.usuario_etl);
                dsnKey.SetValue("Host", cia.host+":"+cia.puerto_compania);
                dsnKey.SetValue("Server", cia.host);
                dsnKey.SetValue("Database", cia.bd_name);
                dsnKey.SetValue("username", cia.usuario_etl);
                dsnKey.SetValue("password", cia.contrasenia_etl);
                dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");

                DSN dsn = new DSN();
                dsn.creado = true;
                dsn.nombreDSN = DsnNombre;
                return dsn ;
                //return 1; //se creo
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                DSN dsn = new DSN();
                dsn.creado = false ;
                dsn.nombreDSN = DsnNombre;
                return dsn;
                //return 0; //Nose creo
            }
        }
    }
}
