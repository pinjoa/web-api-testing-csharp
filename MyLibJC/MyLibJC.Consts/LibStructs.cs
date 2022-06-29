/*
*	<copyright file="LibConst.cs" company="jclab">Copyright (c) 2022 All Rights Reserved</copyright>
* 	<author>Joao Carlos Pinto</author>
*   <date>6/27/2022</date>
*	<description>Módulo responsável pelas definição das estruturas</description>
**/

using System;

namespace MyLibJC.Consts
{

    [Serializable]
    public struct RecLogin
    {
        private string _name;
        private string _pw;

        public RecLogin(string name, string pw)
        {
            _name = name;
            _pw = pw;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Pw
        {
            get => _pw;
            set => _pw = value;
        }
    }

    [Serializable]
    public struct RecSession
    {
        private string _role;
        private string _user;
        private int _userId;
        private string _sessionId;
        private bool _valid;
        private bool _expired;
        private int _errorcod;
        private string _errormsg;

        public RecSession(string role = null, string user = null, int userId = default, string sessionId = default, bool valid = default, bool expired = default, int errorcod = default, string errormsg = null)
        {
            _role = role;
            _user = user;
            _userId = userId;
            _sessionId = sessionId;
            _valid = valid;
            _expired = expired;
            _errorcod = errorcod;
            _errormsg = errormsg;
        }

        public bool Valid
        {
            get => _valid;
            set => _valid = value;
        }

        public string Role
        {
            get => _role;
            set => _role = value;
        }

        public string User
        {
            get => _user;
            set => _user = value;
        }

        public int UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public bool Expired
        {
            get => _expired;
            set => _expired = value;
        }

        public int Errorcod
        {
            get => _errorcod;
            set => _errorcod = value;
        }

        public string Errormsg
        {
            get => _errormsg;
            set => _errormsg = value;
        }

        public string SessionId
        {
            get => _sessionId;
            set => _sessionId = value;
        }
    }
    
    [Serializable]
    public struct RecMsg
    {
        private string _msg;
        private int _cod;

        public RecMsg(string msg, int cod=0)
        {
            _msg = msg;
            _cod = cod;
        }

        public string Msg => _msg;
        public int Error => _cod;
    }
    
    [Serializable]
    public struct RecIdioma
    {
        private int _id;
        private string descricao;

        public RecIdioma(int id, string descricao)
        {
            _id = id;
            this.descricao = descricao;
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Descricao
        {
            get => descricao;
            set => descricao = value;
        }
    }

    [Serializable]
    public struct RecUser
    {
        private int _id;
        private string _name;
        private string _pw;
        private bool _active;
        private int _accessLevel;

        public RecUser(int id, string name, string pw, bool active, int accessLevel)
        {
            _id = id;
            _name = name;
            _pw = pw;
            _active = active;
            _accessLevel = accessLevel;
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

        public string Pw
        {
            get => _pw;
            set => _pw = value;
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
    }
    
}