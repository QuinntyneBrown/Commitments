// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Profiles;

 public class GetProfileByIdValidator : AbstractValidator<GetProfileByIdRequest>
 {
     public GetProfileByIdValidator()
     {
         RuleFor(request => request.ProfileId).NotEqual(0);
     }
 }

 public class GetProfileByIdRequest : IRequest<GetProfileByIdResponse> {
     public int ProfileId { get; set; }
 }

 public class GetProfileByIdResponse
 {
     public ProfileDto Profile { get; set; }
 }

 public class GetProfileByIdHandler : IRequestHandler<GetProfileByIdRequest, GetProfileByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetProfileByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request, CancellationToken cancellationToken)
         => new GetProfileByIdResponse()
         {
             Profile = ProfileDto.FromProfile(await _context.Profiles.FindAsync(request.ProfileId))
         };
 }

