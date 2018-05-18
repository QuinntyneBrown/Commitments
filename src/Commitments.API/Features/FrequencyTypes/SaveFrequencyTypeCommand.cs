using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.FrequencyTypes
{
    public class SaveFrequencyTypeCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.FrequencyType.FrequencyTypeId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public FrequencyTypeApiModel FrequencyType { get; set; }
        }

        public class Response
        {			
            public int FrequencyTypeId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var frequencyType = await _context.FrequencyTypes.FindAsync(request.FrequencyType.FrequencyTypeId);

                if (frequencyType == null) _context.FrequencyTypes.Add(frequencyType = new FrequencyType());

                frequencyType.Name = request.FrequencyType.Name;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { FrequencyTypeId = frequencyType.FrequencyTypeId };
            }
        }
    }
}
