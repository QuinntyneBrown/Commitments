using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Commitments.Core.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Commitments.Core.Exceptions;
using System.Security.Claims;
using System.Collections.Generic;


namespace Commitments.Api.Features.Users;

 public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommandRequest>
 {
     public AuthenticateCommandValidator()
     {
         RuleFor(request => request.Username).NotEqual(default(string));
         RuleFor(request => request.Password).NotEqual(default(string));
     }            
 }

 public class AuthenticateCommandRequest : IRequest<AuthenticateCommandResponse>
 {
     public string Username { get; set; }
     public string Password { get; set; }
 }

 public class AuthenticateCommandResponse
 {
     public string AccessToken { get; set; }
     public int UserId { get; set; }
 }

 public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommandRequest, AuthenticateCommandResponse>
 {
     private readonly IAppDbContext _context;
     private readonly IPasswordHasher _passwordHasher;
     private readonly ITokenProvider _tokenProvider;

     public AuthenticateCommandHandler(IAppDbContext context, ITokenProvider tokenProvider, IPasswordHasher passwordHasher)
     {
         _context = context;
         _tokenProvider = tokenProvider;
         _passwordHasher = passwordHasher;
     }

     public async Task<AuthenticateCommandResponse> Handle(AuthenticateCommandRequest request, CancellationToken cancellationToken)
     {
         var user = await _context.Users
             .SingleOrDefaultAsync(x => x.Username.ToLower() == request.Username.ToLower());

         if (user == null)
             throw new DomainException();

         if (!ValidateUser(user, _passwordHasher.HashPassword(user.Salt, request.Password)))
             throw new DomainException();

         var profile = await _context.Profiles.Include(x => x.User).SingleAsync(x => x.User.Username == request.Username);

         return new AuthenticateCommandResponse()
         {
             AccessToken = _tokenProvider.Get(request.Username, new List<Claim>() { new Claim("ProfileId", $"{profile.ProfileId}") }),
             UserId = user.UserId
         };
     }

     public bool ValidateUser(User user, string transformedPassword)
     {
         if (user == null || transformedPassword == null)
             return false;

         return user.Password == transformedPassword;
     }
 }
