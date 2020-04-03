using System;
using Domain.Enums;

namespace Application.Dtos
{
    public class UserDto : IDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public UserRol? RoleId { get; set; }
        public bool Activate { get; set; }
    }
}