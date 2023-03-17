using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Cards;

 public class GetCardsQueryRequest : IRequest<GetCardsQueryResponse> { }

 public class GetCardsQueryResponse
 {
     public IEnumerable<CardDto> Cards { get; set; }
 }

 public class GetCardsQueryHandler : IRequestHandler<GetCardsQueryRequest, GetCardsQueryResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetCardsQueryHandler(IAppDbContext context) => _context = context;

     public async Task<GetCardsQueryResponse> Handle(GetCardsQueryRequest request, CancellationToken cancellationToken)
         => new GetCardsQueryResponse()
         {
             Cards = await _context.Cards.Select(x => CardDto.FromCard(x)).ToListAsync()
         };
 }
