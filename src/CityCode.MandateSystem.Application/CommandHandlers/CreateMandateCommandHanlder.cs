using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Commands;
using CityCode.MandateSystem.Application.Settings;
using Microsoft.Extensions.Options;

namespace CityCode.MandateSystem.Application.CommandHandlers
{
    public class CreateMandateCommandHanlder : IRequestHandler<CreateMandateCommand, Common.Models.View.Result<MandateRequest>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly SystemSettings _systemSettings;

        public CreateMandateCommandHanlder(IApplicationDbContext context, IMapper mapper, IOptions<SystemSettings> systemSettings)
        {
            _context = context;
            _mapper = mapper;
            _systemSettings = systemSettings.Value;
        }

        public async Task<Common.Models.View.Result<MandateRequest>> Handle(CreateMandateCommand request, CancellationToken cancellationToken)
        {
            var mandateRequest = _mapper.Map<MandateRequest>(request);
            mandateRequest.SetBillerAndProductDetails(_systemSettings.BillerId, _systemSettings.ProductId);
            mandateRequest.GenerateMandateRefernce();
            await _context.MandateRequests.AddAsync(mandateRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<MandateRequest>.Success(DateTime.UtcNow, mandateRequest);
        }
    }
}