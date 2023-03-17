using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Tags;

 public class GetTagsQueryRequest : IRequest<GetTagsQueryResponse> { }

 public class GetTagsQueryResponse
 {
     public IEnumerable<TagDto> Tags { get; set; }
 }

 public class GetTagsQueryHandler : IRequestHandler<GetTagsQueryRequest, GetTagsQueryResponse>
 {
     private readonly IAppDbContext _context;

     public GetTagsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetTagsQueryResponse> Handle(GetTagsQueryRequest request, CancellationToken cancellationToken)
         => new GetTagsQueryResponse()
         {
             Tags = await _context.Tags.Select(x => TagDto.FromTag(x)).ToListAsync()
         };
 }
