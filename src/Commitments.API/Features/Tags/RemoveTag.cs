using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Tags;

 public class RemoveTagCommandValidator : AbstractValidator<RemoveTagRequest>
 {
     public RemoveTagCommandValidator()
     {
         RuleFor(request => request.TagId).NotEqual(0);
     }
 }
 public class RemoveTagRequest : IRequest<RemoveTagResponse>
 {
     public int TagId { get; set; }
 }

 public class RemoveTagResponse { }

 public class RemoveTagCommandHandler : IRequestHandler<RemoveTagRequest, RemoveTagResponse>
 {
     private readonly ICommimentsDbContext _context;

     public RemoveTagCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<RemoveTagResponse> Handle(RemoveTagRequest request, CancellationToken cancellationToken)
     {
         _context.Tags.Remove(await _context.Tags.FindAsync(request.TagId));
         await _context.SaveChangesAsync(cancellationToken);
         return new RemoveTagResponse() { };
     }
 }
