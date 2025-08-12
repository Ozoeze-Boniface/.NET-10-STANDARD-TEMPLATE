using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain.Entities
{
    public class Bank
    {
        [Key]
        public string BankCode { get; set; } = null!; // NOT NULL
        public string BankName { get; set; } = null!; // NOT NULL
        public string? BankShortName { get; set; }
        public string? OldBankCode { get; set; }
        public int? BankCode2 { get; set; }
        public string? OldBankCode2 { get; set; }
        public string? BankShort { get; set; }
        public string? BankName2 { get; set; }
    }
}