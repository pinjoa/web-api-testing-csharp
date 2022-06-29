/*
*	<copyright file="UsersDao.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelo acesso e manipulação de dados de Users</description>
**/

using MyLibJC.Consts;
using MyLibJC.DB;

namespace MyLibJC.DAL
{
    public class UsersDao
    {
        private bool _active;
        private RecSession _session;
        private string _token;
        private int _id;
        private string _name;
        private int _accessLevel;

        public UsersDao()
        {
            Active = false;
            _session = new RecSession();
            _session.Valid = false;
            Token = "";
            Id = -1;
            Name = "";
            AccessLevel = 0;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public int AccessLevel
        {
            get => _accessLevel;
            set => _accessLevel = value;
        }

        public RecSession Session => _session;

        public string Token
        {
            get => _token;
            set => _token = value;
        }

        public void ResetSession(RecSession r)
        {
            _session = r;
            Active = Session.Valid;
        }

        public RecUser[] Lista()
        {
            return DbUsers.Users();
        }
        
        public bool Inserir(RecUser v)
        {
            return DbUsers.Inserir(v);
        }
        
        public bool Modificar(RecUser v)
        {
            return DbUsers.Modificar(v);
        }

        public bool Remover(int v)
        {
            return DbUsers.Remover(v);
        }

        public RecUser GetById(int id)
        {
            return DbUsers.GetById(id);
        }
        
        public bool Login(string name, string pw, ref int id)
        {
            return DbUsers.Login(name, pw, ref id);
        }
        
    }
}