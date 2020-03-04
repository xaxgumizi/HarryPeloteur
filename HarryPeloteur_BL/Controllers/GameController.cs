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
        public HttpResponseMessage GetGame(int id)
        {
            System.Diagnostics.Debug.WriteLine("Demande d'info pour la partie: " + id.ToString());
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
            System.Diagnostics.Debug.WriteLine("Commande reçue: " + command);

            var gameInfos = db.GetGameInfos(id); // Obtient les informations actuelles sur la partie
            string resultText = "";
            string[] parameters = command.Split(' ');
            if (parameters.Length > 0)
            {
                if (gameInfos.Character.Pv > 0)
                {
                    string action = parameters[0];
                    switch (action)
                    {
                        case "avancer": // Gére le déplacement du personnage
                            resultText = logicHandler.HandleAvancer(gameInfos, parameters);
                            break;
                        case "combattre": // Gére le combat
                            resultText = logicHandler.HandleCombattre(gameInfos, parameters);
                            break;
                        case "fuir": // Gére la fuite
                            resultText = logicHandler.HandleFuir(gameInfos, parameters);
                            break;
                        case "ramasser": // Gère le fait de ramasser un objet
                            resultText = logicHandler.HandleRamasser(gameInfos, parameters);
                            break;
                    }
                }
                else
                {
                    resultText = "Vous êtes mort Game Over";
                }
            }

            gameInfos = db.GetGameInfos(id); // Obtient les nouvelles infos après que le handler les ait modifiés
            var text = logicHandler.GenerateDisplayText(gameInfos);
            //text.add(resultText);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameInfos, text, resultText });
        }

        [Route("newGame")]
        public HttpResponseMessage PostNewGame([FromBody]string characterInfos)
        {
            string name = characterInfos.Substring(0,characterInfos.Length - 1);
            int difficulte = Int32.Parse(characterInfos.Substring(characterInfos.Length-1,1));
            
            int? ID = logicHandler.GenerateNewGame(name, difficulte);

            HarryPeloteur_DAL.GameInformationDTO gameInfos = db.GetGameInfos(ID);
            var text = logicHandler.GenerateDisplayText(gameInfos);
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameInfos, text });
        }
        

    }
}
