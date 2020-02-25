﻿using System;
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




            gameInfos = db.GetGameInfos(id); // Obtient les nouvelles infos après que le handler les ait modifiés


            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { gameInfos, texts });
        }
    }
}