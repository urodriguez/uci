namespace Application.ApplicationResults
{
    public interface IApplicationResult
    {
        ApplicationResultStatus Status { get; set; }
        string Message { get; set; }
        bool IsSuccessful();
    }
}