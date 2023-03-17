// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Activities;

 public class GetActivityByIdValidator : AbstractValidator<GetActivityByIdRequest>
 {
     public GetActivityByIdValidator()
     {
         RuleFor(request => request.ActivityId).NotEqual(0);
     }
 }

 public class GetActivityByIdRequest : IRequest<GetActivityByIdResponse> {
     public int ActivityId { get; set; }
 }

 public class GetActivityByIdResponse
 {
     public ActivityDto Activity { get; set; }
 }

 public class GetActivityByIdHandler : IRequestHandler<GetActivityByIdRequest, GetActivityByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetActivityByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetActivityByIdResponse> Handle(GetActivityByIdRequest request, CancellationToken cancellationToken)
         => new GetActivityByIdResponse()
         {
             Activity = ActivityDto.FromActivity(await _context.Activities
                 .Include(x => x.Behaviour)
                 .Include("Behaviour.BehaviourType")
                 .SingleAsync(x => x.ActivityId == request.ActivityId))
         };
 }

