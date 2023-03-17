using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Tags;

 public class GetTagByIdQueryCommandValidator : AbstractValidator<GetTagByIdQueryRequest>
 {
     public GetTagByIdQueryCommandValidator()
     {
         RuleFor(request => request.TagId).NotEqual(default(int));
     }
 }

 public class GetTagByIdQueryRequest : IRequest<GetTagByIdQueryResponse> {
     public int TagId { get; set; }
 }

 public class GetTagByIdQueryResponse
 {
     public TagDto Tag { get; set; }
 }

 public class GetTagByIdQueryCommandHandler : IRequestHandler<GetTagByIdQueryRequest, GetTagByIdQueryResponse>
 {
     private readonly ICommimentsDbContext _context;

     public GetTagByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetTagByIdQueryResponse> Handle(GetTagByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetTagByIdQueryResponse()
         {
             Tag = TagDto.FromTag(await _context.Tags.FindAsync(request.TagId))
         };
 }
