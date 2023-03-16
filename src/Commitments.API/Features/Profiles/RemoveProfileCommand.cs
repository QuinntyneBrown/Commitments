using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Profiles;

 public class RemoveProfileCommandValidator : AbstractValidator<RemoveProfileCommandRequest>
 {
     public RemoveProfileCommandValidator()
     {
         RuleFor(request => request.ProfileId).NotEqual(0);
     }
 }

 public class RemoveProfileCommandRequest : IRequest<RemoveProfileCommandResponse>
 {
     public int ProfileId { get; set; }
 }

 public class RemoveProfileCommandResponse { }

 public class RemoveProfileCommandHandler : IRequestHandler<RemoveProfileCommandRequest, RemoveProfileCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public RemoveProfileCommandHandler(IAppDbContext context) => _context = context;

     public async Task<RemoveProfileCommandResponse> Handle(RemoveProfileCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Profiles.Remove(await _context.Profiles.FindAsync(request.ProfileId));
         await _context.SaveChangesAsync(cancellationToken);
         return new RemoveProfileCommandResponse()
         {

         };
     }

 }
