/*
*	<copyright file="IdiomasDao.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelo acesso e manipulação de dados de Idiomas</description>
**/

using MyLibJC.Consts;
using MyLibJC.DB;

namespace MyLibJC.DAL
{
    public class IdiomasDao
    {
        public RecIdioma[] Lista()
        {
            return DbIdiomas.Idiomas();
        }

        public RecIdioma GetById(int id)
        {
            return DbIdiomas.GetById(id);
        }
        
        public bool Inserir(RecIdioma v)
        {
            return DbIdiomas.Inserir(v);
        }
        
        public bool Modificar(RecIdioma v)
        {
            return DbIdiomas.Modificar(v);
        }

        public bool Remover(int v)
        {
            return DbIdiomas.Remover(v);
        }
    }
}