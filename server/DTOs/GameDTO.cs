using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.DTOs
{
    public class GameDTO
    {
        // everything is req beacauer thhes info is imp for client to render
            public required string Id { get; set; }       
            public required List<string> Players { get; set; } 
            public  PuzzleDto? Puzzledto { get; set; }
    }
        public class InitiateGameDTO
    {
        public required string GameId { get; set; }
        public required string PlayerID { get; set; }

        public required bool PlayersReady { get; set; }
    }

    public class JoinGameDTO
    {
        public required string GameID { get; set; }
        public required string PlayerID { get; set; }
    }

    public class CreateGameDTO
    {
        public required string PlayerID { get; set; }
        public int Size { get; set; }
    }



}