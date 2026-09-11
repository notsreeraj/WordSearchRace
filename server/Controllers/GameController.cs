using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Interfaces;
using server.Models;

namespace server.Controllers
{
    [Route("[controller]")]
    public class GameController(IGameService _gameService, IPuzzleService _puzzleService) : Controller
    {
        private readonly ILogger<GameController> _logger;

        // public GameController(ILogger<GameController> logger)
        // {
        //     _logger = logger;
        // }

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

        [HttpGet("CreateGame")]
        public async Task<ActionResult<Game>> CreateMatchRoom(string playerId)
        {
            // call game service to instantiate a game instance
           var newGame =   _gameService.CreateNewGame(playerId);


            return newGame;

        }

        #endregion
    }
}