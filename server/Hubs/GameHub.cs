using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.ObjectPool;
using server.DTOs;
using server.Interfaces;

namespace server.Hubs
{
    public class GameHub(IGameService _gameService , IPuzzleService _puzzleService) :Hub
    {


        public async Task CreateGame(CreateGameDTO createGameDto)
        {
            // create new game instance via gameserivce
            var game = _gameService.CreateNewGame(createGameDto.PlayerID,createGameDto.Size);
            
            // add the new connecionstringID to the group with name as game id
            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);
            // send the caller the message with gameid  (Clietns.Caller.SendAsync)
            await Clients.Caller.SendAsync("GameCreatedAndWaitingForPlayer" , game.Id );
        }




    
        public async Task JoinGame(JoinGameDTO joinGameDTO){
            // add the new connectionString id to the group with name as game id from dto
            await Groups.AddToGroupAsync(Context.ConnectionId,joinGameDTO.GameID);
            // call the join game method from gameservice via joingame method
            var game = _gameService.JoinGame(joinGameDTO.GameID,joinGameDTO.PlayerID);


            GameDTO gameDto = new GameDTO
            {
                Id = game.Id,
                Players = game.Players,
                Puzzledto = new PuzzleDto
                {
                    GridSingleD = _puzzleService.ConvertToStringArr(game.Puzzle.Grid)
                    ,ListOfWords = game.Puzzle.ListOfWords
                }

            };
            // send the group message that the room is filled  
            await Clients.Group(game.Id).SendAsync("RoomReady", gameDto);

        }
        
    }
}