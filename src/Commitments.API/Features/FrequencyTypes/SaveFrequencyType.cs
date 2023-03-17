// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.AggregateModel;
using Commitments.Core.Interfaces;


namespace Commitments.Api.Features.FrequencyTypes;

 public class SaveFrequencyTypeCommandValidator: AbstractValidator<SaveFrequencyTypeRequest> {
     public SaveFrequencyTypeCommandValidator()
     {
         RuleFor(request => request.FrequencyType.FrequencyTypeId).NotNull();
     }
 }

 public class SaveFrequencyTypeRequest : IRequest<SaveFrequencyTypeResponse> {
     public FrequencyTypeDto FrequencyType { get; set; }
 }

 public class SaveFrequencyTypeResponse
 {            
     public int FrequencyTypeId { get; set; }
 }

 public class SaveFrequencyTypeCommandHandler : IRequestHandler<SaveFrequencyTypeRequest, SaveFrequencyTypeResponse>
 {
     public ICommimentsDbContext _context { get; set; }

     public SaveFrequencyTypeCommandHandler(ICommimentsDbContext context) => _context = context;

     public async Task<SaveFrequencyTypeResponse> Handle(SaveFrequencyTypeRequest request, CancellationToken cancellationToken)
     {
         var frequencyType = await _context.FrequencyTypes.FindAsync(request.FrequencyType.FrequencyTypeId);

         if (frequencyType == null) _context.FrequencyTypes.Add(frequencyType = new FrequencyType());

         frequencyType.Name = request.FrequencyType.Name;

         await _context.SaveChangesAsync(cancellationToken);

         return new SaveFrequencyTypeResponse() { FrequencyTypeId = frequencyType.FrequencyTypeId };
     }
 }

