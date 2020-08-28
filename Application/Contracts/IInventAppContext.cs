using System;

namespace Application.Contracts
{
    public interface IInventAppContext
    {
        string SecurityToken { get; set; }

        Guid UserId { get; set; }
        string UserEmail { get; set; }
    }
}