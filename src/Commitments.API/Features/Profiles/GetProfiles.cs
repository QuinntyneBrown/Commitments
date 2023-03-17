// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Profiles;

 public class GetProfilesRequest : IRequest<GetProfilesResponse> { }

 public class GetProfilesResponse
 {
     public IEnumerable<ProfileDto> Profiles { get; set; }
 }

 public class GetProfilesQueryHandler : IRequestHandler<GetProfilesRequest, GetProfilesResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetProfilesQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetProfilesResponse> Handle(GetProfilesRequest request, CancellationToken cancellationToken)
         => new GetProfilesResponse()
         {
             Profiles = await _context.Profiles.Select(x => ProfileDto.FromProfile(x)).ToListAsync()
         };
 }

