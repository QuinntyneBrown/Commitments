using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.Core.AggregateModel;
using Commitments.Core.Identity;


namespace Commitments.Api.Features.Profiles;

 public class CreateProfileRequest : IRequest<CreateProfileResponse> {

     public string Username { get; set; }
     public string Name { get; set; }
     public string AvatarUrl { get; set; }
     public string Password { get; set; }
     public string ConfirmPassword { get; set; }
 }

 public class CreateProfileResponse
 {
     public int ProfileId { get; set; }
 }

 public class CreateProfileCommandHandler : IRequestHandler<CreateProfileRequest, CreateProfileResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public IPasswordHasher _passwordHasher { get; set; }
     public CreateProfileCommandHandler(ICommimentsDbContext context, IPasswordHasher passwordHasher) {
         _context = context;
         _passwordHasher = passwordHasher;
     }

     public async Task<CreateProfileResponse> Handle(CreateProfileRequest request, CancellationToken cancellationToken) {

         var profile = new Profile() { Name = request.Name, AvatarUrl = request.AvatarUrl };
         profile.User = new User() { Username = request.Username };
         profile.User.Password = _passwordHasher.HashPassword(profile.User.Salt, request.Password);                                
         _context.Profiles.Add(profile);

         await _context.SaveChangesAsync(cancellationToken);

         return new CreateProfileResponse() { ProfileId = profile.ProfileId };
     }
 }
