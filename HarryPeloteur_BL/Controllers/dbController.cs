using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Controllers
{
    public class dbController
    {
        public Models.CharacterDTO getCharacter(int id)
        {
            var personnage = new Models.CharacterDTO()
            {
                id = id,
                nom = "Harroux",
                pv = 5,
                force = 10.0F,
                fuite = 3.0F,
                dexterite = 2.0F,
                xp = 0,
                po = 0
            };

            return personnage;
        }

        public Models.PartieDTO getGame(int id)
        {
            var partie = new Models.PartieDTO()
            {
                id = id,
                id_personnage = 52,
                salle_actuelle = 12,
                difficulte = 0
            };

            return partie;
        }

        public List<Models.SalleDTO> getRoom(int id)
        {
            var room = new Models.SalleDTO()
            {
                id = 1345,
                id_partie = id,
                coordonnees = new int[] { 12, 52 },
                id_contenu = 2,
                type_contenu = 3,
                portes = new int[] { 0, 1, 0, 0 },
                etat = 0
            };

            var salles = new List<Models.SalleDTO> { room, room };
            
            return salles;
        }

        public Models.GameInformationDTO getGameInfos(int id)
        {
            var gameinfos = new Models.GameInformationDTO()
            {
                game = getGame(id),
                character = getCharacter(id),
                rooms = getRoom(id)
            };
            return gameinfos;
        }
    }
}