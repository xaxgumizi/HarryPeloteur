using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarryPeloteur_BL.Controllers
{
    public class dbController
    {
        DebugTools dt = new DebugTools();
        private Models.CharacterDTO personnage = new Models.CharacterDTO()
        {
            id = 0,
            salle_actuelle = 12,
            nom = "Harroux",
            pv = 100,
            force = 10,
            fuite = 3,
            dexterite = 2,
            xp = 10,
            po = 0
        };

        private Models.PartieDTO partie = new Models.PartieDTO()
        {
            id = 0,
            id_personnage = 52,
            difficulte = 0
        };


        private List<Models.SalleDTO> salles = new List<Models.SalleDTO> { };


        public dbController()
        {
            Models.SalleDTO room1 = new Models.SalleDTO()
            {
                id = 12,
                id_partie = 0,
                coordonnees = new int[] { 0, 0 },
                id_contenu = 2,
                type_contenu = 2,
                portes = new int[] { 0, 1, 1, 0 },
                etat = 0
            };
            this.salles.Add(room1);


            Models.SalleDTO room2 = new Models.SalleDTO()
            {
                id = 13,
                id_partie = 0,
                coordonnees = new int[] { 1, 0 },
                id_contenu = 2,
                type_contenu = 1,
                portes = new int[] { 1, 0, 0, 1 },
                etat = 0
            };
       
            this.salles.Add(room2);
        }

        public Models.CharacterDTO getCharacter(int id)
        {
            var personnage = this.personnage;
            personnage.id = id;

            return personnage;
        }

        public Models.PartieDTO getGame(int id)
        {
            var partie = this.partie;
            partie.id = id;

            return partie;
        }

        public List<Models.SalleDTO> getRoom(int id)
        {
            return this.salles;
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

        public Models.MonstreDTO getMonster(int id)
        {
            var monster = new Models.MonstreDTO()
            {
                Id = 2,
                Nom = "Jean-Claude Van Dame",
                Pv = 100,
                Force = 10,
                Dexterite = 3,
                Drop_xp = 10,
                Drop_argent = 5,
                Proba_drop_argent = 0.5F
            };
            return monster;
        }

        public void insertRoom(Models.SalleDTO room)
        {
            this.salles.Add(room);
        }
        public void updateRoom(Models.SalleDTO room)
        {
            var logic = new GameLogic();
            int index = logic.FindRoomById(this.salles, room.id).index;
            this.salles[index] = room;
        }
        public void updateCharacter(Models.CharacterDTO character)
        {
            dt.dbg("Mise à jour du personnage");
            //dt.VarDump(character);
            this.personnage = character;
        }
    }
}