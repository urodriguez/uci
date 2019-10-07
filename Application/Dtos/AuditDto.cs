using System;
using Domain.Enums;

namespace Application.Dtos
{
    public class AuditDto
    {
        public int ApplicationCode { get; set; }
        public string User { get; set; }
        public Guid EntityId { get; set; }
        public string Entity { get; set; }
        public string OldEntity { get; set; }
        public AuditAction Action { get; set; }
        public DateTime Date { get; set; }
    }
}