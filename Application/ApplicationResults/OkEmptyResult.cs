namespace Application.ApplicationResults
{
    public class OkEmptyResult : EmptyResult
    {
        public OkEmptyResult()
        {
            Status = ApplicationResultStatus.Ok;
        }

        public override bool IsSuccessful() => true;
    }
}