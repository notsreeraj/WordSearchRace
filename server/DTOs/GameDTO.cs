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
            public required PuzzleDto Puzzle { get; set; }
    }
        public class InitiateGameDTO
    {
        public string? GameId { get; set; }
        public int Size { get; set; }
    }



}