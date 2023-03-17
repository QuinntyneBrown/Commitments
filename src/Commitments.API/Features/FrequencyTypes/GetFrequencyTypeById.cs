// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using FluentValidation;


namespace Commitments.Api.Features.FrequencyTypes;

 public class GetFrequencyTypeByIdValidator : AbstractValidator<GetFrequencyTypeByIdRequest>
 {
     public GetFrequencyTypeByIdValidator()
     {
         RuleFor(request => request.FrequencyTypeId).NotEqual(0);
     }
 }

 public class GetFrequencyTypeByIdRequest : IRequest<GetFrequencyTypeByIdResponse> {
     public int FrequencyTypeId { get; set; }
 }

 public class GetFrequencyTypeByIdResponse
 {
     public FrequencyTypeDto FrequencyType { get; set; }
 }

 public class GetFrequencyTypeByIdHandler : IRequestHandler<GetFrequencyTypeByIdRequest, GetFrequencyTypeByIdResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public GetFrequencyTypeByIdHandler(ICommimentsDbContext context) => _context = context;

     public async Task<GetFrequencyTypeByIdResponse> Handle(GetFrequencyTypeByIdRequest request, CancellationToken cancellationToken)
         => new GetFrequencyTypeByIdResponse()
         {
             FrequencyType = FrequencyTypeDto.FromFrequencyType(await _context.FrequencyTypes.FindAsync(request.FrequencyTypeId))
         };
 }

