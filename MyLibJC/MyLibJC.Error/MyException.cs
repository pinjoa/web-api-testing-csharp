/*
*	<copyright file="MyException.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável tratamento de erros personalizados</description>
**/

using System;

namespace MyLibJC.Error
{
    public class MyException : ApplicationException
    {
        private MyException() : base("ERROR: n/d !!!") {}
        public MyException(string txt) : base(txt) { }
        public MyException(string txt, System.Exception e) : base(txt, e) { }
    }
}