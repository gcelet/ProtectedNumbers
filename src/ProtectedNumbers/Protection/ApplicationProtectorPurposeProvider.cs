// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

using ProtectedNumbers.Configuration;

/// <summary>
/// Default purpose provider that reads the configured protector purpose and sub-purposes
/// from <see cref="ProtectedNumbersConfigurationRuntime"/>.
/// </summary>
public class ApplicationProtectorPurposeProvider : IApplicationProtectorPurposeProvider
{
  /// <summary>
  /// Creates a new provider using the given runtime configuration.
  /// </summary>
  public ApplicationProtectorPurposeProvider(ProtectedNumbersConfigurationRuntime configuration)
  {
    Configuration = configuration;
  }

  private ProtectedNumbersConfigurationRuntime Configuration { get; }

  /// <inheritdoc />
  public ApplicationProtectorPurpose GetApplicationProtectorPurpose() =>
    new(Configuration.ProtectorPurpose, Configuration.ProtectorSubPurposes);
}
