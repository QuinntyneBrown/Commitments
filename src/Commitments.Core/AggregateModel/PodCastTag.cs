// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace Commitments.Core.AggregateModel;

public class PodCastTag
{
    public int PodCastTagId { get; set; }
    public int PodCastId { get; set; }
    public int TagId { get; set; }
    public PodCast PodCast { get; set; }
    public Tag Tag { get; set; }
}

