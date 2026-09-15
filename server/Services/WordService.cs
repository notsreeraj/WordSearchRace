using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Interfaces;

namespace server.Services
{

    public class WordService : IWordService
    {


        private readonly Dictionary<int, List<string>> _wordLists = new Dictionary<int, List<string>>
        {
        [15] = new List<string>
        {
        "TIGER", "MOUNTAIN", "RIVER", "FOREST", "DESERT",
        "OCEAN", "VALLEY", "CANYON", "GLACIER", "VOLCANO",
        "PENINSULA", "ARCHIPELAGO"
        },
        [16] = new List<string>
        {
        "ELEPHANT", "GIRAFFE", "DOLPHIN", "PANTHER", "OCTOPUS",
        "KANGAROO", "CHEETAH", "CROCODILE", "RHINOCEROS", "HIPPOPOTAMUS"
        },
        [17] = new List<string>
        {
        "PLANET", "GALAXY", "METEOR", "ASTEROID", "SATELLITE",
        "TELESCOPE", "UNIVERSE", "NEBULA", "SUPERNOVA", "CONSTELLATION"
        },
        [18] = new List<string>
        {
        "BASKETBALL", "VOLLEYBALL", "BADMINTON", "SWIMMING", "WRESTLING",
        "GYMNASTICS", "ARCHERY", "CYCLING", "MARATHON", "TRIATHLON",
        "WEIGHTLIFTING"
        },
        [19] = new List<string>
        {
        "SYMPHONY", "ORCHESTRA", "TRUMPET", "CLARINET", "SAXOPHONE",
        "PERCUSSION", "HARMONICA", "ACCORDION", "XYLOPHONE", "KEYBOARD",
        "VIOLINIST", "COMPOSITION"
        },
        [20] = new List<string>
        {
        "AUSTRALIA", "ARGENTINA", "INDONESIA", "SWITZERLAND", "NETHERLANDS",
        "PHILIPPINES", "KAZAKHSTAN", "MADAGASCAR", "VENEZUELA", "BANGLADESH"
        },
        [21] = new List<string>
        {
        "SPAGHETTI", "HAMBURGER", "PINEAPPLE", "WATERMELON", "STRAWBERRY",
        "CHOCOLATE", "CROISSANT", "QUESADILLA", "ENCHILADA", "CAULIFLOWER"
        },
        [22] = new List<string>
        {
        "ELECTRICITY", "MAGNETISM", "GRAVITATION", "PHOTOSYNTHESIS", "MOLECULE",
        "CHROMOSOME", "HYDROGEN", "ORGANISM", "EXPERIMENT", "LABORATORY"
        },
        [23] = new List<string>
        {
        "ALGORITHM", "PROGRAMMING", "DEVELOPER", "DATABASE", "ENCRYPTION",
        "MICROPROCESSOR", "ARTIFICIAL", "INTELLIGENCE", "NETWORKING", "CYBERSECURITY"
        },
        [24] = new List<string>
        {
        "ENGINEER", "ARCHITECT", "PHOTOGRAPHER", "ACCOUNTANT", "ELECTRICIAN",
        "PHYSICIAN", "VETERINARIAN", "PSYCHOLOGIST", "CHOREOGRAPHER", "ENTREPRENEUR"
        },
        [25] = new List<string>
        {
        "UNBELIEVABLE", "RESPONSIBILITY", "CHARACTERISTIC", "MISUNDERSTANDING",
        "ACKNOWLEDGEMENT", "INCOMPREHENSIBLE", "DISPROPORTIONATE",
        "CONGRATULATIONS", "ADMINISTRATION"
        }
        };


        /// <summary>
        /// method to return the main dicitonry containing the list of words
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IReadOnlyDictionary<int, List<string>> GetAllWordLists() => _wordLists;
        
           
        
        /// <summary>
        /// Method to get the list of choich by the size of grid as choice
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetWords(int size)
        {
          return _wordLists.TryGetValue(size , out var list)
                    ? list
                    : throw new ArgumentException($"No word list found for size{size}");
        }
    }
}