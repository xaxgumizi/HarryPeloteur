using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Models
{
    public class GameInformationDTO
    {
        public Models.PartieDTO game { get; set; }
        public Models.CharacterDTO character { get; set; }
        public List<Models.SalleDTO> rooms { get; set; }
    }
}