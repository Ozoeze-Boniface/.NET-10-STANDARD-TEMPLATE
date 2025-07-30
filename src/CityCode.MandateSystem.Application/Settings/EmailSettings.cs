using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Settings
{
    public class EmailSettings
    {
        public string? SendGridApiKey { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
    }
}