using System;
using Application.Contracts;

namespace Application
{
    public class InventAppContext : IInventAppContext
    {
        public string SecurityToken { get; set; }

        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
    }
}