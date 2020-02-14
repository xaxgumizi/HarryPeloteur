using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Controllers
{
    public class GameLogic
    {
        public Models.GameInformationDTO HandleAvancer(Models.GameInformationDTO gameinfos, string[] parameters)
        {
            switch (parameters[1])
            {
                case "devant":
                    break;
                case "droite":
                    break;
                case "gauche":
                    break;
                case "derriere":
                    break;
            }

            return gameinfos;
        }
    }
}