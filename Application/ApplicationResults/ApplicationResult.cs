namespace Application.ApplicationResults
{
    public class ApplicationResult<TData> : IApplicationResult
    {
        public ApplicationStatus Status { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}