using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Client
    {
        private int _idCli;
        private string _nomCli;
        private string _preCli;
        private string _niveauCli;
        private DateTime _dateInsCli;

        #region Constructeur
        public Client(int IdCli, string NomCli, string PreCli, string NiveauCli, DateTime DateInsCli)
        {

            _idCli = IdCli;
            _nomCli = NomCli;
            _preCli = PreCli;
            _niveauCli = NiveauCli;
            _dateInsCli = DateInsCli;

        }

        #endregion


        #region Accesseurs/Mutateurs
        public int IdCli
        {
            get { return _idCli; }
            set { _idCli = value; }
        }
        public string NomCli
        {
            get { return _nomCli; }
            set { _nomCli = value; }
        }
        public string PreCli
        {
            get { return _preCli; }
            set { _preCli = value; }
        }
        public string NiveauCli
        {
            get { return _niveauCli; }
            set { _niveauCli = value; }
        }
        public DateTime DateInsCli
        {
            get { return _dateInsCli; }
            set { _dateInsCli = value; }
        }
        
        #endregion

        public override string ToString()
        {
            return _nomCli + _preCli;
        }
    }
}

