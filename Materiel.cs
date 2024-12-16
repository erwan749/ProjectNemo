using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Materiel
    {
        #region Champs
        private int _idMat;
        private string _typeMat;
        private double _prixMat;
        private int _qte;
        #endregion

        #region Constructeur
        public Materiel(int id, string typeMat, double prix, int qte)
        {
            _idMat = id;
            _typeMat = typeMat;
            _prixMat = prix;
            _qte = qte;
        }
        #endregion

        #region Acceseur/Mutateur

        public int IdMat
        {
            get { return _idMat; }
            set { _idMat = value; }
        }
        public string TypeMat
        {
            get { return _typeMat; }
            set { _typeMat = value; }
        }
        public double PrixMat
        {
            get { return _prixMat; }
            set { _prixMat = value; }
        }
        public int Qte
        {
            get { return _qte; }
            set { _qte = value; }
        }

        #endregion

        public override string ToString()
        {
            return _idMat.ToString() + _typeMat;
        }
    }
}