using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Dtos
{
    public class MailContent
    {
        public string Subject { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}