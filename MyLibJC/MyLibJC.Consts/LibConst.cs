/*
*	<copyright file="LibConst.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelas configurações estáticas</description>
**/

namespace MyLibJC.Consts
{
    public class LibConst
    {
        public const string CTpNormal = "normal";
        public const string CTpSuperAdmin = "superadmin";
        public const string CTpTodos = CTpNormal+","+CTpSuperAdmin;
        public static readonly string[] DefaultNiveisAcessoText = CTpTodos.Split(",");
        public const int IdNormal = 0;
        public const int IdSuperAdmin = 1;

        public const string CTKAuth = "Bearer "; // não retirar o espaço
        public const string MyCorsPolicy = "_myCorsPolicy";

        public const int MinutosValidadeToken = 20;
    }
}