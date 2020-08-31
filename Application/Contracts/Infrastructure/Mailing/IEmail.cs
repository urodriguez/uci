using System.Collections.Generic;
using Application.Contracts.Infrastructure.Queueing;
using Application.Infrastructure;

namespace Application.Contracts.Infrastructure.Mailing
{
    public interface IEmail : IQueueable
    {
        InfrastructureCredential Credential { get; set; }
        bool UseCustomSmtpServer { get; set; }
        ISmtpServerConfiguration SmtpServerConfiguration { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        IEnumerable<IAttachment> Attachments { get; set; }
    }
}