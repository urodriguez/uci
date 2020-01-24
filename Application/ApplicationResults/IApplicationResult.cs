namespace Application.ApplicationResults
{
    public interface IApplicationResult
    {
        ApplicationStatus Status { get; set; }
        string Message { get; set; }
    }
}