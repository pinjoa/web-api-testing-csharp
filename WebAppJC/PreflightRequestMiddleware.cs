/*
*	<copyright file="PreflightRequestMiddleware.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pela configuração "pre-flight" necessário para o CORS</description>
**/

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyLibJC.Hub;

namespace WebAppJC
{
    public class PreflightRequestMiddleware
    {
        private readonly RequestDelegate _next;
        
        public PreflightRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public Task Invoke(HttpContext context)
        {
            return BeginInvoke(context);
        }
        
        private Task BeginInvoke(HttpContext context)
        {
            AppCtrl appctrl = new AppCtrl();
            if (appctrl.CorsListValid) 
                context.Response.Headers.Add("Access-Control-Allow-Origin", appctrl.CorsList.Split(","));
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization, ActualUserOrImpersonatedUserSamAccount, IsImpersonatedUser");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            if (context.Request.Method == HttpMethod.Options.Method)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return context.Response.WriteAsync("OK");
            }
            return _next.Invoke(context);
        }
    }

    public static class PreflightRequestExtensions
    {
        public static IApplicationBuilder UsePreflightRequestHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PreflightRequestMiddleware>();
        }
    }

}