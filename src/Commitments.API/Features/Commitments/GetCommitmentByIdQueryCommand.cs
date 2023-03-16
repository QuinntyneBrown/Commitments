using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Commitments;

 public class GetCommitmentByIdQueryCommandValidator : AbstractValidator<GetCommitmentByIdQueryCommandRequest>
 {
     public GetCommitmentByIdQueryCommandValidator()
     {
         RuleFor(request => request.CommitmentId).NotEqual(0);
     }
 }

 public class GetCommitmentByIdQueryCommandRequest : IRequest<GetCommitmentByIdQueryCommandResponse> {
     public int CommitmentId { get; set; }
 }

 public class GetCommitmentByIdQueryCommandResponse
 {
     public CommitmentApiModel Commitment { get; set; }
 }

 public class GetCommitmentByIdQueryCommandHandler : IRequestHandler<GetCommitmentByIdQueryCommandRequest, GetCommitmentByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetCommitmentByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetCommitmentByIdQueryCommandResponse> Handle(GetCommitmentByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetCommitmentByIdQueryCommandResponse()
         {
             Commitment = CommitmentApiModel.FromCommitment(await _context.Commitments.FindAsync(request.CommitmentId))
         };
 }
