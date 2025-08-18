using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatID { get; set; }
        public Chat? Chat { get; set; }
        public required string Content { get; set; }
        public required string SenderID { get; set; }
        public DateTime Date { get; set; }

    }
}
