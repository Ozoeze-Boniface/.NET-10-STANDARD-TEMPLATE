using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public long ActivityId { get; set; }
        public string? Entity { get; set; }
        public long? Actor { get; set; }
        public string? ActorName { get; set; }
        public string? Action { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}