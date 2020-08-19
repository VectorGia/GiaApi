using System;
using System.Diagnostics;
using System.IO;
using AppGia.Dao.Etl;
using AppGia.Util;
using Microsoft.Extensions.Configuration;
using NLog;
using static System.Convert;

namespace AppGia.Helpers
{
    public class ETLHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ConfiguracionCorreoDataAccessLayer _configCorreo = new ConfiguracionCorreoDataAccessLayer();
        private QueryExecuter _queryExecuter = new QueryExecuter();


        public void extraeBalanzaAuto()
        {
            logger.Info("inicio de extraeBalanzaAuto");
            String consultaExistenRegs = "select count(1) as numRegs from balanza";
            bool existenRegs = ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (!existenRegs)
            {
                extraeBalanza(-1, -1);
            }
            else
            {
                int anioActual = new DateTime().Year;
                extraeBalanza(anioActual, anioActual);
            }
        }

        public void extraeBalanza(int anioInicio, int anioFin)
        {
            logger.Info("inicio extraeBalanza (anioInicio={0}, anioFin={1}))", anioInicio, anioFin);
            StopWatch sw =
                new StopWatch(String.Format("extraeBalanza (anioInicio={0}, anioFin={1})", anioInicio, anioFin));
            sw.start();

            try
            {
                executeExtraccion(String.Format("{0} {1} {2}", "all", anioInicio, anioFin));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en ejecucion de extraeBalanza");
                _configCorreo.EnviarCorreo(
                    "Estimado Usuario : \n\n  La extracción(balanza) correspondiente  se genero incorrectamente, favor de revisar logs: \n ",
                    "ETL Extracción Balanza");
            }

            sw.stop();
            logger.Info(sw.prettyPrint());
        }

        public void extraeFlujoAuto()
        {
            logger.Info("inicio de extraeFlujoAuto");
            String consultaExistenRegs = "select count(1) as numRegs from semanal";
            bool existenRegs = ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (!existenRegs)
            {
                extraeFlujo(-1, -1, -1);
            }
            else
            {
                int anioActual = new DateTime().Year;
                int mes = new DateTime().Month;
                extraeFlujo(anioActual, anioActual, mes);
            }
        }

        public void extraeFlujo(int anioInicio, int anioFin, int mes)
        {
            logger.Info("extraeFlujo(anioInicio={0}, anioFin={1}, mes={2})", anioInicio, anioFin, mes);
            StopWatch sw = new StopWatch(String.Format("extraeFlujo(anioInicio={0}, anioFin={1}, mes={2})", anioInicio,
                anioFin, mes));
            sw.start();
            try
            {
                executeExtraccion(String.Format("{0} {1} {2} {3}", "all", anioInicio, anioFin, mes));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en ejecucion de extraeBalanza");
                _configCorreo.EnviarCorreo(
                    "Estimado Usuario : \n\n  La extracción(balanza) correspondiente  se genero incorrectamente, favor de revisar logs: \n ",
                    "ETL Extracción Balanza");
            }

            sw.stop();
            logger.Info(sw.prettyPrint());
        }


        private int executeExtraccion(string arguments)
        {
            var configuration = GetConfiguration();
            string extraccionPath = configuration.GetSection("Data").GetSection("extraccionPath").Value;
            string javapath = extraccionPath + "jre\\bin\\java.exe";
            string allArgs = " -jar extraccion-1.0.jar " + arguments;
            string cmd = javapath + allArgs;
            logger.Trace("executing.. '{0}'", cmd);
            var processInfo = new ProcessStartInfo(javapath, allArgs)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = extraccionPath
            };
            Process proc;

            if ((proc = Process.Start(processInfo)) == null)
            {
                throw new InvalidOperationException("No pudo inicializarce el proceso de extraccion");
            }

            using (StreamReader reader = proc.StandardOutput)
            {
                while (!reader.EndOfStream)
                {
                    logger.Info(reader.ReadLine());
                }
            }

            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();
            if (exitCode != 0)
            {
                throw new Exception("El proceso de extraccion termino con error exitCode='" + exitCode + "' ");
            }

            return exitCode;
        }

        private IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}