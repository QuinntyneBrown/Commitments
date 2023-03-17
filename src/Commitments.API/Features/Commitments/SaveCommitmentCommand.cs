using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class SaveCommitmentCommandValidator: AbstractValidator<SaveCommitmentCommandRequest> {
     public SaveCommitmentCommandValidator()
     {
         RuleFor(request => request.Commitment.CommitmentId).NotNull();
     }
 }

 public class SaveCommitmentCommandRequest : IRequest<SaveCommitmentCommandResponse> {
     public CommitmentDto Commitment { get; set; }
 }

 public class SaveCommitmentCommandResponse
 {            
     public int CommitmentId { get; set; }
 }

 public class SaveCommitmentCommandHandler : IRequestHandler<SaveCommitmentCommandRequest, SaveCommitmentCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public SaveCommitmentCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveCommitmentCommandResponse> Handle(SaveCommitmentCommandRequest request, CancellationToken cancellationToken)
     {
         var commitment = await _context.Commitments
             .Include(x => x.CommitmentFrequencies)
             .Include("CommitmentFrequencies.Frequency")
             .SingleOrDefaultAsync(x => x.CommitmentId == request.Commitment.CommitmentId);

         if (commitment == null) _context.Commitments.Add(commitment = new Commitment());

         commitment.BehaviourId = request.Commitment.BehaviourId;
         commitment.ProfileId = request.Commitment.ProfileId;

         commitment.CommitmentFrequencies.Clear();

         foreach (var cf in request.Commitment.CommitmentFrequencies) {                    
             commitment.CommitmentFrequencies.Add(new CommitmentFrequency()
             {
                 FrequencyId = cf.FrequencyId
             });
         }
         await _context.SaveChangesAsync(cancellationToken);

         return new SaveCommitmentCommandResponse() { CommitmentId = commitment.CommitmentId };
     }
 }
