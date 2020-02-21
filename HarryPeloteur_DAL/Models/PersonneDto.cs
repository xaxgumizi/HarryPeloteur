using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class PersonneDTO
    {
        public int Id { get; set; }
        public int SalleActuelle { get; set; }
        public string Nom { get; set; }
        public int Pv { get; set; }
        public int Force { get; set; }
        public int Fuite { get; set; }
        public int Dexterite { get; set; }
        public int Xp { get; set; }
        public int Po { get; set; } 
    }
}
