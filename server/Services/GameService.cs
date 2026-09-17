using System;
using System.Collections.Concurrent;
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

        private readonly ConcurrentDictionary<string , Game>? _games = new ConcurrentDictionary<string, Game>();
        
        #endregion


        /// <summary>
        /// method to create a new game instance
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public  Game CreateNewGame(string playerID)
        {
             var newGame =  new Game(playerID);

             _games.TryAdd( newGame.Id,newGame);

            return   newGame;
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
        public Game InitiateGame(string gameId, int size)
        {
            Console.WriteLine($"""
            
            *******
            Game id = {gameId}
            *********
            
            """);
            var currentGame  = FindGameID(gameId);
            if(currentGame == null) throw new ArgumentException("Game not found"); 
            // we should also make sure that both players are ready /
            // if on clicks ready he should be waiting for the next one to press ready



            // get the puzzlle with size and listChoice
            var newPuzzle =  _puzzleService.GeneratePuzzle(size);
            currentGame.Puzzle = newPuzzle;

            return  currentGame;
        }

        /// <summary>
        /// finds a game with id
        /// returns null if not found
        /// here i wrote game? becaue this method may return null
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        private Game? FindGameID(string gameId)
        {
           
            // here i changed the containskey to trygetvalue to make it thread safe . 
            // the previous can cause situatoin where key can be delet by other calls to the same dictionary
            return _games.TryGetValue(gameId, out var game) ? game: null;
             
        }






    }//end class

}//end namespace