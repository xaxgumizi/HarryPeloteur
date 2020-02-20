using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPeloteur_DAL
{
    public class GameInformationDTO
    {
        public PartieDTO game { get; set; }
        public PersonneDTO character { get; set; }
        public List<SalleDTO> rooms { get; set; }
    }
}
