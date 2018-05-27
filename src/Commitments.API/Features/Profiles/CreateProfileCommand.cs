using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.Core.Entities;
using Commitments.Core.Identity;

namespace Commitments.API.Features.Profiles
{
    public class CreateProfileCommand
    {
        public class Request : IRequest<Response> {

            public string Username { get; set; }
            public string Name { get; set; }
            public string AvatarUrl { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public class Response
        {
            public int ProfileId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public IPasswordHasher _passwordHasher { get; set; }
            public Handler(IAppDbContext context, IPasswordHasher passwordHasher) {
                _context = context;
                _passwordHasher = passwordHasher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {

                var profile = new Profile() { Name = request.Name, AvatarUrl = request.AvatarUrl };
                profile.User = new User() { Username = request.Username };
                profile.User.Password = _passwordHasher.HashPassword(profile.User.Salt, request.Password);                                
                _context.Profiles.Add(profile);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { ProfileId = profile.ProfileId };
            }
        }
    }
}
