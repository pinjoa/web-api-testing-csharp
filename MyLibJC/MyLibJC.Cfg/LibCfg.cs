/*
*	<copyright file="LibCfg.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelas configurações</description>
**/

namespace MyLibJC.Cfg
{
    public class LibCfg
    {
        private string jwtSecret;
        private string corsList;

        public LibCfg()
        {
            JwtSecret = "sdf987asdkjsdbid_,.f,asdcasdc$#_zxQW$%ssw___=0_MNAJKSKJWUUIWUSNSVJA_çº+poioikjJJUJUH";
            // configuração por omissão do frontend em angular...
            CorsList = "http://localhost:4200";
        }

        public LibCfg(string jwtSecret, string corsList)
        {
            JwtSecret = jwtSecret;
            CorsList = corsList;
        }

        public bool JwtSecretValid => JwtSecret.Trim().Length > 0;
        
        public string JwtSecret
        {
            get => jwtSecret;
            set => jwtSecret = value;
        }

        public bool CorsListValid => CorsList.Trim().Length > 0;

        public string CorsList
        {
            get => corsList;
            set => corsList = value;
        }

    }
}