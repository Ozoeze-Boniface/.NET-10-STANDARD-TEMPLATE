using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Settings
{
    public class SystemSettings
    {
        public int ProductId { get; set; }
        public int BillerId { get; set; }
        public EmailSettings? EmailSettings { get; set; }
    }
}