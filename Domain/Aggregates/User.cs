using System;
using System.Linq;
using Domain.Attributes;
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

        [Required]
        public string Name { get; set; }

        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public UserRol RoleId { get; set; }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool Activate { get; set; }

        public bool IsUsingCustomPassword { get; set; }

        public bool IsLocked() => AccessFailedCount == 3;

        public bool PasswordIsValid(string password) => Password == password;

        public bool IsAdmin() => RoleId == UserRol.Admin;

        public static bool EmailIsValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void GenerateDefaultPassword()
        {
            IsUsingCustomPassword = false;

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            const int passwordLength = 10;
            Password = new string(Enumerable.Repeat(chars, passwordLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool PasswordSatisfyComplexity(string password)
        {
            return password.Length == 10; //TODO: improve complexity
        }
    }
}