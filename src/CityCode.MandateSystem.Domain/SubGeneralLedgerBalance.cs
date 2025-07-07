using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain
{
    public class SubGeneralLedgerBalance : BaseAuditableEntity
    {
        public int SubGeneralLedgerBalanceId { get; set; }
        public string? ParentLedgerCode { get; set; } = string.Empty;
        public decimal? EndOfDayBalance { get; set; }
        public int? SubGeneralLedgerCount { get; set; }
    }
}