using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Common.Models
{
    public class SubGeneralLedgerBalanceDto
    {
        public int SUB_GENERAL_LEDGER_BALANCE_ID { get; set; }
        public string? PARENT_LEDGER_CODE { get; set; } = string.Empty;
        public decimal? END_OF_DAY_BALANCE { get; set; }
        public int? SUB_GENERAL_LEDGER_COUNT { get; set; }
        public string? CREATED_BY { get; set; } = string.Empty;
        public DateTime DATE_CREATED { get; set; }
        public DateTime TIME_CREATED { get; set; }
        public virtual string? LAST_MODIFIED_BY { get; set; }
        public DateTime? LAST_MODIFIED_DATE { get; set; }
        public DateTime? LAST_MODIFIED_TIME { get; set; }
        public string? APPROVED_BY { get; set; }
        public DateTime? DATE_APPROVED { get; set; }
        public DateTime? TIME_APPROVED { get; set; }
        public string? STATUS { get; set; }
        public string? HASH_VALUE { get; set; }
        public bool DELETED_FLAG { get; set; } = false;
        public string? DELETED_BY { get; set; } = string.Empty;
        public bool IS_DELETED { get; set; } = false;
        public DateTime? DATE_DELETED { get; set; }
        public DateTime? TIME_DELETED { get; set; }
        public ulong? ROW_VERSION { get; set; }
    }
}