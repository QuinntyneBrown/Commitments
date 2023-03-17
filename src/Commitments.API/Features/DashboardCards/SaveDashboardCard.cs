// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;


namespace Commitments.Api.Features.DashboardCards;

 public class SaveDashboardCardCommandValidator: AbstractValidator<SaveDashboardCardRequest> {
     public SaveDashboardCardCommandValidator()
     {
         RuleFor(request => request.DashboardCard.DashboardCardId).NotNull();
     }
 }

 public class SaveDashboardCardRequest : IRequest<SaveDashboardCardResponse> {
     public DashboardCardDto DashboardCard { get; set; }
 }

 public class SaveDashboardCardResponse
 {            
     public int DashboardCardId { get; set; }
 }

 public class SaveDashboardCardCommandHandler : IRequestHandler<SaveDashboardCardRequest, SaveDashboardCardResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveDashboardCardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveDashboardCardResponse> Handle(SaveDashboardCardRequest request, CancellationToken cancellationToken)
     {
         var dashboard = await _context.Dashboards.Include(x => x.DashboardCards).SingleAsync(x => x.DashboardId == request.DashboardCard.DashboardId);

         var dashboardCard = await _context.DashboardCards.FindAsync(request.DashboardCard.DashboardCardId);

         if (dashboardCard == null) {
             _context.DashboardCards.Add(dashboardCard = new DashboardCard());

             var maxLeft = 1;

             foreach(var dc in dashboard.DashboardCards) {
                 var options = JsonConvert.DeserializeObject<OptionsDto>(dc.Options);

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

         return new SaveDashboardCardResponse() { DashboardCardId = dashboardCard.DashboardCardId };
     }
 }

