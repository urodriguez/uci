namespace Application.ApplicationResults
{
    public class ApplicationResult<TData> : IApplicationResult
    {
        public ApplicationResultStatus Status { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful() => Status == ApplicationResultStatus.Ok;
        public TData Data { get; set; }
    }
}