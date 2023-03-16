using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.DashboardCards;

 public class GetDashboardCardByIdQueryCommandValidator : AbstractValidator<GetDashboardCardByIdQueryCommandRequest>
 {
     public GetDashboardCardByIdQueryCommandValidator()
     {
         RuleFor(request => request.DashboardCardId).NotEqual(0);
     }
 }

 public class GetDashboardCardByIdQueryCommandRequest : IRequest<GetDashboardCardByIdQueryCommandResponse> {
     public int DashboardCardId { get; set; }
 }

 public class GetDashboardCardByIdQueryCommandResponse
 {
     public DashboardCardApiModel DashboardCard { get; set; }
 }

 public class GetDashboardCardByIdQueryCommandHandler : IRequestHandler<GetDashboardCardByIdQueryCommandRequest, GetDashboardCardByIdQueryCommandResponse>
 {
     public IAppDbContext _context { get; set; }

     public GetDashboardCardByIdQueryCommandHandler(IAppDbContext context) => _context = context;

     public async Task<GetDashboardCardByIdQueryCommandResponse> Handle(GetDashboardCardByIdQueryCommandRequest request, CancellationToken cancellationToken)
         => new GetDashboardCardByIdQueryCommandResponse()
         {
             DashboardCard = DashboardCardApiModel.FromDashboardCard(await _context.DashboardCards.FindAsync(request.DashboardCardId))
         };
 }
