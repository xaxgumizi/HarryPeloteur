using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class SalleDTO
    {
        public int Id { get; set; }
        public int[] Coordonnees { get; set; }
        public Nullable<int> IdContenu { get; set; }
        public Nullable<int> TypeContenu { get; set; }
        public int[] Portes { get; set; }
        public Nullable<int> Etat { get; set; }
        public Nullable<int> IdPartie { get; set; }

        public virtual PartieDTO PartieDto { get; set; }
    }
}
