using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Player
    {
        public required string Id { get; set; }
        public string? ConnectionId { get; set; }
    }
}