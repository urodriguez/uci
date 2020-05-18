namespace Application.ApplicationResults
{
    public class OkApplicationResult<TData> : ApplicationResult<TData>
    {
        public OkApplicationResult()
        {
            Status = ApplicationResultStatus.Ok;
        }

        public override bool IsSuccessful() => true;
    }
}