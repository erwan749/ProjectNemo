using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Plonge
    {
        #region Champs

        private int _idPlonge;
        private DateTime _datePlonge;
        private string _nameSite;
        private string _duréePlonge;

        #endregion

        #region Constructeur

        public Plonge(int IdPlonge, DateTime DatePlonge, string Site, string DuréePlonge)
        {
            _idPlonge = IdPlonge;
            _datePlonge = DatePlonge;
            _nameSite = Site;
            _duréePlonge = DuréePlonge;
        }

        #endregion

        #region Accesseurs/Mutateurs

        public int IdPlonge
        {
            get { return _idPlonge; }
            set { _idPlonge = value; }
        }
        public DateTime DatePlonge
        {
            get { return _datePlonge; }
            set { _datePlonge = value; }
        }
        public string Site
        {
            get { return _nameSite; }
            set { _nameSite = value; }
        }
        public string DuréePlonge
        {
            get { return _duréePlonge; }
            set { _duréePlonge = value; }
        }
        #endregion

    }
}
