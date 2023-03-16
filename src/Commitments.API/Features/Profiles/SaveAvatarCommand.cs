using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Profiles;

 public class SaveAvatarCommandRequest : IRequest<SaveAvatarCommandResponse> {

     public int ProfileId { get; set; }
     public string AvatarUrl { get; set; }
 }

 public class SaveAvatarCommandResponse
 {
     public int ProfileId { get;set; }
 }

 public class SaveAvatarCommandHandler : IRequestHandler<SaveAvatarCommandRequest, SaveAvatarCommandResponse>
 {
     public IAppDbContext _context { get; set; }
     public SaveAvatarCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveAvatarCommandResponse> Handle(SaveAvatarCommandRequest request, CancellationToken cancellationToken) {
         var profile = _context.Profiles.Find(request.ProfileId);
         profile.AvatarUrl = request.AvatarUrl;
         await _context.SaveChangesAsync(cancellationToken);
         return new SaveAvatarCommandResponse() {
             ProfileId = profile.ProfileId
         };
     }
 }
