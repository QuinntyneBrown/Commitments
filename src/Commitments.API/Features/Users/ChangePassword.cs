// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Commitments.Core.Identity;
using System;


namespace Commitments.Api.Features.Users;

 public class ChangePasswordRequest : IRequest<ChangePasswordResponse> {
     public int ProfileId { get; set; }
     public string OldPassword { get; set; }
     public string NewPassword { get; set; }

 }

 public class ChangePasswordResponse { }

 public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public IPasswordHasher _passwordHasher { get; set; }
     public ChangePasswordCommandHandler(ICommimentsDbContext context, IPasswordHasher passwordHasher)
     {
         _context = context;
         _passwordHasher = passwordHasher;
     }

     public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken) {

         var user = await _context.Profiles
             .Include(x => x.User)
             .Where(x => x.ProfileId == request.ProfileId)
             .Select(x => x.User)
             .SingleAsync();

         if (user.Password != _passwordHasher.HashPassword(user.Salt, request.OldPassword))
             throw new Exception();

         user.Password = _passwordHasher.HashPassword(user.Salt, request.NewPassword);

         await _context.SaveChangesAsync(cancellationToken);

         return new();

     }
 }

