// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ToDoService.Core.AggregateModel.ToDoAggregate;

public class ToDo
{
    public Guid ToDoId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ProfileId { get; set; }
    public DateTime Due { get; set; }
    public DateTime CompletedOn { get; set; }
}