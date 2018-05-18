using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;

namespace Commitments.API.Features.CommitmentFrequencies
{
    public class SaveCommitmentFrequencyCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.CommitmentFrequency.CommitmentFrequencyId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public CommitmentFrequencyApiModel CommitmentFrequency { get; set; }
        }

        public class Response
        {			
            public int CommitmentFrequencyId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
			public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var commitmentFrequency = await _context.CommitmentFrequencies.FindAsync(request.CommitmentFrequency.CommitmentFrequencyId);

                if (commitmentFrequency == null) _context.CommitmentFrequencies.Add(commitmentFrequency = new CommitmentFrequency());

                commitmentFrequency.Frequency = request.CommitmentFrequency.Frequency;
                commitmentFrequency.CommitmentId = request.CommitmentFrequency.CommitmentId;
                commitmentFrequency.IsDesirable = request.CommitmentFrequency.IsDesirable;
                commitmentFrequency.FrequencyTypeId = request.CommitmentFrequency.FrequencyTypeId;

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { CommitmentFrequencyId = commitmentFrequency.CommitmentFrequencyId };
            }
        }
    }
}
