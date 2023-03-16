using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Profiles;

 public class GetProfilesQueryRequest : IRequest<GetProfilesQueryResponse> { }

 public class GetProfilesQueryResponse
 {
     public IEnumerable<ProfileApiModel> Profiles { get; set; }
 }

 public class GetProfilesQueryHandler : IRequestHandler<GetProfilesQueryRequest, GetProfilesQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetProfilesQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetProfilesQueryResponse> Handle(GetProfilesQueryRequest request, CancellationToken cancellationToken)
         => new GetProfilesQueryResponse()
         {
             Profiles = await _context.Profiles.Select(x => ProfileApiModel.FromProfile(x)).ToListAsync()
         };
 }
