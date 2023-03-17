// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Frequencies;

 public class RemoveFrequencyCommandValidator : AbstractValidator<RemoveFrequencyRequest>
 {
     public RemoveFrequencyCommandValidator()
     {
         RuleFor(request => request.FrequencyId).NotEqual(0);
     }
 }

 public class RemoveFrequencyRequest : IRequest
 {
     public int FrequencyId { get; set; }
 }

 public class RemoveFrequencyCommandHandler : IRequestHandler<RemoveFrequencyRequest>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveFrequencyCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task Handle(RemoveFrequencyRequest request, CancellationToken cancellationToken)
     {
         _context.Frequencies.Remove(await _context.Frequencies.FindAsync(request.FrequencyId));
         await _context.SaveChangesAsync(cancellationToken);
     }

 }

