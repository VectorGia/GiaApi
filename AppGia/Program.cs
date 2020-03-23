using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Jobs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppGia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExtraccionProcess.ExtraccionContableSchedule("0 0 0 1 * ?");//0 am de cada mes 1 de mes
            ExtraccionProcess.ExtraccionFlujoSchedule(   "0 0 0 ? * MON");//cada lunes a las 0 am
            
            MontosConsolidadosProcess.MontosContableSchedule("0 0 5 1 * ?");//5 am de cada mes 1 de mes
            MontosConsolidadosProcess.MontosFlujoSchedule(   "0 0 5 ? * MON");//cada lunes a las 5 am
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
