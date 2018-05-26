using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Entities;
using Commitments.Core.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;

namespace Commitments.API.Features.DashboardCards
{
    public class SaveDashboardCardCommand
    {
        public class Validator: AbstractValidator<Request> {
            public Validator()
            {
                RuleFor(request => request.DashboardCard.DashboardCardId).NotNull();
            }
        }

        public class Request : IRequest<Response> {
            public DashboardCardApiModel DashboardCard { get; set; }
        }

        public class Response
        {            
            public int DashboardCardId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public IAppDbContext _context { get; set; }
            
            public Handler(IAppDbContext context) => _context = context;

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var dashboard = await _context.Dashboards.Include(x => x.DashboardCards).SingleAsync(x => x.DashboardId == request.DashboardCard.DashboardId);

                var dashboardCard = await _context.DashboardCards.FindAsync(request.DashboardCard.DashboardCardId);

                if (dashboardCard == null) {
                    _context.DashboardCards.Add(dashboardCard = new DashboardCard());

                    var maxLeft = 1;

                    foreach(var dc in dashboard.DashboardCards) {
                        var options = JsonConvert.DeserializeObject<DashboardCardApiModel.OptionsApiModel>(dc.Options);

                        if ((options.Left + options.Width) > maxLeft)
                            maxLeft = options.Left + options.Width;
                    }
                    
                    request.DashboardCard.Options.Left = maxLeft;

                }

                dashboardCard.DashboardCardId = request.DashboardCard.DashboardCardId;
                dashboardCard.DashboardId = request.DashboardCard.DashboardId;
                dashboardCard.CardId = request.DashboardCard.CardId;
                dashboardCard.CardLayoutId = request.DashboardCard.CardLayoutId;
                dashboardCard.Options = JsonConvert.SerializeObject(request.DashboardCard.Options);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response() { DashboardCardId = dashboardCard.DashboardCardId };
            }
        }
    }
}
