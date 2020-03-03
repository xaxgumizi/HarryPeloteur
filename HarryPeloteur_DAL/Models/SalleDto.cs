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
        public int IdContenu { get; set; }
        public int TypeContenu { get; set; }
        public int[] Portes { get; set; }
        public int Etat { get; set; }
        public int IdPartie { get; set; }

        public virtual PartieDTO PartieDto { get; set; }
    }
}
