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
     public ProfileApiModel Profile { get; set; }
 }

 public class GetProfileByUsernameQueryHandler : IRequestHandler<GetProfileByUsernameQueryRequest, GetProfileByUsernameQueryResponse>
 {
     public IAppDbContext _context { get; set; }
     public GetProfileByUsernameQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetProfileByUsernameQueryResponse> Handle(GetProfileByUsernameQueryRequest request, CancellationToken cancellationToken) 
         => new GetProfileByUsernameQueryResponse()
         {
             Profile = ProfileApiModel.FromProfile(await _context.Profiles.SingleAsync(x => x.User.Username == request.Username))
         };
 }
