using System;
using Domain.Enums;

namespace Application.Dtos
{
    public class UserDto : ICrudDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public bool Activate { get; set; }
    }
}