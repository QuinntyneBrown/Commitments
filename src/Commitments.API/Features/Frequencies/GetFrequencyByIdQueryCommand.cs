using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.Api.Features.Frequencies
{
    public class GetFrequencyByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.FrequencyId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int FrequencyId { get; set; }
        }

        public class Response
        {
            public FrequencyApiModel Frequency { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    Frequency = FrequencyApiModel.FromFrequency(await _context.Frequencies.FindAsync(request.FrequencyId))
                };
        }
    }
}
