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
    public class GameController(IGameService _gameService , IPuzzleService _puzzleService ) : Controller
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
        public async Task<ActionResult<GameDTO>> InitiateGame([FromBody]InitiateGameDTO initiateGameDto)
        {                      
            // do the data validation in this controller itself
            if(initiateGameDto == null)
            {
                return BadRequest("Initiate game dto is  not valid");
            }
            // we should also make sure that both players are ready 
            // let us make sure that in the front end the dto bool property is only true when both playes are ready
            if(!initiateGameDto.PlayersReady) return BadRequest("Players must be ready to initiate a game");
            else
            {
                Console.WriteLine($"game id from controller = {initiateGameDto.GameId}");
            Game game =  _gameService.InitiateGame(initiateGameDto.GameId,initiateGameDto.Size);
            
            /*
            converting game obect to game dto which also includes puzzledto
            */
            GameDTO   gamedto = new GameDTO{
                Id =  game.Id,
                Players = game.Players,
                Puzzledto = new PuzzleDto{
                    // call the method from puzzle service char[] to string []
                    GridSingleD = _puzzleService.ConvertToStringArr(game.Puzzle.Grid)
                    ,ListOfWords = game.Puzzle.ListOfWords
                }

            };

            return Ok(gamedto);
            }

            

        }



        [HttpPost("JoinGame")]
        public async  Task<ActionResult<GameDTO>> JoinGame([FromBody]JoinGameDTO joinGameDTO){
            if(joinGameDTO == null){
                return BadRequest("Not enough info to conitinue");
            }

            else{
                var gameToJoin = _gameService.JoinGame(joinGameDTO.GameID,joinGameDTO.NewPlayerID);

                GameDTO gameDTO = new GameDTO{
                    Id = gameToJoin.Id,
                    Players = gameToJoin.Players,
                    Puzzledto = new PuzzleDto{
                    // call the method from puzzle service char[] to string []
                    GridSingleD = _puzzleService.ConvertToStringArr(gameToJoin.Puzzle.Grid)
                    ,ListOfWords = gameToJoin.Puzzle.ListOfWords
                }

                };
                return Ok(gameDTO);
            }
        }
        #endregion
    }
}