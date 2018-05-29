using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.Core.Identity;
using System;

namespace Commitments.API.Features.Users
{
    public class ChangePasswordCommand
    {
        public class Request : IRequest<Response> {
            public int ProfileId { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }

        }

        public class Response { }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            public IPasswordHasher _passwordHasher { get; set; }
            public Handler(IAppDbContext context, IPasswordHasher passwordHasher)
            {
                _context = context;
                _passwordHasher = passwordHasher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {
                var user = await _context.Profiles
                    .Include(x => x.User)
                    .Where(x => x.ProfileId == request.ProfileId)
                    .Select(x => x.User)
                    .SingleAsync();

                if (user.Password != _passwordHasher.HashPassword(user.Salt, request.OldPassword))
                    throw new Exception();

                user.Password = _passwordHasher.HashPassword(user.Salt, request.NewPassword);

                await _context.SaveChangesAsync(cancellationToken);

			    return new Response() { };
            }
        }
    }
}
