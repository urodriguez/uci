namespace Application.ApplicationResults
{
    public class EmptyResult : IApplicationResult
    {
        public ApplicationStatus Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful() => Status == ApplicationStatus.Ok;
    }
}