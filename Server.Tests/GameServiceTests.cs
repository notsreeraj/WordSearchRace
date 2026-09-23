using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using server.Services;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace Server.Tests
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService = new PlayerService();

        public GameServiceTests(){
             _gameService = new GameService(null!,_playerService);
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
            // arrange
            string gameID = "ifbghb";
            string playerID = "dosn";

            Assert.Throws<ArgumentException>(()=> _gameService.JoinGame(gameID,playerID));
        }

        // test to game reached maximum player
        [Fact]
        public void JoingGameMaxPlayerReached()
        {
            // arrange
            string gameID = "ifbghb";
            string playerID = "dosn";

            Assert.Throws<Exception>(()=> _gameService.JoinGame(gameID,playerID));
        }
        
    }
}