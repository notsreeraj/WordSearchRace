using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.ObjectPool;
using server.Interfaces;

namespace server.Hubs
{
    public class GameHub(IGameService _gameService) :Hub
    {
        // method to create a group and joing 

        public async Task JoinRoom(string gameId){
            await Groups.AddToGroupAsync(Context.ConnectionId,gameId);

            // if there is already 2 players in teh game broacast message click ready to start game 
            if (_gameService.IsMaxNumPlayer(gameId))
            {
                // notify the players that they can initiate game by clicking ready
                // herr the first argument of sendAsync is an event not a method
                await Clients.Group(gameId).SendAsync("BothPlayersReady","Click Ready to start the race...");
            }
            else
            {
                // let the player know that they are waiting for new player.
                await Clients.Caller.SendAsync("WaitingForPlayer","Waiting for Opponent to Join..");
            }

        }
        
    }
}