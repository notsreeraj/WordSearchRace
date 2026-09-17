using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IPuzzleService
    {
        // method to get puzzle with list of words and size (for testing purpose let it return a grid now )
           Puzzle GeneratePuzzle(int size);
            char[,] ConvertToGrid(string[] rows);
            string[] GridToStringArr(char [,] grid);
    }
}