using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesWebAzure
{
    public class MonstreDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Pv { get; set; }
        public float Force { get; set; }
        public float Dexterite { get; set; }
        public int Drop_xp { get; set; }
        public int Drop_argent { get; set; }
        public float Proba_drop_argent { get; set; }
    }
}
