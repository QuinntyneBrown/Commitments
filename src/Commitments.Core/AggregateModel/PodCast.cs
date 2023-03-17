// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;

public class PodCast: BaseEntity
{	
	public int PodCastId { get; set; }
	public string Url { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}

