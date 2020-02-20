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
        public Nullable<int> coordonneeX { get; set; }
        public Nullable<int> coordonneeY { get; set; }
        public Nullable<int> id_contenu { get; set; }
        public Nullable<int> type_contenu { get; set; }
        public string portes { get; set; }
        public Nullable<int> etat { get; set; }
        public Nullable<int> id_partie { get; set; }

        public virtual PartieDTO PartieDto { get; set; }
    }
}
