using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs
{
    public class PuzzleDto
    {
        // string array 

        // this is string array to support json serializatoin 
        public string[]? GridSingleD { get; set; } 
        public List<string>  ListOfWords { get; set; }
        // int size
    }
}