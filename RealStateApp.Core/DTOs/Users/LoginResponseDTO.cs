using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Core.Application.DTOs.Users
{
    public class LoginResponseDTO
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public bool IsVerified { get; set; } = false;

    }
}
