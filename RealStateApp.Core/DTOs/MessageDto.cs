using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int ChatID { get; set; }
        public ChatDto? Chat { get; set; }
        public required string Content { get; set; }
        public required string SenderID { get; set; }
        public DateTime Date { get; set; }

    }
}
