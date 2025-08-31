// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers;

using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using ProtectedNumbers.Configuration;
using ProtectedNumbers.Internal;
using ProtectedNumbers.Protection;

/// <summary>
/// Dependency injection helpers for wiring ProtectedNumbers services.
/// </summary>
public static class Extensions
{
  /// <summary>
  /// Registers the services required to use ProtectedNumbers with ASP.NET Core and System.Text.Json.
  /// </summary>
  /// <param name="services">The service collection to add registrations to.</param>
  /// <param name="opts">Optional configuration callback to customize ProtectedNumbers integration via a builder.</param>
  /// <returns>The same <paramref name="services"/> instance to enable chaining.</returns>
  /// <remarks>
  /// This method:
  /// - Configures JSON serialization to expose an <see cref="IServiceProvider"/> at runtime so converters can resolve services.
  /// - Registers <see cref="IApplicationDataProtector"/> and the default <see cref="IApplicationDataPreparator"/>.
  /// </remarks>
  public static IServiceCollection AddProtectedNumbers(this IServiceCollection services,
    Action<ProtectedNumbersConfigurationBuilder>? opts = null)
  {
    ProtectedNumbersConfiguration configuration = new();

    opts?.Invoke(new ProtectedNumbersConfigurationBuilder(configuration));

    ProtectedNumbersConfigurationRuntime runtimeConfiguration = new()
    {
      ProtectorPurpose = configuration.ProtectorPurpose ?? "ProtectedNumbers",
      ProtectorSubPurposes = configuration.ProtectorSubPurposes,
    };

    services.AddSingleton(runtimeConfiguration);

    services.AddScoped(typeof(IApplicationDataProtector),
      configuration.DataProtectorType ?? typeof(ApplicationDataProtector));

    Type? applicationDataPreparatorType = configuration.DataPreparatorType;

    if (configuration.DataSaltProviderType != null)
    {
      services.AddScoped(typeof(IApplicationDataSaltProvider), configuration.DataSaltProviderType);
      applicationDataPreparatorType ??= typeof(ApplicationDataPreparatorWithSalt);
    }

    applicationDataPreparatorType ??= typeof(ApplicationDataPreparatorWithoutSalt);
    services.AddScoped(typeof(IApplicationDataPreparator), applicationDataPreparatorType);

    Type applicationProtectorPurposeProvider = configuration.PurposeProvider ?? typeof(ApplicationProtectorPurposeProvider);
    services.AddScoped(typeof(IApplicationProtectorPurposeProvider), applicationProtectorPurposeProvider);

    if (configuration.EnabledSystemTextJsonServiceProvider)
    {
      services.ConfigureOptions<ProtectedNumbersConfigureJsonOptions>();
    }

    if (configuration.EnabledMvcModelBinder)
    {
      services.ConfigureOptions<ProtectedNumbersConfigureMvcOptions>();
    }

    if (configuration.EnabledMvcSystemTextJsonServiceProvider)
    {
      services.ConfigureOptions<ProtectedNumbersConfigureMvcJsonOptions>();
    }

    return services;
  }

  /// <summary>
  /// Ensures that a JSON converter exposing an <see cref="IServiceProvider"/> is present in the
  /// provided <see cref="JsonSerializerOptions"/>. If none is found, a converter is added.
  /// </summary>
  /// <param name="jsonSerializerOptions">The <see cref="JsonSerializerOptions"/> to augment.</param>
  /// <param name="serviceProvider">The root <see cref="IServiceProvider"/> used to resolve services when no request scope exists.</param>
  /// <param name="httpContextAccessor">
  /// Optional accessor to reach the current <see cref="Microsoft.AspNetCore.Http.HttpContext"/>.
  /// When available, it allows resolving scoped services via <see cref="HttpContext.RequestServices"/>.
  /// </param>
  /// <remarks>
  /// This method is idempotent: if an <see cref="IServiceProvider"/> is already discoverable via
  /// <see cref="JsonSerializerOptions.Converters"/>, it performs no action. The converter added is a
  /// lightweight placeholder (<see cref="ProtectedNumbers.Internal.ServiceProviderDummySystemTextJsonConverter"/>)
  /// whose sole purpose is to expose an <see cref="IServiceProvider"/> to other converters at runtime.
  /// </remarks>
  public static void TryAddServiceProvider(this JsonSerializerOptions jsonSerializerOptions,
    IServiceProvider serviceProvider,
    IHttpContextAccessor? httpContextAccessor = null)
  {
    IServiceProvider? alreadyRegisterServiceProvider = jsonSerializerOptions.GetServiceProvider();

    if (alreadyRegisterServiceProvider != null)
    {
      return;
    }

    ServiceProviderDummySystemTextJsonConverter converter = new(serviceProvider, httpContextAccessor);
    jsonSerializerOptions.Converters.Add(converter);
  }
}
