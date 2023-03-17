// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Profiles;

 public class RemoveProfileCommandValidator : AbstractValidator<RemoveProfileRequest>
 {
     public RemoveProfileCommandValidator()
     {
         RuleFor(request => request.ProfileId).NotEqual(0);
     }
 }

 public class RemoveProfileRequest : IRequest<RemoveProfileResponse>
 {
     public int ProfileId { get; set; }
 }

 public class RemoveProfileResponse { }

 public class RemoveProfileCommandHandler : IRequestHandler<RemoveProfileRequest, RemoveProfileResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public RemoveProfileCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<RemoveProfileResponse> Handle(RemoveProfileRequest request, CancellationToken cancellationToken)
     {
         _context.Profiles.Remove(await _context.Profiles.FindAsync(request.ProfileId));
         await _context.SaveChangesAsync(cancellationToken);
         return new RemoveProfileResponse()
         {

         };
     }

 }

