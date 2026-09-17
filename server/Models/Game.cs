using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace server.Models
{

    /// <summary>
    ///  this class represents a game
    /// </summary>
    /// <param name="playerId">Needs atleast one player to start a game</param>
    public class Game(string playerId)
    {
        public string Id { get; set; }   =  Guid.NewGuid().ToString();
        public List<string> Players { get; set; } = new List<string>
        {
            playerId
        };
        public Puzzle? Puzzle { get; set; }

    }
}