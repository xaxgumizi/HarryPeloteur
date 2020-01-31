using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Models
{
    public class PartieDTO
    {
        public int id { get; set; }

        public int id_personnage { get; set; }

        public int salle_actuelle { get; set; }
        public int difficulte { get; set; }
    }
}