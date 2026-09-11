using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IGameService
    {
        static Dictionary<string , Game> _games ;

        Game CreateNewGame(string playerID);


        Game InitiateGame(int gameId, int size , int listChoice);

    }
}