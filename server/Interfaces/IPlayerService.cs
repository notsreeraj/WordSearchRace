using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Interfaces
{
    public interface IPlayerService
    {
        // method to serach a player from the active player list
        bool IsPlayerActive(string playerID);
    }
}