using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Models
{
    public class CharacterDTO
    {
        public int id { get; set; }
        public int salle_actuelle { get; set; }
        public string nom { get; set; }

        public int pv { get; set; }
        public int force { get; set; }
        public int fuite { get; set; }
        public int dexterite { get; set; }
        public int xp { get; set; }
        public int po { get; set; }

    }
}