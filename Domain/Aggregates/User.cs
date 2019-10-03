using System;
using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Aggregates
{
    public class User : Entity, IAggregateRoot
    {
        public User()
        {
            DateCreated = DateTime.UtcNow;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public UserRol RoleId { get; set; }
        public bool EmailConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLoginTime { get; set; }
        public bool Activated { get; set; }

        public bool IsLocked() => AccessFailedCount == 3;

        public bool PasswordIsValid(string password) => Password == password;

        public bool IsAdmin() => RoleId == UserRol.Admin;
    }
}