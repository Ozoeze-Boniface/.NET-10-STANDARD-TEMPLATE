using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRails.BankingApi.Domain.Common
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = default!;
        public string Topic { get; set; } = default!;
        public string GroupId { get; set; } = default!;
    }
}