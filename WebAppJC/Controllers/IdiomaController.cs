/*
*	<copyright file="IdiomaController.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Controlador responsável pelo acesso e manipulação de dados de Idiomas</description>
**/

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibJC.Consts;
using MyLibJC.Hub;

namespace WebAppJC.Controllers
{
    [ApiController]
    [Route("WEBAPP/[controller]")]
    public class IdiomaController : ControllerBase
    {
        /// <summary>
        /// obter uma lista de idiomas
        /// </summary>
        /// <returns></returns>
        [HttpGet("Lista"), Authorize(Roles = LibConst.CTpTodos)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RecIdioma[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        public ActionResult GetIdiomaList()
        {
            string myauth = HttpContext.Request.Headers["Authorization"];
            AppCtrl appctrl = new AppCtrl();
            RecSession tk = appctrl.DecodeToken(myauth);
            if (!tk.Valid)
                return new BadRequestObjectResult(new RecMsg(tk.Errormsg, tk.Errorcod));
            
            return new OkObjectResult(appctrl.Idiomas.Lista());
        }
        
        /// <summary>
        /// obter um idioma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"), Authorize(Roles = LibConst.CTpTodos)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RecIdioma), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status404NotFound)]
        public ActionResult GetIdiomaById(int id)
        {
            string myauth = HttpContext.Request.Headers["Authorization"];
            AppCtrl appctrl = new AppCtrl();
            RecSession tk = appctrl.DecodeToken(myauth);
            if (!tk.Valid)
                return new BadRequestObjectResult(new RecMsg(tk.Errormsg, tk.Errorcod));

            try
            {
                return new OkObjectResult(appctrl.Idiomas.GetById(id));
            }
            catch (Exception e)
            {
                return new NotFoundObjectResult(new RecMsg($"Idioma {id} não foi encontrado! ERRO=\"{e.Message}\"", 404));
            }
        }

        /// <summary>
        /// modificar um idioma
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("{id}"), Authorize(Roles = LibConst.CTpTodos)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RecIdioma), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status404NotFound)]
        public ActionResult SetIdiomaById(int id, [FromBody] RecIdioma data)
        {
            string myauth = HttpContext.Request.Headers["Authorization"];
            AppCtrl appctrl = new AppCtrl();
            RecSession tk = appctrl.DecodeToken(myauth);
            if (!tk.Valid)
                return new BadRequestObjectResult(new RecMsg(tk.Errormsg, tk.Errorcod));

            if (data.Descricao.Trim().Length<1)
                return new BadRequestObjectResult(new RecMsg("Tem que ter uma descrição válida!", 400));
            
            try
            {
                if (appctrl.Idiomas.Modificar(data))
                {
                    return new OkObjectResult(data);
                }
                else
                {
                    return new NotFoundObjectResult(new RecMsg($"Idioma {id} não foi encontrado!", 404));
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new RecMsg($"Idioma {id} produziu um ERRO=\"{e.Message}\"", 400));
            }
        }

        /// <summary>
        /// remover um idioma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize(Roles = LibConst.CTpTodos)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RecMsg), StatusCodes.Status404NotFound)]
        public ActionResult DeleteIdiomaById(int id)
        {
            string myauth = HttpContext.Request.Headers["Authorization"];
            AppCtrl appctrl = new AppCtrl();
            RecSession tk = appctrl.DecodeToken(myauth);
            if (!tk.Valid)
                return new BadRequestObjectResult(new RecMsg(tk.Errormsg, tk.Errorcod));

            try
            {
                if (appctrl.Idiomas.Remover(id))
                {
                    return new OkObjectResult(new RecMsg($"Idioma {id} removido com sucesso!", 200));
                }
                else
                {
                    return new NotFoundObjectResult(new RecMsg($"Idioma {id} não foi encontrado!", 404));
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new RecMsg($"Idioma {id} produziu um ERRO=\"{e.Message}\"", 400));
            }
        }

    }
}