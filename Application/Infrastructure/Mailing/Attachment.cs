using Application.Contracts.Infrastructure.Mailing;

namespace Application.Infrastructure.Mailing
{
    public class Attachment : IAttachment
    {
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
    }
}