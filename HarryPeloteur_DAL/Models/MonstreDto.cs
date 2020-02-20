using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class MonstreDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Pv { get; set; }
        public float Force { get; set; }
        public float Fuite { get; set; }
        public float Dexterite { get; set; }

        public int DropXp { get; set; }
        public int DropArgent { get; set; }
        public float ProbaDropArgent { get; set; }
    }
}
