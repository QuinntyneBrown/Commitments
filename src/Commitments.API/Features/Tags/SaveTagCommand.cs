using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using Commitments.Core.Interfaces;
using Commitments.Core.Entities;
using Commitments.Core.Extensions;


namespace Commitments.Api.Features.Tags;

 public class SaveTagCommandValidator: AbstractValidator<SaveTagCommandRequest> {
     public SaveTagCommandValidator()
     {
         RuleFor(request => request.Tag.TagId).NotNull();
     }
 }

 public class SaveTagCommandRequest : IRequest<SaveTagCommandResponse> {
     public TagApiModel Tag { get; set; }
 }

 public class SaveTagCommandResponse
 {            
     public int TagId { get; set; }
 }

 public class SaveTagCommandHandler : IRequestHandler<SaveTagCommandRequest, SaveTagCommandResponse>
 {
     private readonly IAppDbContext _context;

     public SaveTagCommandHandler(IAppDbContext context) => _context = context;

     public async Task<SaveTagCommandResponse> Handle(SaveTagCommandRequest request, CancellationToken cancellationToken)
     {
         var tag = await _context.Tags.FindAsync(request.Tag.TagId);

         if (tag == null) _context.Tags.Add(tag = new Tag());

         tag.Name = request.Tag.Name;

         tag.Slug = request.Tag.Name.GenerateSlug();

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveTagCommandResponse() { TagId = tag.TagId };
     }
 }
