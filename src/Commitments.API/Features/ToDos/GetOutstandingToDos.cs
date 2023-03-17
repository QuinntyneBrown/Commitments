// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.ToDos;

 public class GetOutstandingToDosRequest : IRequest<GetOutstandingToDosResponse> {
     public int ProfileId { get; set; }
 }

 public class GetOutstandingToDosResponse
 {
     public IEnumerable<ToDoDto> ToDos { get; set; }
 }

 public class GetOutstandingToDosQueryHandler : IRequestHandler<GetOutstandingToDosRequest, GetOutstandingToDosResponse>
 {
     public ICommimentsDbContext _context { get; set; }
     public GetOutstandingToDosQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetOutstandingToDosResponse> Handle(GetOutstandingToDosRequest request, CancellationToken cancellationToken)
         => new GetOutstandingToDosResponse()
         {
             ToDos = await _context.ToDos
             .Where(x => x.CompletedOn == null && x.ProfileId == request.ProfileId)
             .Select(x => ToDoDto.FromToDo(x))
             .ToListAsync()
         };
 }

