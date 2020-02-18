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
        DebugTools dt = new DebugTools();

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
            dt.dbg(command);

            var LogicHandler = new GameLogic(); // La logique du jeu

            var gameinfos = db.getGameInfos(id); // Obtient les informations actuelles sur la partie

            string[] parameters = command.Split(' '); // Parse la commande
            if (parameters.Length > 0)
            {
                string action = parameters[0]; // Le verbe d'action
                switch (action)
                {
                    case "avancer": // Gére le déplacement du personnage
                        LogicHandler.HandleAvancer(gameinfos, parameters);
                        break;
                    case "combattre": // Gére le combat
                        LogicHandler.HandleCombattre(gameinfos, parameters);
                        break;
                    case "fuir": // Gére la fuite
                        LogicHandler.HandleFuir(gameinfos, parameters);
                        break;
                    case null:
                        dt.dbg("Commande non reconnue");
                        break;
                }
                    
            }

            gameinfos = db.getGameInfos(id); // Obtient les nouvelles infos après que le handler les ait modifiés


            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameinfos });
        }
    }
}
