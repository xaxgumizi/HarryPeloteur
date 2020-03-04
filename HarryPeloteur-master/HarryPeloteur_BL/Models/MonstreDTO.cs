using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Models
{
    public class MonstreDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Pv { get; set; }
        public int Force { get; set; }
        public int Fuite { get; set; }
        public int Dexterite { get; set; }

        public int Drop_xp { get; set; }
        public int Drop_argent { get; set; }
        public float Proba_drop_argent { get; set; }
    }
}