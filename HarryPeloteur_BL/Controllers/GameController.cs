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

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { newgameinfos });
        }
    }
}
