namespace Application.Contracts
{
    public interface IInventAppContext
    {
        string UserName { get; set; }

        string SecurityToken { get; set; }
    }
}