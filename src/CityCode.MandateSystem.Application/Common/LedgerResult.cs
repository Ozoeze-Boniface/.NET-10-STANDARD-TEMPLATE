using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Common
{
    public class LedgerResult<T>
    {
        public T? Value { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess => !string.IsNullOrEmpty(ErrorMessage);
    }
}