using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Tags;

 public class GetTagByIdValidator : AbstractValidator<GetTagByIdRequest>
 {
     public GetTagByIdValidator()
     {
         RuleFor(request => request.TagId).NotEqual(default(int));
     }
 }

 public class GetTagByIdRequest : IRequest<GetTagByIdResponse> {
     public int TagId { get; set; }
 }

 public class GetTagByIdResponse
 {
     public TagDto Tag { get; set; }
 }

 public class GetTagByIdHandler : IRequestHandler<GetTagByIdRequest, GetTagByIdResponse>
 {
     private readonly ICommimentsDbContext _context;

     public GetTagByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetTagByIdResponse> Handle(GetTagByIdRequest request, CancellationToken cancellationToken)
         => new GetTagByIdResponse()
         {
             Tag = TagDto.FromTag(await _context.Tags.FindAsync(request.TagId))
         };
 }
