// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.DashboardCards;

 public class GetDashboardCardsRequest : IRequest<GetDashboardCardsResponse> { }

 public class GetDashboardCardsResponse
 {
     public IEnumerable<DashboardCardDto> DashboardCards { get; set; }
 }

 public class GetDashboardCardsQueryHandler : IRequestHandler<GetDashboardCardsRequest, GetDashboardCardsResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDashboardCardsQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDashboardCardsResponse> Handle(GetDashboardCardsRequest request, CancellationToken cancellationToken)
         => new GetDashboardCardsResponse()
         {
             DashboardCards = await _context.DashboardCards.Select(x => DashboardCardDto.FromDashboardCard(x)).ToListAsync()
         };
 }

