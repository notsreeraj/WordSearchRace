using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IGameService
    {
         

         Game CreateNewGame(string playerID);


         Game InitiateGame(string gameId, int size );
         Game JoinGame(string gamedID , string newPlayeID);

    }
}