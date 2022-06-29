/*
*	<copyright file="DbUsers.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelos dados estáticos, Users</description>
**/

using System;
using System.Collections.Generic;
using MyLibJC.Consts;
using MyLibJC.Error;

namespace MyLibJC.DB
{
    public class DbUsers
    {
        private static List<RecUser> lista;
        
        static DbUsers()
        {
            lista = new List<RecUser>();
            lista.Add(new RecUser(1, "superuser", "superbock", true, 1));
            lista.Add(new RecUser(2, "default", "testing", true, 0));
        }
        
        public static RecUser[] Users()
        {
            return lista.ToArray();
        }

        public static bool Inserir(RecUser v)
        {
            foreach (RecUser r in lista)
            {
                if (r.Id == v.Id)
                {
                    if (String.Compare(r.Name, v.Name, StringComparison.Ordinal) == 0)
                    {
                        throw new MyException("Já existe este registo!");
                    }
                    else
                    {
                        throw new MyException($"Já existe um registo com o id:{v.Id}!");
                    }
                }
            }
            lista.Add(new RecUser(v.Id, v.Name, v.Pw, v.Active, v.AccessLevel));
            return true;
        }

        public static bool Modificar(RecUser v)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                RecUser r = lista[i];
                if (r.Id == v.Id)
                {
                    lista[i] = v;
                    return true;
                }
            }
            return false;
        }

        public static bool Remover(int v)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                RecUser r = lista[i];
                if (r.Id == v)
                {
                    return lista.Remove(r);
                }
            }
            return false;
        }

        public static RecUser GetById(int id)
        {
            foreach (RecUser r in lista)
            {
                if (r.Id == id)
                {
                    return r;
                }
            }
            throw new MyException("Não encontrado!");
        }
        
        public static bool Login(string name, string pw, ref int id)
        {
            foreach (RecUser r in lista)
            {
                if (String.Compare(r.Name, name, StringComparison.Ordinal) == 0 && 
                    String.Compare(r.Pw, pw, StringComparison.Ordinal) == 0)
                {
                    id = r.Id;
                    return true;
                }
            }
            return false;
        }
        
    }
}