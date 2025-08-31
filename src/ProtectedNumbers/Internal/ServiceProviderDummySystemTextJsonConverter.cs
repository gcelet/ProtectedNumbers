// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Internal;

using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http;

/// <summary>
/// A no-op System.Text.Json converter that also exposes an <see cref="IServiceProvider"/> instance to
/// <see cref="JsonSerializerOptions"/> at runtime. This enables JSON serialization to resolve services
/// (including scoped services via the current <see cref="Microsoft.AspNetCore.Http.HttpContext"/>) when
/// converting other types.
/// </summary>
internal class ServiceProviderDummySystemTextJsonConverter : JsonConverter<object>, IServiceProvider
{
  /// <summary>
  /// Initializes a new instance of the converter wrapper.
  /// </summary>
  /// <param name="serviceProvider">The root <see cref="IServiceProvider"/> to fall back to when no request is available.</param>
  /// <param name="httpContextAccessor">Accessor used to reach the current request's <see cref="HttpContext"/>.</param>
  public ServiceProviderDummySystemTextJsonConverter(
    IServiceProvider serviceProvider,
    IHttpContextAccessor? httpContextAccessor)
  {
    HttpContextAccessor = httpContextAccessor;
    ServiceProvider = serviceProvider;
  }

  private IHttpContextAccessor? HttpContextAccessor { get; }

  private IServiceProvider ServiceProvider { get; }

  /// <inheritdoc/>
  public override bool CanConvert(Type typeToConvert) => false;

  /// <summary>
  /// Resolves a service from the current request service provider when available; otherwise uses the root provider.
  /// </summary>
  /// <param name="serviceType">The type of service to resolve.</param>
  /// <returns>The resolved service instance, or <see langword="null"/> if not found.</returns>
  public object? GetService(Type serviceType)
  {
    // Use the request services, if available, to be able to resolve
    // scoped services.
    // If there isn't a current HttpContext, just use the root service
    // provider.
    IServiceProvider services =
      HttpContextAccessor?.HttpContext?.RequestServices ?? ServiceProvider;

    object? service = services.GetService(serviceType);

    return service;
  }

  /// <inheritdoc/>
  public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
    throw new NotSupportedException();

  /// <inheritdoc/>
  public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
    throw new NotSupportedException();
}
