using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace server.Interfaces
{
    public interface IWordService
    {
        List<string> GetWords(int size);
        IReadOnlyDictionary<int,List<string>> GetAllWordLists ();
    }
}