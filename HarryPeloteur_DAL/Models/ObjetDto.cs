using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class ObjetDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string ProprieteCible { get; set; }
        public Nullable<int> Montant { get; set; }
    }
}
