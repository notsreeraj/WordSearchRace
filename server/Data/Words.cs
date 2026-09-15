using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Data
{
    public class Words
    {
        public Dictionary<int, List<string>> WordLists = new Dictionary<int, List<string>>
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
    }
}