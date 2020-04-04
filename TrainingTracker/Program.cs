/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |              
 *===========================================================================*/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TrainingTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
