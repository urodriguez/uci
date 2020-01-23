namespace Application
{
    public class EmptyResult : IApplicationResult
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public EmptyResult()
        {
            Status = 1;
            Message = "Empty result";
        }
    }
}