namespace Application
{
    public class ApplicationResult<TData> : IApplicationResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}