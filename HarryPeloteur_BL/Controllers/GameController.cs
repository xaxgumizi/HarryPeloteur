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

        //dbController db = new dbController();
        DebugTools dt = new DebugTools();
        HarryPeloteur_DAL.DBController db = new HarryPeloteur_DAL.DBController();

        GameLogic logicHandler = new GameLogic(); // La logique du jeu

        [Route("game/{id}")]
        public HttpResponseMessage GetGame(int id) // https://localhost:44344/api/game/character?id=12
        {
            var gameInfos = db.GetGameInfos(id);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameInfos });
        }

        [Route("games")]
        public HttpResponseMessage GetGames()
        {
            List<HarryPeloteur_DAL.PartieDTO> liste = db.GetParties();
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { liste });
        }

        [Route("game/{id}")]
        public HttpResponseMessage PutGame(int id, [FromBody]string command)
        {
            System.Diagnostics.Debug.WriteLine(command);


            

            var gameInfos = db.GetGameInfos(id); // Obtient les informations actuelles sur la partie

            string[] parameters = command.Split(' ');
            if (parameters.Length > 0)
            {
                string action = parameters[0];
                switch (action)
                {
                    case "avancer": // Gére le déplacement du personnage
                        logicHandler.HandleAvancer(gameInfos, parameters);
                        break;
                    case "combattre": // Gére le combat
                        logicHandler.HandleCombattre(gameInfos, parameters);
                        break;
                    case "fuir": // Gére la fuite
                        logicHandler.HandleFuir(gameInfos, parameters);
                        break;
                    case "ramasser": // Gère le fait de ramasser un objet
                        logicHandler.HandleRamasser(gameInfos, parameters);
                        break;
                }

            }


            /******************* modifs Augustin **************************/
            HarryPeloteur_DAL.SalleDTO piece = logicHandler.FindRoomById(gameInfos.Rooms, gameInfos.Character.SalleActuelle);
            //la piece actuelle
            var endText = "";
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
                        switch (piece.TypeContenu)
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
                endText += db.GetTexte(type);
                champ++;
            }
            //actions possibles: 
            //mouvement 10-devant; 11-gauche; 12-droite; 13-derriere;
            //combat 20-combattre; 21-fuir;
            //objet 30-utiliser objet
            var actionsPossibles = new List<string> { };
            switch (piece.Etat)
            {
                case 0: //salle non terminée
                    switch (piece.TypeContenu)
                    {
                        case 0: //rien
                            for (int i = 0; i < 4; i++)
                            {
                                if (piece.Portes[i] == 1)
                                {
                                    actionsPossibles.Add((i + 10).ToString());
                                }
                            }
                            break;
                        case 1: //objet
                            actionsPossibles.Add((30).ToString());
                            for (int i = 0; i < 4; i++)
                            {
                                if (piece.Portes[i] == 1)
                                {
                                    actionsPossibles.Add((i + 10).ToString());
                                }
                            }
                            break;
                        case 2:
                            actionsPossibles.Add((20).ToString());
                            actionsPossibles.Add((21).ToString());
                            break;
                    }
                    break;
                case 1: //salle terminée
                    for (int i = 0; i < 4; i++)
                    {
                        if (piece.Portes[i] == 1)
                        {
                            actionsPossibles.Add((i + 10).ToString());
                        }
                    }
                    break;
            }


            gameInfos = db.GetGameInfos(id); // Obtient les nouvelles infos après que le handler les ait modifiés


            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameInfos, endText, actionsPossibles });
        }
    }
}
