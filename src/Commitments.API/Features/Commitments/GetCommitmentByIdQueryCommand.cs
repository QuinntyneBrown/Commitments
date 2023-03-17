using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Commitments;

 public class GetCommitmentByIdQueryCommandValidator : AbstractValidator<GetCommitmentByIdQueryRequest>
 {
     public GetCommitmentByIdQueryCommandValidator()
     {
         RuleFor(request => request.CommitmentId).NotEqual(0);
     }
 }

 public class GetCommitmentByIdQueryRequest : IRequest<GetCommitmentByIdQueryResponse> {
     public int CommitmentId { get; set; }
 }

 public class GetCommitmentByIdQueryResponse
 {
     public CommitmentDto Commitment { get; set; }
 }

 public class GetCommitmentByIdQueryCommandHandler : IRequestHandler<GetCommitmentByIdQueryRequest, GetCommitmentByIdQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetCommitmentByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetCommitmentByIdQueryResponse> Handle(GetCommitmentByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetCommitmentByIdQueryResponse()
         {
             Commitment = CommitmentDto.FromCommitment(await _context.Commitments.FindAsync(request.CommitmentId))
         };
 }
