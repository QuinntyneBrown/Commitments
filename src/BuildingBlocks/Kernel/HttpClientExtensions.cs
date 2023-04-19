// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using System.Text;


namespace Kernel;

public static class HttpClientExtensions
{
    public static async Task<TResult> PostAsAsync<TResult>(this HttpClient client, string url, HttpContent content)
    {
        var responseMessage = await client.PostAsync(url, content);

        return JsonConvert.DeserializeObject<TResult>(await responseMessage.Content.ReadAsStringAsync());
    }

    public static async Task<TOut> PostAsAsync<TIn, TOut>(this HttpClient client, string url, TIn content)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync(url, stringContent);

        var responseText = await responseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TOut>(responseText);
    }

    public static async Task<TOut> PutAsAsync<TIn, TOut>(this HttpClient client, string url, TIn content)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var responseMessage = await client.PutAsync(url, stringContent);

        return JsonConvert.DeserializeObject<TOut>(await responseMessage.Content.ReadAsStringAsync());
    }

    public static async Task<T> GetAsync<T>(this HttpClient client, string url)
    {
        HttpResponseMessage httpResponseMessage = await client.GetAsync(url);

        return JsonConvert.DeserializeObject<T>((await httpResponseMessage.Content.ReadAsStringAsync()));
    }
}