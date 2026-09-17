using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.DTOs;
using server.Interfaces;
using server.Models;

namespace server.Controllers
{
    // url/Game/methodname

    
    [Route("[controller]")]
    public class GameController(IGameService _gameService ) : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        # region Endpoints

            // get[]

        [HttpGet("CreateGame/{playerId}")]
        public async Task<ActionResult<Game>> CreateMatchRoom(string playerId)
        {
            // call game service to instantiate a game instance
           var newGame =   _gameService.CreateNewGame(playerId);


          return  newGame;

        }


        [HttpPost("InitiateGame")]
        public async Task<ActionResult<Game>> InitiateGame([FromBody]InitiateGameDTO initiateGameDto)
        {                      
            // do the data validation in this controller itself
            if(initiateGameDto == null)
            {
                return BadRequest("Initiate game dto is  not valid");
            }

            else
            {
                Console.WriteLine($"game id from controller = {initiateGameDto.GameId}");
            var game =  _gameService.InitiateGame(initiateGameDto.GameId,initiateGameDto.Size);
            return Ok(game);
            }

            

        }

        #endregion
    }
}