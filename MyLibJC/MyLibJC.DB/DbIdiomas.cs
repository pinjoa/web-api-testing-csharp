/*
*	<copyright file="DbIdiomas.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelos dados estáticos, Idiomas</description>
**/

using System;
using System.Collections.Generic;
using MyLibJC.Consts;
using MyLibJC.Error;

namespace MyLibJC.DB
{
    public class DbIdiomas
    {
        private static List<RecIdioma> lista;

        static DbIdiomas()
        {
            lista = new List<RecIdioma>();
            lista.Add(new RecIdioma(1, "Português"));
            lista.Add(new RecIdioma(2, "Espanhol"));
            lista.Add(new RecIdioma(3, "Francês"));
            lista.Add(new RecIdioma(4, "Alemão"));
            lista.Add(new RecIdioma(5, "Inglês"));
        }
        
        public static RecIdioma[] Idiomas()
        {
            return lista.ToArray();
        }

        public static RecIdioma GetById(int id)
        {
            foreach (RecIdioma r in lista)
            {
                if (r.Id == id)
                {
                    return r;
                }
            }
            throw new MyException("Não encontrado!");
        }

        public static bool Inserir(RecIdioma v)
        {
            foreach (RecIdioma r in lista)
            {
                if (r.Id == v.Id)
                {
                    if (String.Compare(r.Descricao, v.Descricao, StringComparison.Ordinal) == 0)
                    {
                        throw new MyException("Já existe este registo!");
                    }
                    else
                    {
                        throw new MyException($"Já existe um registo com o id:{v.Id}!");
                    }
                }
            }
            lista.Add(new RecIdioma(v.Id, v.Descricao));
            return true;
        }

        public static bool Modificar(RecIdioma v)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                RecIdioma r = lista[i];
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
                RecIdioma r = lista[i];
                if (r.Id == v)
                {
                    return lista.Remove(r);
                }
            }
            return false;
        }
        
    }
}