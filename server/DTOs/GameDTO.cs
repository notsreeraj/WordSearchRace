using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.DTOs
{
    public class GameDTO
    {
            public string Id { get; set; }       
            public List<string> Players { get; set; } 
            public Puzzle? GamePuzzle { get; set; }
    }
        public class InitiateGameDTO
    {
        public string? GameId { get; set; }
        public int Size { get; set; }
    }
    


}