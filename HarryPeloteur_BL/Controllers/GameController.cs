using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HarryPeloteur_BL.Controllers
{
    [RoutePrefix("api")]
    public class GameController : ApiController
    {
        dbController db = new dbController();
        [Route("game/{id}")]
        public HttpResponseMessage GetGame(int id) // https://localhost:44344/api/game/character?id=12
        {
            var gameinfos = db.getGameInfos(id);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameinfos });
        }

        [Route("games")]
        public HttpResponseMessage GetGames()
        {
            var liste = new List<Models.PartieDTO> { };
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { liste });
        }

        [Route("game/{id}")]
        public HttpResponseMessage PutGame(int id, [FromBody]string command)
        {
            System.Diagnostics.Debug.WriteLine(command);

            var LogicHandler = new GameLogic();

            var gameinfos = db.getGameInfos(id);
            var newgameinfos = new Models.GameInformationDTO();

            string[] parameters = command.Split(' ');
            if (parameters.Length > 0)
            {
                string action = parameters[0];
                switch (action)
                {
                    case "avancer":
                        newgameinfos = LogicHandler.HandleAvancer(gameinfos, parameters);
                        break;

                }

            }


            /******************* modifs Augustin **************************/
            piece = FindRoomById(newgameinfos.rooms, newgameinfos.character.salle_actuelle);
            //la piece actuelle
            var end_text = "";
            int champ = 0; // champ de texte traité 0:intro; 1:maintext; 2:outro;
            int type = 0;
            //type 
            //0 : intro;
            //1 : vide;
            //2 : objet;
            //3 : monstre;
            //4 : outro;
            while (champ < 3)
            {
                switch (champ)
                {
                    case 0:
                        type = 0;
                        break;
                    case 1:
                        type = 1;
                        switch (piece.type_contenu)
                        {//on filtre par contenu de a salle pour demander le texte correspondant
                            case 1: //objet
                                type = 2;
                                break;
                            case 2: //monstre
                                type = 3;
                                break;
                            default: //rien
                                break;
                        }
                        break;
                    case 2:
                        type = 4;
                        break;
                }
                end_text += get_text(type);
                champ++;
            }
            //actions possibles: 
            //mouvement 10-devant; 11-gauche; 12-droite; 13-derriere;
            //combat 20-combattre; 21-fuir;
            //objet 30-utiliser objet
            var actions_possibles = new List<string> { };
            switch (piece.etat)
            {
                case 0: //salle non terminée
                    switch (piece.type_contenu)
                    {
                        case 0: //rien
                            for (int i = 0; i < 4; i++)
                            {
                                if (piece.portes[i])
                                {
                                    actions_possibles.Add(ToString(i + 10));
                                }
                            }
                            break;
                        case 1: //objet
                            actions_possibles.Add(ToString(30));
                            for (int i = 0; i < 4; i++)
                            {
                                if (piece.portes[i])
                                {
                                    actions_possibles.Add(ToString(i + 10));
                                }
                            }
                            break;
                        case 2:
                            actions_possibles.Add(ToString(20));
                            actions_possibles.Add(ToString(21));
                            break;
                    }
                    break;
                case 1: //salle terminée
                    for (int i = 0; i < 4; i++)
                    {
                        if (piece.portes[i])
                        {
                            actions_possibles.Add(ToString(i + 10));
                        }
                    }
                    break;
            }



            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, end_text, actions_possibles, new { newgameinfos });
        }
    }
}
