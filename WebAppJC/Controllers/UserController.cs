/*
*	<copyright file="UserController.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Controlador responsável pelo acesso e manipulação de dados de Users</description>
**/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibJC.Consts;
using MyLibJC.Hub;

namespace WebAppJC.Controllers
{
    [ApiController]
    [Route("WEBAPP/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// fornece a lista de utilizadores sem a password
        /// </summary>
        /// <returns></returns>
        [HttpGet("Lista"), Authorize(Roles = LibConst.CTpSuperAdmin)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RecUser[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        public ActionResult GetUserList()
        {
            string myauth = HttpContext.Request.Headers["Authorization"];
            AppCtrl appctrl = new AppCtrl();
            RecSession tk = appctrl.DecodeToken(myauth);
            if (!tk.Valid)
                return new BadRequestObjectResult(new RecMsg(tk.Errormsg, tk.Errorcod));
            
            RecUser[] lista = appctrl.Users.Lista();
            for (int i = 0; i < lista.Length; i++)
            {
                lista[i].Pw="";
            }
            return new OkObjectResult(lista);
        }
        
        
    }
}