using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemoApp
{
    internal class Participant
    {
        #region Champs
        private int _idPart;
		private int _idPlong;
		private int _idCli;
		private string _nameCli;
		private bool _presencePart;
        #endregion

        #region Accesseurs/Mutateurs
        public bool PresencePart
        {
			get { return _presencePart; }
			set { _presencePart = value; }
		}


		public int IdCli
        {
			get { return _idCli; }
			set { _idCli = value; }
		}


		public int IdPlong
		{
			get { return _idPlong; }
			set { _idPlong = value; }
		}


		public int IdPart
		{
			get { return _idPart; }
			set { _idPart = value; }
		}
		public string NameCli
		{
			get { return _nameCli; }
			set { _nameCli = value; } 
		}
        #endregion

        #region Constructeur

        public Participant(int IdParticipant, int IdPlongée ,  int IdClient,string nameCli, int PresenceParticipant)
		{
			_idPart = IdParticipant;
			_idPlong = IdPlongée;
			_idCli = IdClient;
			_nameCli = nameCli;
			_presencePart = PresenceParticipant == 1;
		}

        #endregion

    }
}
