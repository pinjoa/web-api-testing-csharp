/*
*	<copyright file="AuthController.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Controlador responsável pelo acesso ao API</description>
**/

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibJC.Consts;
using MyLibJC.Hub;

namespace WebAppJC.Controllers
{
    [ApiController]
    [Route("WEBAPP/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// endpoint para autenticação no API
        /// </summary>
        /// <param name="data"></param>
        /// <returns>devolve as informações de identificação do utilizador incluindo o token</returns>
        /// <remarks>
        /// dados para utilizador básico:
        /// POST {"name":"default","pw":"testing"}
        /// dados para utilizador administrador:
        /// POST {"name":"superuser","pw":"superbock"}
        /// </remarks>
        [HttpPost("Login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status404NotFound)]
        public ActionResult LoginUser([FromBody] RecLogin data)
        {
            if (data.Name.Trim().Length<1 || data.Pw.Trim().Length<1)
                return new BadRequestObjectResult(new RecMsg("ERRO: sem dados para login!", 400));
            
            AppCtrl appctrl = new AppCtrl();
            if (!appctrl.Login(data.Name, data.Pw))
            {
                return new NotFoundObjectResult(
                    new RecMsg("ERRO: utilizador não está ativo, não existe ou password incorreta!", 404));
            }

            try
            {
                HttpContext.Response.Headers.Add("Authorization", LibConst.CTKAuth + appctrl.Users.Token);
                Dictionary<string, string> resultado = new Dictionary<string, string>();
                resultado.Add("Authorization", LibConst.CTKAuth + appctrl.Users.Token);
                resultado.Add("userid", appctrl.Users.Id.ToString());
                resultado.Add("user", appctrl.Users.Name);
                resultado.Add("access", appctrl.Users.AccessLevel.ToString());
                resultado.Add("cod", "200");
                return new OkObjectResult(resultado);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new RecMsg($"Login produziu um ERRO:\"{e.Message}\"", 400));
            }
        }
        
        /// <summary>
        /// endpoint para terminar a sessão de utilizador
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ATENÇÃO: os dados deste projeto não são persistentes, portanto esta operação não afeta o funcionamento...
        /// </remarks>
        [HttpGet("Logout"), Authorize(Roles = LibConst.CTpTodos)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        public ActionResult LogoutUser()
        {
            string myauth = HttpContext.Request.Headers["Authorization"];
            AppCtrl appctrl = new AppCtrl();
            RecSession tk = appctrl.DecodeToken(myauth);
            if (!tk.Valid)
                return new BadRequestObjectResult(new RecMsg(tk.Errormsg, tk.Errorcod));
            
            if (appctrl.Logout(tk))
                return new OkObjectResult(new RecMsg($"O utilizador [{tk.User}|{tk.UserId}|{tk.Role}] terminou a sessão!", 200));
            else
                return new BadRequestObjectResult(new RecMsg($"O utilizador [{tk.User}|{tk.UserId}|{tk.Role}] não terminou a sessão!", 400));
        }
        
    }
}