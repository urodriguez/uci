using System;
using System.Linq;
using Domain.Attributes;
using Domain.Contracts.Aggregates;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Aggregates
{
    public class User : Entity, IAggregateRoot
    {
        public User()
        {
            DateCreated = DateTime.UtcNow;
        }

        [Required]
        public string Name { get; private set; }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new BusinessRuleException($"{EntityName}: name can not be null or empty");
            if (name.Length >= 32) throw new BusinessRuleException($"{EntityName}: name length can not be greater than 32");
            Name = name;
        }

        public virtual string Password { get; private set; }
        public void SetPassword(string password)
        {
            if (!PasswordSatisfyComplexity(password)) throw new BusinessRuleException($"{EntityName}: password does not satisfy the complexity");
            Password = password;
        }
        private static bool PasswordSatisfyComplexity(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length == 8; //TODO: improve complexity
        }

        [Required]
        public string Email { get; private set; }
        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new BusinessRuleException($"{EntityName}: email can not be null or empty");
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                Email = addr.Address;
            }
            catch
            {
                throw new BusinessRuleException($"{EntityName}: email format is invalid ");
            }
        }

        [Required]
        public string FirstName { get; private set; }
        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName)) throw new BusinessRuleException($"{EntityName}: firstName can not be null or empty");
            if (firstName.Length >= 32) throw new BusinessRuleException($"{EntityName}: firstName length can not be greater than 32");
            FirstName = firstName;
        }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; private set; }
        public void SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName)) throw new BusinessRuleException($"{EntityName}: lastName can not be null or empty");
            if (lastName.Length >= 32) throw new BusinessRuleException($"{EntityName}: lastName length can not be greater than 32");
            LastName = lastName;
        }

        [Required]
        public virtual UserRole Role { get; private set; }
        public void SetRole(UserRole role)
        {
            if (!Enum.IsDefined(typeof(UserRole), role)) throw new BusinessRuleException($"{EntityName}: role is not defined");
            Role = role;
        }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool Activate { get; set; }

        public bool IsUsingCustomPassword { get; set; }

        public bool IsLocked() => AccessFailedCount == 3;

        public bool HasPassword(string password) => Password == password;

        public virtual bool IsAdmin() => Role == UserRole.Admin;

        public void GenerateDefaultPassword()
        {
            IsUsingCustomPassword = false;

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            const int passwordLength = 8;
            var password = new string(Enumerable.Repeat(chars, passwordLength).Select(s => s[random.Next(s.Length)]).ToArray());
            SetPassword(password);
        }

        public void ResetAccessFailedCount()
        {
            AccessFailedCount = 0;
        }
    }
}