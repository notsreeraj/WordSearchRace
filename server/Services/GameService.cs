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
    public class GameService(IPuzzleService _puzzleService , IPlayerService _playerService) : IGameService
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
            // check to confirm there is both player in the game
            if(currentGame.Players.Count != 2) throw new Exception("2 Players must be joined to initiate a game ");
            // if on clicks ready he should be waiting for the next one to press ready



            // get the puzzlle with size and listChoice
            var newPuzzle =  _puzzleService.GeneratePuzzle(size);
            currentGame.Puzzle = newPuzzle;

            return  currentGame;
        }

        /// <summary>
        /// method to let a player join a game
        /// </summary>
        /// <param name="gamedID"></param>
        /// <param name="newPlayeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Game JoinGame(string gameID, string newPlayerID)
        {
            // validate whether the player is active
            if(!_playerService.IsPlayerActive(newPlayerID)) throw new Exception("Player Not Valid");

            // validate the gameID
            var currentGame  = FindGameID(gameID);
            if(currentGame == null) throw new ArgumentException("Game not found");


            // also check if there is already 2 player in the game 
            if(currentGame.Players.Count == 2 ) throw new Exception("Reached Maximum amount of players in Game"); 
            
            // use the currentGame and add the new player to the playeslist
            currentGame.Players.Add(newPlayerID);                 
            return currentGame;
        }



        #region Helper Methods

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
            // the previous can cause situatoin where key can be deleted by other calls to the same dictionary
            return _games.TryGetValue(gameId, out var game) ? game: null;
             
        }


        #endregion






    }//end class

}//end namespace