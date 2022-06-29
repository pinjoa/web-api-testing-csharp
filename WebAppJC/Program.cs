/*
*	<copyright file="Program.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo principal do WebAppJC</description>
**/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebAppJC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}