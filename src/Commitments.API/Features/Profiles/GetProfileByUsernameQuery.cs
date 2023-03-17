using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Profiles;

 public class GetProfileByUsernameQueryRequest : IRequest<GetProfileByUsernameQueryResponse> {
     public string Username { get; set; }
 }

 public class GetProfileByUsernameQueryResponse
 {
     public ProfileDto Profile { get; set; }
 }

 public class GetProfileByUsernameQueryHandler : IRequestHandler<GetProfileByUsernameQueryRequest, GetProfileByUsernameQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetProfileByUsernameQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetProfileByUsernameQueryResponse> Handle(GetProfileByUsernameQueryRequest request, CancellationToken cancellationToken) 
         => new GetProfileByUsernameQueryResponse()
         {
             Profile = ProfileDto.FromProfile(await _context.Profiles.SingleAsync(x => x.User.Username == request.Username))
         };
 }
