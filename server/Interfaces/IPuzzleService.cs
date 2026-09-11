using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace server.Interfaces
{
    public interface IPuzzleService
    {
        // method to get puzzle with list of words and size (for testing purpose let it return a grid now )
          char[,] GeneratePuzzle(int size);
    }
}