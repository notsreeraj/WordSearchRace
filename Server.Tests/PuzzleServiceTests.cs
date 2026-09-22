using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using server.Services; // this is to add the reference to this class

namespace Server.Tests
{
    public class PuzzleServiceTests
    {
        


        // test for conveerttoStringArr method testing whether /0 converts to *
        // alright to the test this method i should give an empty grid as input and see if t
        // the out is all string containing *

        [Fact]
        public void ConvertToStringArrEmptyChar()
        {
            // arrange 
            var EmptyGrid = new char[2,2];            
            var _puzzleService = new PuzzleService(null!);
            var expected = new string[2]{"**","**"};

            // act 
            var result = _puzzleService.ConvertToStringArr(EmptyGrid);
            // Assert
            
            Assert.Equal(expected, result);

        }

        
    }
}