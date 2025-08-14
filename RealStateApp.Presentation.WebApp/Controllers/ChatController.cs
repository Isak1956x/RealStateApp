using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Presentation.WebApp.Handlers;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public ChatController(IChatService chatService, IMessageService messageService)
        {
            _chatService = chatService;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(int propertyId, string message)
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageDto = new MessageDto
                {
                    ChatID = 3,
                    Content = message,
                    SenderID = 1, // Replace with actual sender ID
                    Date = DateTime.Now
                };
               await _messageService.AddAsync(messageDto);
            }
            return RedirectToRoute( new { controller= "Property", action = "Details", Id = propertyId });
        }
    }
}
