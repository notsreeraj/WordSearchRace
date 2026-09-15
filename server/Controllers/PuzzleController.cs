using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Interfaces;

namespace server.Controllers
{
    /// <summary>
    /// Controller to manage any request regarding puzzle
    /// </summary>
    /// <param name="puzzleService"></param>
    [Route("[controller]")]
    public class PuzzleController(IPuzzleService puzzleService) : Controller
    {


        // call  the generate puzzle method from puzzle service
        [HttpGet]
        public ActionResult<string[]> GetPuzzle()
        {
            var grid =  puzzleService.GeneratePuzzle(20);
            ;

             var rows = Enumerable.Range(0, grid.GetLength(0))
            .Select(row => new string(Enumerable.Range(0, grid.GetLength(1))
                .Select(col => grid[row, col] == '\0' ? '*' : grid[row, col])
                .ToArray()))
            .ToArray();

        return Ok(rows);

        }

    }
}