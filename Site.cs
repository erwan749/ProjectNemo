using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Site
    {

        #region Champs

        private int _idSite;
        private string _nomSite;
        private int _profondMax;

        #endregion

        #region Constructeur

        public Site(int IdSite, string NomSite, int ProfondMax)
        {
            _idSite = IdSite;
            _nomSite = NomSite;
            _profondMax = ProfondMax;
        }

        #endregion

        #region Accesseurs/Mutateurs
        public int IdSite
        {
            get { return _idSite; }
            set { _idSite = value; }
        }
        public string NomSite
        {
            get { return _nomSite; }
            set { _nomSite = value; }
        }
        public int ProfondMax
        {
            get { return _profondMax; }
            set { _profondMax = value; }
        }
        #endregion
    }
}
