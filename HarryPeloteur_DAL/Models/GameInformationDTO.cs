using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class GameInformationDTO
    {
        public PartieDTO Game { get; set; }
        public PersonneDTO Character { get; set; }
        public List<SalleDTO> Rooms { get; set; }
    }
}
