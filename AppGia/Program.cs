using AppGia.Jobs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AppGia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExtraccionProcess.ExtraccionContableSchedule();
            ExtraccionProcess.ExtraccionFlujoSchedule();
            MontosConsolidadosProcess.MontosContableSchedule();
            MontosConsolidadosProcess.MontosFlujoSchedule();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
