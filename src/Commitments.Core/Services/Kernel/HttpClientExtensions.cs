// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Commitments.Core.Services.Kernel;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data, CancellationToken cancellationToken = default)
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        return await httpClient.PostAsync(url, content, cancellationToken);
    }

    public static async Task<T> ReadAsAsync<T>(this HttpContent content)
    {
        var stream = await content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream) ?? default!;
    }

    public static void AddDefaultHeaders(this HttpClient client, IServiceProvider serviceProvider)
    {
        var httpContextAccessor = serviceProvider.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var authorization = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorization))
        {
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization);
        }
    }
}
