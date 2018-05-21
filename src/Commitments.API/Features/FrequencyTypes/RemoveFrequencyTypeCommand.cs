using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.FrequencyTypes
{
    public class RemoveFrequencyTypeCommand
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.FrequencyType.FrequencyTypeId).NotEqual(0);
            }
        }

        public class Request : IRequest
        {
            public FrequencyType FrequencyType { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                _context.FrequencyTypes.Remove(await _context.FrequencyTypes.FindAsync(request.FrequencyType.FrequencyTypeId));
                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
