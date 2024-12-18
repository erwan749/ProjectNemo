﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Personnel
    {
        #region Champs

        private int _idPers;
        private string _nomPers;
        private string _prePers;
        private int _idRole;
        private string _nomRole;
        private string _certifRole;

        #endregion

        #region Constructeur

        public Personnel(int IdPers, string NomPers, string PrePres,int IdRole, string NomRole, string CertifRole)
        {

            _idPers = IdPers;
            _nomPers = NomPers;
            _prePers = PrePres;
            _idRole = IdRole;
            _nomRole = NomRole;
            _certifRole = CertifRole;

        }

        #endregion

        #region Accesseurs/Mutateurs
        public int IdRole
        {
            get { return _idRole; }
            set { _idRole = value; }
        }
        public int IdPers
        {
            get { return _idPers; }
            set { _idPers = value; }
        }
        public string NomPers
        {
            get { return _nomPers; }
            set { _nomPers = value; }
        }
        public string PrePres
        {
            get { return _prePers; }
            set { _prePers = value; }
        }
        public string NomRole
        {
            get { return _nomRole; }
            set { _nomRole = value; }
        }
        public string CertifPers
        {
            get { return _certifRole; }
            set { _certifRole = value; }
        }

        #endregion

        public override string ToString()
        {
            return _nomPers + _prePers;
        }
    }
}