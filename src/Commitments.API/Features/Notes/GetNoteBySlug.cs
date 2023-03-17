// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Api.Features.Notes;

 public class GetNoteBySlugRequest : IRequest<GetNoteBySlugResponse> {
     public string Slug { get; set; }
 }

 public class GetNoteBySlugResponse
 {
     public NoteDto Note { get; set; }
 }

 public class GetNoteBySlugQueryHandler : IRequestHandler<GetNoteBySlugRequest, GetNoteBySlugResponse>
 {
     private readonly ICommimentsDbContext _context;

     public GetNoteBySlugQueryHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetNoteBySlugResponse> Handle(GetNoteBySlugRequest request, CancellationToken cancellationToken)
     {
         return new GetNoteBySlugResponse()
         {
             Note = NoteDto.FromNote(await _context.Notes
                 .Include(x => x.NoteTags)
                 .Include("NoteTags.Tag")
                 .Where(x => x.Slug == request.Slug)
                 .SingleAsync())
         };
     }
 }

