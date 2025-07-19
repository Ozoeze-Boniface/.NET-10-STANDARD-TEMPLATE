using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain.Entities
{
    public class Permission : BaseAuditableEntity
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "CreateUser", "DeletePost"
        public string Description { get; set; } = string.Empty; // Human-readable description
        public string Resource { get; set; } = string.Empty; // e.g., "User", "Post", "Report"
        public string Action { get; set; } = string.Empty; // e.g., "Read", "Write", "Delete", "Approve"
        public bool IsActive { get; set; } = true;
        public long UserId { get; set; }
    }

}