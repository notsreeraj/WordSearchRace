using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using server.Services;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Tests
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService = new PlayerService();
        private readonly ILogger<GameService> _loggger = NullLogger<GameService>.Instance ;

        public GameServiceTests(){
             _gameService = new GameService(null!,_playerService, _loggger);
        }

        // test the no game found
        // public Game InitiateGame(string gameId, int size)
        [Fact]
        public void InitiateGameArgumentException()
        {
            // arrange
            
            string testGameId = "ibgsabghb";
            int testSize = 20;
            

            // act  + Assert
            Assert.Throws<ArgumentException>(()=> _gameService.InitiateGame(testGameId,testSize));
            

        }

        /// <summary>
        /// to test the second exceptoin of not engought players
        /// </summary>
        [Fact]
        public void InitiateGameWithNotEnoughPlayers()
        {
            // arrange
            

            Game testGame = _gameService.CreateNewGame("testPlayer");


            // act + assert  
            Assert.Throws<Exception>(()=> _gameService.InitiateGame(testGame.Id,20));          
        }

        // test to check Joing game if player check is working
        [Fact]
        public void JoinGameInvalidPlayer()
        {

            // arrange
            string gameID = "ifbghb";
            string playerID = "dosn";
            


            // act + assert
            Assert.Throws<Exception>(()=> _gameService.JoinGame(gameID,playerID));
        }

        // Test to check invalid game entry
        [Fact]
        public void JoingGameInvalidGameID()
        {
            // requirement need valid playe id , IsPlayerActive() must should positive

            // arrange
            string gameID = "ifbghb";
            string playerID = "dosn";
            _playerService.AddNewActivePlayer(playerID);

            Assert.Throws<ArgumentException>(()=> _gameService.JoinGame(gameID,playerID));
        }

        // test to game reached maximum player
        [Fact]
        public void JoingGameMaxPlayerReached()
        {
            // requirement IspLayerActive must return true.
            // Must have a valid gameId
            // Game nust have more that 2 players

            // arrange
            
            string playerID = "dosn";
            _playerService.AddNewActivePlayer(playerID);
            Game testGame =_gameService.CreateNewGame(playerID);
            // add 1 more players to the gameplayer list
            testGame.Players.Add("iughbg;");
            Console.WriteLine($"Player count in {testGame.Id} is {testGame.Players.Count}");

            Assert.Throws<Exception>(()=> _gameService.JoinGame(testGame.Id,playerID));
        }
        
    }
}