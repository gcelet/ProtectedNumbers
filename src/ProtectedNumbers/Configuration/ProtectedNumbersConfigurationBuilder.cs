// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Configuration;

using ProtectedNumbers.Protection;

/// <summary>
/// Fluent builder to configure <see cref="ProtectedNumbersConfiguration"/> when calling
/// <see cref="ProtectedNumbers.Extensions.AddProtectedNumbers(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{ProtectedNumbers.Configuration.ProtectedNumbersConfigurationBuilder}?)"/>.
/// </summary>
public class ProtectedNumbersConfigurationBuilder
{
  /// <summary>
  /// Creates a builder bound to the provided configuration instance.
  /// </summary>
  /// <param name="configuration">The configuration object to mutate.</param>
  public ProtectedNumbersConfigurationBuilder(ProtectedNumbersConfiguration configuration)
  {
    Configuration = configuration;
  }

  private ProtectedNumbersConfiguration Configuration { get; }

  /// <summary>
  /// Disables registration of the MVC model binder for ProtectedNumber types.
  /// </summary>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder DisableMvcModelBinder()
  {
    Configuration.EnabledMvcModelBinder = false;
    return this;
  }

  /// <summary>
  /// Disables exposing an <see cref="IServiceProvider"/> via JSON converters for MVC scenarios.
  /// </summary>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder DisableMvcSystemTextJsonServiceProvider()
  {
    Configuration.EnabledMvcSystemTextJsonServiceProvider = false;
    return this;
  }

  /// <summary>
  /// Disables exposing an <see cref="IServiceProvider"/> via JSON converters for non-MVC scenarios.
  /// </summary>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder DisableSystemTextJsonServiceProvider()
  {
    Configuration.EnabledSystemTextJsonServiceProvider = false;
    return this;
  }

  /// <summary>
  /// Enables registration of the MVC model binder for ProtectedNumber types.
  /// </summary>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder EnableMvcModelBinder()
  {
    Configuration.EnabledMvcModelBinder = true;
    return this;
  }

  /// <summary>
  /// Enables exposing an <see cref="IServiceProvider"/> via JSON converters for MVC scenarios.
  /// </summary>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder EnableMvcSystemTextJsonServiceProvider()
  {
    Configuration.EnabledMvcSystemTextJsonServiceProvider = true;
    return this;
  }

  /// <summary>
  /// Enables exposing an <see cref="IServiceProvider"/> via JSON converters for non-MVC scenarios.
  /// </summary>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder EnableSystemTextJsonServiceProvider()
  {
    Configuration.EnabledSystemTextJsonServiceProvider = true;
    return this;
  }

  /// <summary>
  /// Sets the protector purpose and optional sub-purposes to use for protecting numbers.
  /// </summary>
  /// <param name="protectorPurpose">The root purpose string.</param>
  /// <param name="protectorSubPurposes">Optional sub-purposes for additional scoping.</param>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder SetProtectorPurpose(string protectorPurpose, params string[]? protectorSubPurposes)
  {
    Configuration.ProtectorPurpose = protectorPurpose;
    Configuration.ProtectorSubPurposes = protectorSubPurposes;
    return this;
  }

  /// <summary>
  /// Specifies a custom implementation for <see cref="IApplicationDataPreparator"/> to use.
  /// </summary>
  /// <typeparam name="T">The preparator type to register.</typeparam>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder UseApplicationDataPreparator<T>()
    where T : IApplicationDataPreparator
  {
    Configuration.DataPreparatorType = typeof(T);
    return this;
  }

  /// <summary>
  /// Specifies a custom implementation for <see cref="IApplicationDataProtector"/> to use.
  /// </summary>
  /// <typeparam name="T">The protector type to register.</typeparam>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder UseApplicationDataProtector<T>()
    where T : IApplicationDataProtector
  {
    Configuration.DataProtectorType = typeof(T);
    return this;
  }

  /// <summary>
  /// Specifies a custom implementation for <see cref="IApplicationDataSaltProvider"/> to use.
  /// </summary>
  /// <typeparam name="T">The salt provider type to register.</typeparam>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder UseApplicationDataSaltProvider<T>()
    where T : IApplicationDataSaltProvider
  {
    Configuration.DataSaltProviderType = typeof(T);
    return this;
  }

  /// <summary>
  /// Specifies a custom implementation for <see cref="IApplicationProtectorPurposeProvider"/> to supply
  /// the protector purpose at runtime.
  /// </summary>
  /// <typeparam name="T">The provider type to register.</typeparam>
  /// <returns>The current builder for chaining.</returns>
  public ProtectedNumbersConfigurationBuilder UseApplicationProtectorPurposeProvider<T>()
    where T : IApplicationProtectorPurposeProvider
  {
    Configuration.PurposeProvider = typeof(T);
    return this;
  }
}
