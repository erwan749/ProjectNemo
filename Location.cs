using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Location
    {
        #region Champs
        private int _idLoc;
        private string _name;
        private int _idPlong;
        private int _idMat;
        private int _qte;
        #endregion

        #region Constructeur
        public Location(int idLoc, string name, int idPlong, int idMat, int qte)
        {
            _idLoc = idLoc;
            _name = name;
            _idPlong = idPlong;
            _idMat = idMat;
            _qte = qte;
        }
        #endregion

        #region Accesseurs/Mutateurs

        public int IdLoc
        {
            get { return _idLoc; }
            set { _idLoc = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int IdPlong
        {
            get { return _idPlong; }
            set { _idPlong = value; }
        }
        public int IdMat
        {
            get { return _idMat; }
            set { _idMat = value; }
        }
        public int Qte
        {
            get { return _qte; }
            set { _qte = value; } 
        }

        #endregion

        public override string ToString() {
            return _name;
        }

    }
}
