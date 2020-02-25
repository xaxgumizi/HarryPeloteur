using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesWebAzure
{
    public class SalleDto
    {
        public int Id { get; set; }
        public int[] coordonnees { get; set; }
        public Nullable<int> id_contenu { get; set; }
        public Nullable<int> type_contenu { get; set; }
        public int[] portes { get; set; }
        public Nullable<int> etat { get; set; }
        public Nullable<int> id_partie { get; set; }
        public virtual PartieDto PartieDto { get; set; }
    }
}
