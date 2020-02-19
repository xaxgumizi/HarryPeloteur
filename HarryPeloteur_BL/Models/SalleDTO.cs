using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Models
{
    public class SalleDTO
    {
        public int id { get; set; }
        public int id_partie { get; set; }
        public int[] coordonnees { get; set; }
        public int id_contenu { get; set; }
        public int type_contenu { get; set; }
        public int[] portes { get; set; }
        public int etat { get; set; }

    }
}