// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.Messages;

public class UserMetadataMessage : IRequest
{
    public string Username { get; set; }
    public string MetadataPropertyName { get; set; }

    public string MetadataPropertyValue { get; set; }

}

