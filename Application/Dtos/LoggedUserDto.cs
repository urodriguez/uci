using System;

namespace Application.Dtos
{
    public class LoggedUserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}