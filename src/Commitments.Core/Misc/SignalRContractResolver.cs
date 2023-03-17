// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;


namespace Commitments.Core.Misc;

public class SignalRContractResolver : IContractResolver
{
    private readonly Assembly _assembly;
    private readonly IContractResolver _camelCaseContractResolver;
    private readonly IContractResolver _defaultContractSerializer;

    public SignalRContractResolver()
    {
        _defaultContractSerializer = new DefaultContractResolver();
        _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
        _assembly = typeof(Hub).GetTypeInfo().Assembly;
    }

    public JsonContract ResolveContract(Type type)
    {
        if (type.GetTypeInfo().Assembly.Equals(_assembly))
            return _defaultContractSerializer.ResolveContract(type);

        return _camelCaseContractResolver.ResolveContract(type);
    }

}

