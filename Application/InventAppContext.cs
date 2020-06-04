using Application.Contracts;

namespace Application
{
    public class InventAppContext : IInventAppContext
    {
        public string UserName { get; set; }
        public string SecurityToken { get; set; }
    }
}