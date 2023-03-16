using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Tags;

 public class GetTagByIdQueryCommandValidator : AbstractValidator<GetTagByIdQueryCommandRequest>
 {
     public GetTagByIdQueryCommandValidator()
     {
         RuleFor(request => request.TagId).NotEqual(default(int));
     }
 }

 public class GetTagByIdQueryCommandRequest : IRequest<GetTagByIdQueryCommandResponse> {
     public int TagId { get; set; }
 }

 public class GetTagByIdQueryCommandResponse
 {
     public TagApiModel Tag { get; set; }
 }

 public class GetTagByIdQueryCommandHandler : IRequestHandler<GetTagByIdQueryCommandRequest, GetTagByIdQueryCommandResponse>
 {
     private readonly IAppDbContext _context;

     public GetTagByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetTagByIdQueryCommandResponse> Handle(GetTagByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetTagByIdQueryCommandResponse()
         {
             Tag = TagApiModel.FromTag(await _context.Tags.FindAsync(request.TagId))
         };
 }
