using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class PuzzleService(IWordService _wordService , ILogger<PuzzleService> logger) : IPuzzleService
    {
        
        // here i am using struct based on the recomendation by MS, 
        // use struct for small , self-contained values .
        //Match represents the score , cell and the direction 
        record struct Match
        {
            public int Score;
            public int Row;
            public int Col;
            public int DRow;
            public int DCol;

        };

         static void PrintGrid(char[,] Grid)
        {
            Console.WriteLine(" ******Printing grid");
            for (int i = 0; i < Grid.GetLength(0); i++)
            {
            Console.WriteLine();

                for (int j = 0; j < Grid.GetLength(0); j++)
                {
                    if (Grid[i, j] == '\0') Console.Write('*');
                    else  Console.Write(Grid[i, j]);
                }
            }
            Console.WriteLine();
        }

   
    /// <summary>
    ///  the main method to generate a puzzle based on  size and list of choice
    /// </summary>
    /// <param name="size"></param>
    /// <param name="listChoice"></param>
    /// <returns></returns>
        public   Puzzle GeneratePuzzle(int size )
        {
    

        //get the list of words from word service
        var words = _wordService.GetWords(size);
        var sorterdWords= words.OrderByDescending(w => w.Length).ToList();
        ////now test the methods inside a loop
        char[,] newGrid = new char[size,size];

        List<string> placedWords = new List<string>();
        List<string> failedWords = new List<string>();

        foreach (string word in sorterdWords)
        {
        bool placed = AddWord(word, newGrid); // assuming you switched this to return bool, as discussed earlier
        if (placed)
            placedWords.Add(word);
        else
            failedWords.Add(word);
        }

        if (placedWords.Count == 0)
        {
            throw new InvalidOperationException("No words could be placed on the grid.");
        }

        var NewPuzzle = new Puzzle
        {
          Grid = newGrid,
          ListOfWords = words
            
        };
        PrintGrid(newGrid);
            

            
        


        return NewPuzzle;


            

            

        }

        /// <summary>
        /// method to find the best cell + direction among the grid 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="Grid"></param>
        static bool AddWord(string word , char[,] Grid)
        {
            // iterate through grid 
            // check 2 conditions  =>  1:  is current cell empty , 2: does current cell contain same letter as the first letter of the word

            // Console.WriteLine($"word == {word}");

            string currentWord = word;
            char[ , ] currentGrid = Grid;

        
            List<Match> Matches = new List<Match>();


            // ******** maybe we can store the match and score for all eight direction and choose the highest score at last//

            for ( int row  = 0; row < currentGrid.GetLength(0); row++)
            {
                for( int col = 0; col < currentGrid.GetLength(0);col++)
                {
                    // if current cell is empty 
                    if (currentGrid[row,col] == '\0' || currentGrid[row, col] == currentWord[0])
                    {

                        Match? match = Measure(word, currentGrid, row, col);
                        // this is to handle the null value return by measure
                        if (match.HasValue)
                        {
                            Matches.Add(match.Value);
                        }
                    }

                }
            }


            //PrintMatches(Matches);

            if (Matches.Count == 0)
            {
                Console.WriteLine($"No valid placement found for {word}");
                return false;
            }

            Match bestMatch = GetBestMatch(Matches);

           
            return AssignLetter(word, bestMatch, Grid);



        }



        //}

        /// <summary>
        ///  this method gives the best direction 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="Grid"></param>
        /// <param name="row"> row coordinate of cell</param>
        /// <param name="col"> col coordinate of the cell</param>
        static Match? Measure(string word, char[,] Grid, int row, int col)
        {
            int[,] directions =
            {
                { 0, 1 },    // East
                { -1, 1 },   // Northeast
                { -1, 0 },   // North
                { -1, -1 },  // Northwest
                { 0, -1 },   // West
                { 1, -1 },   // Southwest
                { 1, 0 },    // South
                { 1, 1 }     // Southeast
            };

            int curScore = 0;
            List<Match> matches = new List<Match>();

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int dRow = directions[i, 0];
                int dCol = directions[i, 1];
                int newScore = GetScore(row, col, word, dRow, dCol, Grid);



                // Only consider this direction if it's at least as good as current best
                if (newScore >= curScore)
                {


                    curScore = newScore;
                    matches.Add(new Match
                    {
                        Score = newScore,
                        Row = row,
                        Col = col,
                        DRow = dRow,
                        DCol = dCol
                    });
                }

                else continue;


            }
            
            if(matches.Count== 0)
            {
                return null;
            }


  
            List<Match> bestMatchesAllDir = matches.Where(m => m.Score == curScore).ToList();
            // PrintMatches(bestMatches);
  
            if (bestMatchesAllDir.Count == 0)
            {
                return null;
            }

            return GetBestMatch(bestMatchesAllDir);

            
        }

        /// <summary>
        /// This method returns score for a single direction based on the condition provided
        /// </summary>
        /// <param name="row"> position in the row </param>
        /// <param name="col"> position in col</param>
        /// <param name="word"> word from the list</param>
        /// <param name="drow"> direction along row</param>
        /// <param name="dcol"> direction along col</param>
        /// <param name="Grid"> reference of  the main grid </param>
        /// <returns></returns>
        static int GetScore (int row , int col , string word , int drow , int dcol , char[,] Grid )
        {
            int score = 1;

            int wordLength = word.Length;
            int wordIndex = 0;


            // get the end of x and y for each direction and use that as the limit condtion in inside the while loop
            int endRow = row + ((wordLength - 1) * drow);
            int endCol = col + ((wordLength - 1) * dcol);


            // this condition makes sure that we actually dont include the firection which will be out of bound
            if (endRow < Grid.GetLength(0) && endCol < Grid.GetLength(0) && endRow >= 0 && endCol >= 0)
            {

                while (wordIndex <= (wordLength - 1))
                {

                    // increase the score when same letter is found
                    if (Grid[row, col] == word[wordIndex])
                    {
                        score++;
                    }
                    // stop when anything other than empty charecter is found
                    else if (Grid[row, col] != '\0')
                    {
                        return 0;
                    }


                    // this moves to the next cell based on the direction
                    row = row + drow;
                    col = col + dcol;
                    wordIndex++;


                }
            }
            else return -1;
                return score;
        }


        /// <summary>
        /// returns a match with highest score from the list of matches from list of matches 
        /// also selects random match if there is more than on match with same score
        /// </summary>
        /// <param name="matches"></param>
        /// <returns></returns>
        static Match GetBestMatch(List<Match> matches)
        {                        

             if (matches.Count == 1)
            {

                return matches[0];
            }
             if(matches.Count == 0)
            {
                throw new Exception("there is no match in the list");
            }

            var randomMatch = matches[Random.Shared.Next(matches.Count)];


            
            return randomMatch;   
             
        }

        /// <summary>
        /// method which will assign letters from a word to grid based on direction
        /// </summary>
        /// <param name="word"></param>
        /// <param name="bestMatch"></param>
        /// <param name="grid"></param>
        static bool AssignLetter(string word , Match bestMatch , char[,]grid)
        {

            string w = word;
            int iRow = bestMatch.Row;
            int iCol = bestMatch.Col;
            int drow = bestMatch.DRow;
            int dcol = bestMatch.DCol;
            int wi = 0;
            int wordLength = word.Length;
            char[,] Grid = grid;

            while(wi <= (wordLength - 1))
            {


                // Console.WriteLine($"""
                //     this is inside assig letter method
                //     ********************

                //     iRow ={iRow}
                //     iCol ={iCol}
                //     ********************

                //     """);

                // assign letter to the grid appropriately
                Grid[iRow,iCol] = word[wi];
                iRow = iRow + drow;
                iCol = iCol + dcol;
                wi++;

                
            }
            return true;

        }



        /// <summary>
        /// this method prints all matches
        /// used to debug 
        /// </summary>
        /// <param name="matches"></param>
        static void PrintMatches(List<Match> matches)
        {
            foreach (Match m in matches)
            {
                Console.WriteLine(m);
            }
        }

        /// <summary>
        /// this method converts char[,] to string[] to make json friendlu
        /// </summary>
        /// <param name="grid">grid reference</param>
        /// <returns></returns>
        public string[] ConvertToStringArr(char [,] grid)      
        {
            

             var rows = Enumerable.Range(0, grid.GetLength(0))
            .Select(row => new string(Enumerable.Range(0, grid.GetLength(1))
                .Select(col => grid[row, col] == '\0' ? '*' : grid[row, col])
                .ToArray()))
            .ToArray();

        return rows;

        }

        /// <summary>
        /// this method converts string[] to char[,]
        /// </summary>
        /// <param name="rows">string [] referenc from dto</param>
        /// <returns></returns>
        public char[,] ConvertToGrid(string[] rows)
        {
            if (rows == null || rows.Length == 0)
            {
                throw new NullReferenceException("Given String [] is empty");
            }

            int rowCount = rows.Length;
            int colCount = rows[0].Length;

            char[,] grid = new char[rowCount, colCount];

            for (int r = 0; r < rowCount; r++)
            {
                // Optional: Handles variable-length strings safely
                int length = Math.Min(rows[r].Length, colCount);

                for (int c = 0; c < length; c++)
                {
                    // Replaces asterisks back to null chars if needed
                    char currentChar = rows[r][c];
                    grid[r, c] = currentChar == '*' ? '\0' : currentChar;
                }
            }

            return grid;
        }


    }
}