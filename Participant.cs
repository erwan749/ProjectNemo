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
		private Plonge _idPlong;
		private Client _idCli;
		private bool _presencePart;
        #endregion

        #region Accesseurs/Mutateurs
        public bool PresencePart
        {
			get { return _presencePart; }
			set { _presencePart = value; }
		}


		public Client IdCli
        {
			get { return _idCli; }
			set { _idCli = value; }
		}


		public Plonge IdPlong
		{
			get { return _idPlong; }
			set { _idPlong = value; }
		}


		public int IdPart
		{
			get { return _idPart; }
			set { _idPart = value; }
		}
        #endregion

        #region Constructeur

        public Participant(int IdParticipant, Plonge IdPlongée, Client IdClient, int PresenceParticipant)
		{
			_idPart = IdParticipant;
			_idPlong = IdPlongée;
			_idCli = IdClient;
			_presencePart = PresenceParticipant == 1;
		}

        #endregion

    }
}
