namespace Application
{
    public interface IApplicationResult
    {
        int Status { get; set; }
        string Message { get; set; }
    }
}