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

        [Route("game/{id}")]
        public HttpResponseMessage GetGame(int id) // https://localhost:44344/api/game/character?id=12
        {
            var personnage = new Models.CharacterDTO()
            {
                id = 52,
                nom = "Harroux",
                pv = 5,
                force = 10.0F,
                fuite = 3.0F,
                dexterite = 2.0F,
                xp = 0,
                po = 0
            };

            var partie = new Models.PartieDTO()
            {
                id = id,
                id_personnage = 52,
                salle_actuelle = 12,
                difficulte = 0
            };

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


            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { partie, personnage, salles });
        }

        [Route("games")]
        public HttpResponseMessage GetGames()
        {
            var liste = new List<Models.PartieDTO> { };
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { liste });
        }
    }
}
