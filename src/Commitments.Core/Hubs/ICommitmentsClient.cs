// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Commitments.Core.Hubs;

public interface ICommitmentsClient
{

    Task Receive(dynamic message);
}