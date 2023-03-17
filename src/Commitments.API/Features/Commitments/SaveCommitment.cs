// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Commitments;

 public class SaveCommitmentCommandValidator: AbstractValidator<SaveCommitmentRequest> {
     public SaveCommitmentCommandValidator()
     {
         RuleFor(request => request.Commitment.CommitmentId).NotNull();
     }
 }

 public class SaveCommitmentRequest : IRequest<SaveCommitmentResponse> {
     public CommitmentDto Commitment { get; set; }
 }

 public class SaveCommitmentResponse
 {            
     public int CommitmentId { get; set; }
 }

 public class SaveCommitmentCommandHandler : IRequestHandler<SaveCommitmentRequest, SaveCommitmentResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveCommitmentCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveCommitmentResponse> Handle(SaveCommitmentRequest request, CancellationToken cancellationToken)
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

         return new SaveCommitmentResponse() { CommitmentId = commitment.CommitmentId };
     }
 }

