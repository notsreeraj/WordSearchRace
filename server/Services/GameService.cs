using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Tree;
using server.Interfaces;
using server.Models;
using Microsoft.Extensions.Logging;
using server.DTOs;

namespace server.Services
{
    public class GameService(IPuzzleService _puzzleService , IPlayerService _playerService , ILogger<GameService> _logger) : IGameService
    {


        #region Memory

        private readonly ConcurrentDictionary<string , Game>? _games = new ConcurrentDictionary<string, Game>();
        
        #endregion


        #region Inherited Methods


        #endregion

        /// <summary>
        /// method to create a new game instance
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public  Game CreateNewGame(string playerID , int size)
        {
             var newGame =  new Game(playerID);
             newGame.Puzzle =  _puzzleService.GeneratePuzzle(size);

             
             

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
           _logger.LogDebug("Initiating game {gameId} with size {size}",gameId, size);
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
            // if(!_playerService.IsPlayerActive(newPlayerID)) throw new Exception("Player Not Valid");

            // validate the gameID
            var currentGame  = FindGameID(gameID);
             if(currentGame == null) throw new ArgumentException("Game not found");


            // also check if there is already 2 player in the game 
            if(currentGame.Players.Count == 2 ) throw new Exception("Reached Maximum amount of players in Game"); 
            
            // use the currentGame and add the new player to the playeslist
            currentGame.Players.Add(newPlayerID);
            currentGame.PlayersProgress.Add(newPlayerID,0);                 
            return currentGame;
        }

        // method to convet game model to gamedto

        public GameDTO ConvertGameToDto(string gameId)
        {
             var game = FindGameID(gameId);
             if(game == null) throw new ArgumentException("Game not found");

            GameDTO gameDto = new GameDTO
            {
                Id = game.Id,
                Players = game.Players,
                Puzzledto = new PuzzleDto
                {
                    GridSingleD = _puzzleService.ConvertToStringArr(game.Puzzle.Grid)
                    ,ListOfWords = game.Puzzle.ListOfWords 
                }

            };
            return gameDto;
        }

        /// <summary>
        /// bridges gameservice and puzzle serice to validate selection 
        /// </summary>
        /// <param name="userSelection"> List of cells </param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ValidateSelection(List<Cell> userSelection , string gameID)
        {
            // make a string from the list of cells 
            string selectedWord ="";
            foreach(Cell c in userSelection)
            {
                selectedWord = selectedWord + c.letter;
            }

            var game = FindGameID(gameID);
 
            List<string> wordList = game.Puzzle.ListOfWords;

            // call Validateword from puzzelservice and pass the string to the method
            // also get the list of word from the game id  
            // find the game 
            
            var result = _puzzleService.Validateword(wordList,selectedWord);

            // return its value

            return result;
        }
        // method to update a player progress
        public Dictionary<string,int> UpdatePlayerProgress( string playerID, string gameId)
        {
            // get the game reference
            var game = FindGameID(gameId);
            if(game == null) throw new Exception("Game Not Found");


            // update the value in dictionary by key
            game.PlayersProgress[playerID] ++;
            _games[gameId]= game;
            return game.PlayersProgress;
            // and also update the game
        }

        /// <summary>
        /// method to see if a player has won by id
        /// </summary>
        /// <param name="playrId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public bool HasPlayerWon(string playerId, string gameId)
        {
            // get the game reference
            var game = FindGameID(gameId);
            if(game == null) throw new Exception("Game Not Found");

            // find the progress of the player and see if the value is same as the count of words
            var prog = game.PlayersProgress[playerId];
            var maxProg = game.Puzzle.ListOfWords.Count;

            if(prog == maxProg)
            {
                return true;
            }

            return false;            
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


        // method to check if a game has max players
        public bool IsMaxNumPlayer(string gameId)
        {
            var game = FindGameID(gameId);
            if(game == null) throw new Exception("Game Not Found");
            _logger.LogDebug("Number of player in the game is {game.Players.Count}",game.Players.Count);
            return game.Players.Count == 2;
        }


        // method to increment the number of players that are ready in a game
        public void UpdateNumPlayersReady( string gameId)
        {
            // get the game 
            // increment its number
            var game = FindGameID(gameId);
            if(game == null) throw new Exception("Game Not Found");

            game.PlayersReady ++;
            _games[gameId] = game;
             
        }

        // method to check if both players are ready for a game

        public bool AreBothPlayerReady(string gameId)
        {
            var game = FindGameID(gameId);
            if(game == null) throw new Exception("Game Not Found");

            return game.PlayersReady == 2;
        }

        


        #endregion






    }//end class

}//end namespace