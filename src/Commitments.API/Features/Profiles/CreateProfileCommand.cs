using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.Core.Entities;
using Commitments.Core.Identity;


namespace Commitments.Api.Features.Profiles;

 public class CreateProfileCommandRequest : IRequest<CreateProfileCommandResponse> {

     public string Username { get; set; }
     public string Name { get; set; }
     public string AvatarUrl { get; set; }
     public string Password { get; set; }
     public string ConfirmPassword { get; set; }
 }

 public class CreateProfileCommandResponse
 {
     public int ProfileId { get; set; }
 }

 public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommandRequest, CreateProfileCommandResponse>
 {
     public IAppDbContext _context { get; set; }
     public IPasswordHasher _passwordHasher { get; set; }
     public CreateProfileCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher) {
         _context = context;
         _passwordHasher = passwordHasher;
     }

     public async Task<CreateProfileCommandResponse> Handle(CreateProfileCommandRequest request, CancellationToken cancellationToken) {

         var profile = new Profile() { Name = request.Name, AvatarUrl = request.AvatarUrl };
         profile.User = new User() { Username = request.Username };
         profile.User.Password = _passwordHasher.HashPassword(profile.User.Salt, request.Password);                                
         _context.Profiles.Add(profile);

         await _context.SaveChangesAsync(cancellationToken);

         return new CreateProfileCommandResponse() { ProfileId = profile.ProfileId };
     }
 }
