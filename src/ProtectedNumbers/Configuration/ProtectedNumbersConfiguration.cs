// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Configuration;

using ProtectedNumbers.Protection;

/// <summary>
/// Options controlling how ProtectedNumbers integrates with ASP.NET Core and System.Text.Json.
/// </summary>
/// <remarks>
/// These options are consumed by <see cref="ProtectedNumbers.Extensions.AddProtectedNumbers(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{ProtectedNumbers.Configuration.ProtectedNumbersConfigurationBuilder}?)"/>
/// to register services, model binders, and JSON configuration.
/// </remarks>
public class ProtectedNumbersConfiguration
{
  /// <summary>
  /// Optional custom implementation type for <see cref="IApplicationDataPreparator"/>.
  /// </summary>
  /// <remarks>
  /// If not specified, default type use depends of <see cref="DataSaltProviderType"/> provided or not:
  /// - if <see cref="DataSaltProviderType"/> not specified: <see cref="Protection.ApplicationDataPreparatorWithoutSalt"/> is used
  /// - if <see cref="DataSaltProviderType"/> specified: <see cref="Protection.ApplicationDataPreparatorWithSalt"/> is used
  /// The type must be resolvable via dependency injection and implement <see cref="IApplicationDataPreparator"/>.
  /// </remarks>
  public Type? DataPreparatorType { get; set; }

  /// <summary>
  /// Optional custom implementation type for <see cref="IApplicationDataProtector"/>.
  /// </summary>
  /// <remarks>
  /// If not specified, <see cref="Protection.ApplicationDataProtector"/> is used by default.
  /// The type must be resolvable via dependency injection and implement <see cref="IApplicationDataProtector"/>.
  /// </remarks>
  public Type? DataProtectorType { get; set; }

  /// <summary>
  /// Optional custom implementation type for <see cref="IApplicationDataSaltProvider"/>.
  /// </summary>
  /// <remarks>
  /// If not specified, no salt will be use during protection.
  /// The type must be resolvable via dependency injection and implement <see cref="IApplicationDataSaltProvider"/>.
  /// </remarks>
  public Type? DataSaltProviderType { get; set; }

  /// <summary>
  /// Enables registration of the MVC model binder for <c>ProtectedNumber</c> and <c>ProtectedNumber?</c>.
  /// </summary>
  /// <remarks>
  /// Default is <c>true</c>. When enabled, <see cref="Internal.ProtectedNumbersConfigureMvcOptions"/> inserts the
  /// <c>ProtectedNumberModelBinderProvider</c> just before the <c>TryParseModelBinderProvider</c>, ensuring
  /// ProtectedNumber binding takes precedence over TryParse-based binders.
  /// </remarks>
  public bool EnabledMvcModelBinder { get; set; } = true;

  /// <summary>
  /// Enables adding a JSON converter that exposes an <see cref="IServiceProvider"/> for MVC's System.Text.Json options.
  /// </summary>
  /// <remarks>
  /// Default is <c>true</c>. When enabled, <see cref="Internal.ProtectedNumbersConfigureMvcJsonOptions"/> ensures an
  /// <see cref="IServiceProvider"/> is discoverable by converters during MVC serialization scenarios.
  /// </remarks>
  public bool EnabledMvcSystemTextJsonServiceProvider { get; set; } = true;

  /// <summary>
  /// Enables adding a JSON converter that exposes an <see cref="IServiceProvider"/> for non-MVC System.Text.Json options.
  /// </summary>
  /// <remarks>
  /// Default is <c>true</c>. When enabled, <see cref="Internal.ProtectedNumbersConfigureJsonOptions"/> ensures an
  /// <see cref="IServiceProvider"/> is discoverable by converters when using <see cref="System.Text.Json.JsonSerializer"/>
  /// outside MVC.
  /// </remarks>
  public bool EnabledSystemTextJsonServiceProvider { get; set; } = true;

  /// <summary>
  /// The root purpose string to use when deriving the application data protector.
  /// </summary>
  /// <remarks>
  /// If specified, this takes precedence over any <see cref="PurposeProvider"/>. Changing this value will
  /// invalidate previously generated protected values.
  /// </remarks>
  public string? ProtectorPurpose { get; set; }

  /// <summary>
  /// Optional sub-purposes appended to <see cref="ProtectorPurpose"/> for additional scoping.
  /// </summary>
  public string[]? ProtectorSubPurposes { get; set; }

  /// <summary>
  /// Optional type implementing <see cref="Protection.IApplicationProtectorPurposeProvider"/> to supply
  /// the effective purpose at runtime.
  /// </summary>
  /// <remarks>
  /// If both <see cref="ProtectorPurpose"/> and <see cref="PurposeProvider"/> are provided, the explicit
  /// <see cref="ProtectorPurpose"/> configuration is used.
  /// </remarks>
  public Type? PurposeProvider { get; set; }
}
