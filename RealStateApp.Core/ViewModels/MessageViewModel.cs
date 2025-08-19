using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public int ChatID { get; set; }
        public ChatViewModel? Chat { get; set; }
        public required string Content { get; set; }
        public required string SenderID { get; set; }
        public DateTime Date { get; set; }
        public bool isFromClient { get; set; }
        public string? SenderName { get; set; }

    }
}
