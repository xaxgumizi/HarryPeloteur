using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesWebAzure
{
    class PersonneDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Pv { get; set; }
        public float Force { get; set; }
        public float Fuite { get; set; }
        public float Dexterite { get; set; }
        public int Xp { get; set; }
        public int Po { get; set; } 
    }
}
