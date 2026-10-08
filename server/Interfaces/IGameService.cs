using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IGameService
    {
         

         Game CreateNewGame(string playerID , int size);


         Game InitiateGame(string gameId, int size );
         Game JoinGame(string gamedID , string newPlayeID);
         bool IsMaxNumPlayer(string gameID);

        bool AreBothPlayerReady(string gameId);
        void UpdateNumPlayersReady(string gameId);
        GameDTO ConvertGameToDto(string  gameId);

        bool ValidateSelection(List<Cell> userSelection , string gamedID , string playerId);
        Dictionary<string,int> UpdatePlayerProgress(string playerID, string gamedId);
        bool HasPlayerWon(string playerId, string gameId);
    }
}