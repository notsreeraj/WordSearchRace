using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.DTOs
{
    public class PuzzleDto
    {
        // this is string array to support json serializatoin 
        public string[]? GridSingleD { get; set; } 
        public required List<string>  ListOfWords { get; set; }
        // int size
    }

    // DTO for selected word

    public class SubmitWordDto
    {
        public required string PlayerId { get; set; }

        //game id this is use to identify the game to get the puzzle
        public required string gameId { get; set; }
        // list of cell
        public required List<Cell> ClientSelection{get; set;}
    }
}