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
        
        // test the argument exeption branch from InitiateGame metho
        // public Game InitiateGame(string gameId, int size)
        [Fact]
        public void InitiateGameArgumentException()
        {
            // arrange
            
            string testGameId = "ibgsabghb";
            int testSize = 20;
            GameService _gameService = new GameService(null!);

            // act  + Assert
            Assert.Throws<ArgumentException>(()=> _gameService.InitiateGame(testGameId,testSize));
            

        }
    }
}