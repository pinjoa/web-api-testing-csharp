/*
*	<copyright file="Startup.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo startup do WebAppJC</description>
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyLibJC.Consts;
using MyLibJC.Hub;

namespace WebAppJC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppCtrl appctrl = new AppCtrl();
            string key = appctrl.JwtSecret;
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //what to validate
                        ValidateIssuer = true, // Validate the server that generates the token
                        ValidateAudience = true, //Validate the recipient of the token is authorized to receive
                        ValidateLifetime = true, //Check if the token is not expired and the signing key of the issuer is valid
                        ValidateIssuerSigningKey = true, //Validate signature of the token
                        //setup validate data
                        ValidIssuer = "superadmin",
                        ValidAudiences = LibConst.DefaultNiveisAcessoText.ToList(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });
            
            services.AddControllers();
            
            if (appctrl.CorsListValid)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(
                        LibConst.MyCorsPolicy,
                        policy =>
                        {
                            policy.WithOrigins(appctrl.CorsList.Split(","))
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        });
                });
            }       
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAppJC", Version = "v1" });
                
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = LibConst.CTKAuth.ToLower().Trim() 
                };
                
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = LibConst.CTKAuth.Trim()
                            }
                        },
                        new string[] {}
                    }
                };

                c.AddSecurityDefinition(LibConst.CTKAuth.Trim(), jwtSecurityScheme);
                c.AddSecurityRequirement(securityRequirement);
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AppCtrl appctrl = new AppCtrl();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAppJC v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            if (appctrl.CorsListValid) app.UseCors(LibConst.MyCorsPolicy);
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}