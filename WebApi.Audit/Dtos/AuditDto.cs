using System;
using WebApi.Audit.Domain.Enums;

namespace WebApi.Audit.Dtos
{
    public class AuditDto
    {
        public UciRodApplication ApplicationCode { get; set; }
        public string User { get; set; }
        public string EntityId { get; set; }
        public string Entity { get; set; }
        public string OldEntity { get; set; }
        public AuditAction Action { get; set; }
        public DateTime Date { get; set; }
    }
}