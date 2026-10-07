using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
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
            await Clients.Caller.SendAsync("GameCreatedAndWaitingForPlayer" , game.Id);
        }




    
        public async Task JoinGame(JoinGameDTO joinGameDTO){
            // add the new connectionString id to the group with name as game id from dto
            await Groups.AddToGroupAsync(Context.ConnectionId,joinGameDTO.GameID);
            // call the join game method from gameservice via joingame method
            var game = _gameService.JoinGame(joinGameDTO.GameID,joinGameDTO.PlayerID);


           // GameDTO gameDto = _gameService.ConvertGameToDto(game);
            // send the group message that the room is filled  
            await Clients.Group(game.Id).SendAsync("GameJoined", "Click Ready to Start Game..." );

        }

        public async Task ReadyGame(InitiateGameDTO initiateGameDTO )
        {
            // we need a way to identify which player is ready , we need to map player and connection id or just sent true and player id 
            // first check if the gamge already has to players ready
            _gameService.UpdateNumPlayersReady(initiateGameDTO.GameId);

            if (_gameService.AreBothPlayerReady(initiateGameDTO.GameId))
            { 
                
                GameDTO gameDto = _gameService.ConvertGameToDto(initiateGameDTO.GameId);
                await Clients.Group(initiateGameDTO.GameId).SendAsync("BothPlayersReady",gameDto);
            }
            else
            {
                
                // send the message to caller
                await Clients.Caller.SendAsync("WaitingSecPlayerToReady" , "Waiting for all players to be ready");
            }
            // if not update the number
            // else send message to the both player with both players are ready
            
        }

        public async Task SubmitWord (SubmitWordDto submitWordDto)
        {
            // call the gameserivice ValidateSelectionMetho(pass in the list of cell from the dto)
            // if the result is true send the caller 1 point
            // if not send the caller 0 point
            // if the result is true , we should also check if a player has completed the list of words, maybe see if the player progress int is same as the count of the list of words
            var result = _gameService.ValidateSelection(submitWordDto.ClientSelection,submitWordDto.gameId);
            // if result is true
            if (result)
            {
                await Clients.Caller.SendAsync("ValidSelection","Your selection is valid");
                // send the the player progress dictionary to the caller or the group

                var playersProgress =  _gameService.UpdatePlayerProgress(submitWordDto.PlayerId,submitWordDto.gameId);
                await Clients.Group(submitWordDto.gameId).SendAsync( "UpdatedPlayersProgress" ,playersProgress);

                // if a playerhas won lets broadcast the message
                if (_gameService.HasPlayerWon(submitWordDto.PlayerId, submitWordDto.gameId))
                {
                    await Clients.Group(submitWordDto.gameId).SendAsync("RaceEnded",submitWordDto.PlayerId);
                }
                
            }
            else
            {
                // let the caller know the seleciotn is not valid
                await Clients.Caller.SendAsync("InvalidSelection","Your selection is not valid");
            }

            // if result is not true
        }
        
    }
}