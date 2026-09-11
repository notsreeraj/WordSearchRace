using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Tree;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class GameService(IPuzzleService _puzzleService) : IGameService
    {


        #region Memory

        static Dictionary<string , Game> _games = new Dictionary<string, Game>();
        
        #endregion


        /// <summary>
        /// method to create a new game instance
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Game CreateNewGame(string playerID)
        {
            var newGame = new Game(playerID);

            _games.Add( newGame.Id,newGame);

            return newGame;
        }

        /// <summary>
        /// method to statea a game , get puzzle and list of words set up via puzzle service
        /// this method should get new puzzle with size 
        /// if the game is not found throw game nor found error
        /// </summary>
        /// <param name="size"></param>
        /// <param name="listChoice"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Game InitiateGame(string gameId, int size, int listChoice)
        {
            var currentGame  = FindGameID(gameId);
            if(currentGame == null) throw new Exception("Game not found"); 


            // get the puzzlle with size and listChoice
            var newPuzzle = _puzzleService.GeneratePuzzle(size , listChoice);

            return currentGame;
        }




        /// <summary>
        /// finds a game with id
        /// returns null if not found
        /// here i wrote game? becaue this method may return null
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        static Game? FindGameID(string gameId)
        {
           

           if(_games.ContainsKey(gameId)) return _games[gameId];
            else return null;
             
        }
    }
}