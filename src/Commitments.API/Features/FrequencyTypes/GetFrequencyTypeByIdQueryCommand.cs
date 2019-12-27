using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;

namespace Commitments.Api.Features.FrequencyTypes
{
    public class GetFrequencyTypeByIdQuery
    {
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.FrequencyTypeId).NotEqual(0);
            }
        }

        public class Request : IRequest<Response> {
            public int FrequencyTypeId { get; set; }
        }

        public class Response
        {
            public FrequencyTypeApiModel FrequencyType { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
                => new Response()
                {
                    FrequencyType = FrequencyTypeApiModel.FromFrequencyType(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId))
                };
        }
    }
}
