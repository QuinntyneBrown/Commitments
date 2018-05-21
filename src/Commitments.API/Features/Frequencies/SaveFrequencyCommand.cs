using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Commitments.API.Features.Frequencies
{
    public class SaveFrequencyCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.Frequency.FrequencyId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public FrequencyApiModel Frequency { get; set; }
        }

        public class Response
        {        	
            public int FrequencyId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
        	public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var frequency = await _context.Frequencies
                    .Include(x => x.FrequencyType)
                    .SingleOrDefaultAsync(x => x.FrequencyId == request.Frequency.FrequencyId);

                if (frequency == null) _context.Frequencies.Add(frequency = new Frequency());

                frequency.Frequency = request.Frequency.Frequency;

                frequency.FrequencyTypeId = request.Frequency.FrequencyTypeId;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { FrequencyId = frequency.FrequencyId };
            }
        }
    }
}
