namespace Application.Contracts.Infrastructure.Mailing
{
    public interface IAttachment
    {
        byte[] FileContent { get; set; }
        string FileName { get; set; }
    }
}