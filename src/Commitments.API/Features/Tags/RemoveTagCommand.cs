using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Tags;

 public class RemoveTagCommandValidator : AbstractValidator<RemoveTagCommandRequest>
 {
     public RemoveTagCommandValidator()
     {
         RuleFor(request => request.TagId).NotEqual(0);
     }
 }
 public class RemoveTagCommandRequest : IRequest<RemoveTagCommandResponse>
 {
     public int TagId { get; set; }
 }

 public class RemoveTagCommandResponse { }

 public class RemoveTagCommandHandler : IRequestHandler<RemoveTagCommandRequest, RemoveTagCommandResponse>
 {
     private readonly ICommimentsDbContext _context;

     public RemoveTagCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<RemoveTagCommandResponse> Handle(RemoveTagCommandRequest request, CancellationToken cancellationToken)
     {
         _context.Tags.Remove(await _context.Tags.FindAsync(request.TagId));
         await _context.SaveChangesAsync(cancellationToken);
         return new RemoveTagCommandResponse() { };
     }
 }
