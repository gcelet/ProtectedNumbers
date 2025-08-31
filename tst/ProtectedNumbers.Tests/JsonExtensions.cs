// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ProtectedNumbers.Internal;

public static class JsonExtensions
{
    public static JsonSerializerOptions DefaultJsonSerializerOptions { get; } = BuildJsonSerializerOptions();

    public static JsonSerializerOptions BuildJsonSerializerOptions()
    {
        JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        return jsonSerializerOptions;
    }

    public static JsonSerializerOptions BuildJsonSerializerOptions(IServiceProvider substituteForServiceProvider,
        IHttpContextAccessor substituteForHttpContextAccessor)
    {
        JsonSerializerOptions jsonSerializerOptions = BuildJsonSerializerOptions();
        ServiceProviderDummySystemTextJsonConverter serviceProviderDummySystemTextJsonConverter =
            new(substituteForServiceProvider, substituteForHttpContextAccessor);

        jsonSerializerOptions.Converters.Add(serviceProviderDummySystemTextJsonConverter);

        return jsonSerializerOptions;
    }

    public static T? ParseJson<T>(this JsonSerializerOptions jsonSerializerOptions, string input)
    {
        T? parsed = JsonSerializer.Deserialize<T>(input, jsonSerializerOptions);

        return parsed;
    }

    public static string ToJson(this JsonSerializerOptions jsonSerializerOptions, dynamic? input)
    {
        string json = JsonSerializer.Serialize(input, jsonSerializerOptions);

        return json;
    }
}
