using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IGameService
    {
        static Dictionary<string , Game>? _games ;

         Task<Game> CreateNewGameAsync(string playerID);


         Task<Game> InitiateGameAsync(string gameId, int size );

    }
}