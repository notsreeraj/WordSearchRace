using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Puzzle()
    {
         public char[,]? Grid { get; set; }
         public List<string>? ListOfWords { get; set; }
    }
}