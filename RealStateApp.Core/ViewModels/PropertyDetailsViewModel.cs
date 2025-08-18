using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Application.ViewModels
{
    public class PropertyDetailsViewModel
    {
        public required PropertyViewModel Property { get; set; }
        public required UserViewModel Agent { get; set; }
        public  ChatViewModel? Chat { get; set; }
        public  List<ChatViewModel>? AgentChat { get; set; }
        public  List<OfferViewModel>? Offers { get; set; }
        public bool Addons { get; set; } = false;
        public string? Rol { get; set; }
    }
}
