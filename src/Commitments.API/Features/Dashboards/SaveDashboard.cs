// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.Dashboards;

 public class SaveDashboardCommandValidator: AbstractValidator<SaveDashboardRequest> {
     public SaveDashboardCommandValidator()
     {
         RuleFor(request => request.Dashboard.DashboardId).NotNull();
     }
 }

 public class SaveDashboardRequest : IRequest<SaveDashboardResponse> {
     public DashboardDto Dashboard { get; set; }
 }

 public class SaveDashboardResponse
 {            
     public int DashboardId { get; set; }
 }

 public class SaveDashboardCommandHandler : IRequestHandler<SaveDashboardRequest, SaveDashboardResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveDashboardCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveDashboardResponse> Handle(SaveDashboardRequest request, CancellationToken cancellationToken)
     {
         var dashboard = await _context.Dashboards.FindAsync(request.Dashboard.DashboardId);

         if (dashboard == null) _context.Dashboards.Add(dashboard = new Dashboard());

         dashboard.Name = request.Dashboard.Name;

         dashboard.ProfileId = request.Dashboard.ProfileId;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveDashboardResponse() { DashboardId = dashboard.DashboardId };
     }
 }

