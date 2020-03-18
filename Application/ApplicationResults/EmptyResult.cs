namespace Application.ApplicationResults
{
    public class EmptyResult : IApplicationResult
    {
        public ApplicationResultStatus Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful() => Status == ApplicationResultStatus.Ok;
    }
}