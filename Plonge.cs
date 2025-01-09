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
        private Site _site;
        private string _duréePlonge;

        #endregion

        #region Constructeur

        public Plonge(int IdPlonge, DateTime DatePlonge, Site Site, string DuréePlonge)
        {
            _idPlonge = IdPlonge;
            _datePlonge = DatePlonge;
            _site = Site;
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
        public Site Site
        {
            get { return _site; }
            set { _site = value; }
        }
        public string DuréePlonge
        {
            get { return _duréePlonge; }
            set { _duréePlonge = value; }
        }
        #endregion

    }
}
