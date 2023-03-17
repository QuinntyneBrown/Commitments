// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.Commitments;

 public class GetCommitmentByIdValidator : AbstractValidator<GetCommitmentByIdRequest>
 {
     public GetCommitmentByIdValidator()
     {
         RuleFor(request => request.CommitmentId).NotEqual(0);
     }
 }

 public class GetCommitmentByIdRequest : IRequest<GetCommitmentByIdResponse> {
     public int CommitmentId { get; set; }
 }

 public class GetCommitmentByIdResponse
 {
     public CommitmentDto Commitment { get; set; }
 }

 public class GetCommitmentByIdHandler : IRequestHandler<GetCommitmentByIdRequest, GetCommitmentByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetCommitmentByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetCommitmentByIdResponse> Handle(GetCommitmentByIdRequest request, CancellationToken cancellationToken)
         => new GetCommitmentByIdResponse()
         {
             Commitment = CommitmentDto.FromCommitment(await _context.Commitments.FindAsync(request.CommitmentId))
         };
 }

