// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Internal;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

/// <summary>
/// Configures ASP.NET Core JSON options to include a converter that exposes the application's service provider
/// to System.Text.Json, enabling dependency resolution during serialization/deserialization.
/// </summary>
internal class ProtectedNumbersConfigureJsonOptions : IConfigureOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>
{
  /// <summary>
  /// Initializes the configurator with the services required to expose providers to JSON.
  /// </summary>
  /// <param name="serviceProvider">The root application <see cref="IServiceProvider"/>.</param>
  /// <param name="httpContextAccessor">Accessor used to access the current request's services, when present.</param>
  public ProtectedNumbersConfigureJsonOptions(IServiceProvider serviceProvider,
    IHttpContextAccessor httpContextAccessor)
  {
    HttpContextAccessor = httpContextAccessor;
    ServiceProvider = serviceProvider;
  }

  private IHttpContextAccessor HttpContextAccessor { get; }

  private IServiceProvider ServiceProvider { get; }

  /// <summary>
  /// Adds the service-provider-aware converter to the JSON <paramref name="options"/>.
  /// </summary>
  /// <param name="options">The JSON options to configure.</param>
  public void Configure(Microsoft.AspNetCore.Http.Json.JsonOptions options)
  {
    options.SerializerOptions.TryAddServiceProvider(ServiceProvider, HttpContextAccessor);
  }
}
