using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.DashboardCards;

 public class SaveDashboardCardRangeCommandRequest : IRequest<SaveDashboardCardRangeCommandResponse> {
     public IEnumerable<DashboardCardDto> DashboardCards { get; set; }
 }

 public class SaveDashboardCardRangeCommandResponse
 {
     public IEnumerable<int> DashboardCardIds { get; set; }
 }

 public class SaveDashboardCardRangeCommandHandler : IRequestHandler<SaveDashboardCardRangeCommandRequest, SaveDashboardCardRangeCommandResponse>
 {
     public IAppDbContext _context { get; set; }
     public IMediator _mediator { get; set; }
     public SaveDashboardCardRangeCommandHandler(IAppDbContext context, IMediator mediator) {
         _context = context;
         _mediator = mediator;
     }

     public async Task<SaveDashboardCardRangeCommandResponse> Handle(SaveDashboardCardRangeCommandRequest request, CancellationToken cancellationToken)
     {
         var dashboardCardIds = new List<int>();

         foreach(var dashboardCard in request.DashboardCards)
         {                    
             var response = await _mediator.Send(new SaveDashboardCardCommandRequest() { DashboardCard = dashboardCard });
             dashboardCardIds.Add(response.DashboardCardId);
         }

         return new SaveDashboardCardRangeCommandResponse()
         {
             DashboardCardIds = dashboardCardIds
         };
     }

 }
