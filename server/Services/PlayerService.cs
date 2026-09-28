using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;

namespace server.Services
{
    public class PlayerService : IPlayerService
    {

        private readonly List<string> ActivePlayers = new List<string>();

        public bool IsPlayerActive(string playerID)
        {
            return ActivePlayers.Contains(playerID);
        }

        // method to a player to active player list
        public void AddNewActivePlayer(string playerID)
        {
            ActivePlayers.Add(playerID);
        }
    }
}