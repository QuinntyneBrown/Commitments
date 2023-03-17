using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Profiles;

 public class GetProfileByUsernameRequest : IRequest<GetProfileByUsernameResponse> {
     public string Username { get; set; }
 }

 public class GetProfileByUsernameResponse
 {
     public ProfileDto Profile { get; set; }
 }

 public class GetProfileByUsernameQueryHandler : IRequestHandler<GetProfileByUsernameRequest, GetProfileByUsernameResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetProfileByUsernameQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetProfileByUsernameResponse> Handle(GetProfileByUsernameRequest request, CancellationToken cancellationToken) 
         => new GetProfileByUsernameResponse()
         {
             Profile = ProfileDto.FromProfile(await _context.Profiles.SingleAsync(x => x.User.Username == request.Username))
         };
 }
