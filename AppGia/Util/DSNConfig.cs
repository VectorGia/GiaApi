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

        public DSN crearDSN(int idEmpresa)
        {
            //Obtener los datos de la Tab_Compania para crear el DSN
            //ETLDataAccesLayer eTLDataAccesLayer = new ETLDataAccesLayer();
            ETLBalanzaDataAccessLayer etlBalanza = new ETLBalanzaDataAccessLayer();
            List<Empresa> lstCia = etlBalanza.EmpresaConexionETL_List(idEmpresa);

            if (lstCia.Count <= 0)
            {

                throw new Exception("La Empresa con identificador " + idEmpresa + " no esta registrado para su extracción ");
            }

            Empresa empresa = new Empresa();
            empresa.usuario_etl = lstCia[0].usuario_etl;
            empresa.contrasenia_etl = lstCia[0].contrasenia_etl;
            empresa.host = lstCia[0].host;
            empresa.puerto_compania = lstCia[0].puerto_compania;
            empresa.bd_name = lstCia[0].bd_name;
            empresa.id = lstCia[0].id;
            empresa.nombre = lstCia[0].nombre;

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
                DsnNombre = empresa.id + "_" + empresa.nombre.Substring(0, 15).TrimEnd().Replace(" ", "_") + "_" + empresa.host; //nombre con el que se va identificar el DSN
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

                dsnKey.SetValue("Database", empresa.bd_name);
                dsnKey.SetValue("Description", Descri);
                dsnKey.SetValue("Driver", DireccionDriver);
                dsnKey.SetValue("User", empresa.usuario_etl);
                dsnKey.SetValue("Host", empresa.host + ":" + empresa.puerto_compania);
                dsnKey.SetValue("Server", empresa.host);
                dsnKey.SetValue("Database", empresa.bd_name);
                dsnKey.SetValue("username", empresa.usuario_etl);
                dsnKey.SetValue("password", empresa.contrasenia_etl);
                dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");

                DSN dsn = new DSN();
                dsn.creado = true;
                dsn.nombreDSN = DsnNombre;
                return dsn;
                //return 1; //se creo
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                DSN dsn = new DSN();
                dsn.creado = false;
                dsn.nombreDSN = DsnNombre;
                throw;
                //return dsn;
                //return 0; //Nose creo
            }
        }
    }
}
