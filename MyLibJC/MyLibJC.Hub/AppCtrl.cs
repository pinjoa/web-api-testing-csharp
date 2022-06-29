/*
*	<copyright file="AppCtrl.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo principal da camada funcional</description>
**/

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyLibJC.Cfg;
using MyLibJC.Consts;
using MyLibJC.DAL;
using MyLibJC.Error;

namespace MyLibJC.Hub
{
    public class AppCtrl
    {
        
        #region reg_Private

        private readonly Lazy<LibCfg> _cfg = new Lazy<LibCfg>(() => new LibCfg());
        private readonly Lazy<IdiomasDao> _idiomas = new Lazy<IdiomasDao>(() => new IdiomasDao());
        private readonly Lazy<UsersDao> _users = new Lazy<UsersDao>(() => new UsersDao());

        private JwtSecurityToken GenerateToken(string role, string username, int uid)
        {
            if (!JwtSecretValid)
            {
                throw new MyException("JwtSecret não está inicializado!");
            }
            
            string key = JwtSecret;
            
            //symetric security key
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //signing credentials
            var signingCredentials = new SigningCredentials(symetricSecurityKey, 
                SecurityAlgorithms.HmacSha256Signature);
            
            var claims = new List<Claim>();
            Guid myUuId = Guid.NewGuid();
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("user", username));
            claims.Add(new Claim("userid", uid.ToString()));
            claims.Add(new Claim("sessionid", myUuId.ToString()));
            return new JwtSecurityToken(
                issuer: "superadmin",
                audience: role,
                expires: DateTime.Now.AddMinutes(LibConst.MinutosValidadeToken),
                signingCredentials: signingCredentials,
                claims: claims
            );
        }
        
        #endregion

        #region reg_Public

        public AppCtrl() { }
        
        public string JwtSecret => _cfg.Value.JwtSecret;
        
        public bool JwtSecretValid => _cfg.Value.JwtSecretValid;
        
        public string CorsList => _cfg.Value.CorsList;
        
        public bool CorsListValid => _cfg.Value.CorsListValid;

        /// <summary>
        /// fornece acesso a operações de registos de idiomas
        /// </summary>
        public IdiomasDao Idiomas => _idiomas.Value;
        
        /// <summary>
        /// fornece acesso a operações de registos de utilizadores
        /// </summary>
        public UsersDao Users => _users.Value;

        /// <summary>
        /// implementa Login e inicializa as informações do utilizador autenticado
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        public bool Login(string name, string pw)
        {
            int id = -1;
            bool resultado = Users.Login(name, pw, ref id);
            if (resultado)
            {
                try
                {
                    Users.Id = id;
                    RecUser u = Users.GetById(id);
                    Users.Name = u.Name;
                    Users.AccessLevel = u.AccessLevel;
                    JwtSecurityToken token = GenerateToken(
                        LibConst.DefaultNiveisAcessoText[u.AccessLevel],
                        u.Name, u.Id);
                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    Users.Token = tokenString;
                }
                catch (Exception e)
                {
                    throw new MyException($"AppCtrl.Login({name})", e);
                }
            }
            return false;
        }

        /// <summary>
        /// implementa Logout (ATENÇÃO: os dados não são persistentes, portanto retorna sempre "true")
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool Logout(RecSession rs)
        {
            return true;
        }
        
        /// <summary>
        /// descodifica o token de sessão 
        /// </summary>
        /// <param name="tkString"></param>
        /// <returns></returns>
        public RecSession DecodeToken(string tkString)
        {
            RecSession resultado = new RecSession();
            resultado.Valid = false;
            resultado.Expired = false;
            if (tkString.Trim().Length>0 && tkString.StartsWith(LibConst.CTKAuth))
            {
                JwtSecurityToken token = new JwtSecurityToken(tkString.Substring(LibConst.CTKAuth.Length));
                Dictionary<string, string> claimVals = token.Claims.ToDictionary(x => x.Type, x => x.Value);
                var currTime = (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                resultado.Role = claimVals[ClaimTypes.Role];
                resultado.User = claimVals["user"];
                resultado.UserId = Convert.ToInt32(claimVals["userid"]);
                resultado.SessionId = claimVals["sessionid"];
                resultado.Expired = (token.Payload.Exp < currTime);
                resultado.Valid = (resultado.User != null && resultado.User.Trim().Length>0 && !resultado.Expired);
            }            
            if (!resultado.Valid)
            {
                //  401 Unauthorized
                resultado.Errorcod = 401;
                resultado.Errormsg = $"Token de sessão inválido! (exp={resultado.Expired}, unauthorized/unauthenticated)";
            }
            
            Users.ResetSession(resultado);
            return resultado;
        }
        
        #endregion

    }
}