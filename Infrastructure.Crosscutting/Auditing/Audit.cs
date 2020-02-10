﻿using Domain.Contracts.Infrastructure.Crosscutting.Auditing;

namespace Infrastructure.Crosscutting.Auditing
{
    public class Audit : IAuditable
    {
        public string Application { get; set; }
        public string Environment { get; set; }
        public string User { get; set; }
        public string Entity { get; set; }
        public string EntityName { get; set; }
        public AuditAction Action { get; set; }
    }
}