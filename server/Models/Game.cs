using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace server.Models
{

    /// <summary>
    /// this class represents a game
    /// </summary>
    /// <param name="playerId"> needs atleast one playr to initiate a game</param>
    public class Game(string playerId)
    {
        public string Id { get; set; }   = new Guid().ToString();
        public List<string> Players { get; set; }
        public Puzzle GamePuzzle { get; set; }
        
    }
}