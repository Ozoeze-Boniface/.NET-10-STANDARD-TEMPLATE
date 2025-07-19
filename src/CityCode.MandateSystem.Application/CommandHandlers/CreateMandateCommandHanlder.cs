using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Commands;

namespace CityCode.MandateSystem.Application.CommandHandlers
{
    public class CreateMandateCommandHanlder : IRequestHandler<CreateMandateCommand, Common.Models.View.Result<MandateRequest>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateMandateCommandHanlder(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Common.Models.View.Result<MandateRequest>> Handle(CreateMandateCommand request, CancellationToken cancellationToken)
        {
            var mandateRequest = _mapper.Map<MandateRequest>(request);
            mandateRequest.GenerateMandateRefernce();
            await _context.MandateRequests.AddAsync(mandateRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<MandateRequest>.Success(DateTime.UtcNow, mandateRequest);
        }
    }
}