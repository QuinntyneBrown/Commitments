using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Profiles;

 public class SaveAvatarRequest : IRequest<SaveAvatarResponse> {

     public int ProfileId { get; set; }
     public string AvatarUrl { get; set; }
 }

 public class SaveAvatarResponse
 {
     public int ProfileId { get;set; }
 }

 public class SaveAvatarCommandHandler : IRequestHandler<SaveAvatarRequest, SaveAvatarResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public SaveAvatarCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveAvatarResponse> Handle(SaveAvatarRequest request, CancellationToken cancellationToken) {
         var profile = _context.Profiles.Find(request.ProfileId);
         profile.AvatarUrl = request.AvatarUrl;
         await _context.SaveChangesAsync(cancellationToken);
         return new SaveAvatarResponse() {
             ProfileId = profile.ProfileId
         };
     }
 }
