using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Tags;

 public class GetTagBySlugQueryRequest : IRequest<GetTagBySlugQueryResponse> {
     public string Slug { get; set; }
 }

 public class GetTagBySlugQueryResponse
 {
     public TagDto Tag { get; set; }
 }

 public class GetTagBySlugQueryHandler : IRequestHandler<GetTagBySlugQueryRequest, GetTagBySlugQueryResponse>
 {
     private readonly ICommimentsDbContext _context;

     public GetTagBySlugQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetTagBySlugQueryResponse> Handle(GetTagBySlugQueryRequest request, CancellationToken cancellationToken)
         => new GetTagBySlugQueryResponse()
         {
             Tag = TagDto.FromTag(await _context.Tags
                 .Include(x =>x.NoteTags)
                 .Include("NoteTags.Note")
                 .Where(x => x.Slug == request.Slug)
                 .SingleAsync())
         };
 }
