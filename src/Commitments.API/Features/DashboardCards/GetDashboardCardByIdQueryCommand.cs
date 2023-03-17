using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.DashboardCards;

 public class GetDashboardCardByIdQueryCommandValidator : AbstractValidator<GetDashboardCardByIdQueryRequest>
 {
     public GetDashboardCardByIdQueryCommandValidator()
     {
         RuleFor(request => request.DashboardCardId).NotEqual(0);
     }
 }

 public class GetDashboardCardByIdQueryRequest : IRequest<GetDashboardCardByIdQueryResponse> {
     public int DashboardCardId { get; set; }
 }

 public class GetDashboardCardByIdQueryResponse
 {
     public DashboardCardDto DashboardCard { get; set; }
 }

 public class GetDashboardCardByIdQueryCommandHandler : IRequestHandler<GetDashboardCardByIdQueryRequest, GetDashboardCardByIdQueryResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetDashboardCardByIdQueryCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetDashboardCardByIdQueryResponse> Handle(GetDashboardCardByIdQueryRequest request, CancellationToken cancellationToken)
         => new GetDashboardCardByIdQueryResponse()
         {
             DashboardCard = DashboardCardDto.FromDashboardCard(await _context.DashboardCards.FindAsync(request.DashboardCardId))
         };
 }
