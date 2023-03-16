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
     public TagApiModel Tag { get; set; }
 }

 public class GetTagBySlugQueryHandler : IRequestHandler<GetTagBySlugQueryRequest, GetTagBySlugQueryResponse>
 {
     private readonly IAppDbContext _context;

     public GetTagBySlugQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetTagBySlugQueryResponse> Handle(GetTagBySlugQueryRequest request, CancellationToken cancellationToken)
         => new GetTagBySlugQueryResponse()
         {
             Tag = TagApiModel.FromTag(await _context.Tags
                 .Include(x =>x.NoteTags)
                 .Include("NoteTags.Note")
                 .Where(x => x.Slug == request.Slug)
                 .SingleAsync())
         };
 }
