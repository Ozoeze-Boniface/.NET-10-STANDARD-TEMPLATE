using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Commands
{
    public class ApproveMandateCommand : IRequest<Common.Models.View.Result<Mandate>>
    {
        public long MandateId { get; set; }
        public long ApproverId { get; set; }
        public string ApprovedBy { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}