// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Configuration;

/// <summary>
/// Runtime-effective configuration values for the data protector purpose used by ProtectedNumbers.
/// </summary>
/// <remarks>
/// This type is registered and populated during service registration and then consumed at runtime
/// by <see cref="ProtectedNumbers.Protection.IApplicationProtectorPurposeProvider"/> to build the
/// <see cref="ProtectedNumbers.Protection.ApplicationProtectorPurpose"/>.
/// </remarks>
public class ProtectedNumbersConfigurationRuntime
{
  /// <summary>
  /// The root purpose used to derive data protection keys.
  /// </summary>
  #if NET6_0
  public string ProtectorPurpose { get; init; } = null!;
#else
  public required string ProtectorPurpose { get; init; }
#endif

  /// <summary>
  /// Optional sub-purposes appended to the root purpose for additional scoping.
  /// </summary>
  public string[]? ProtectorSubPurposes { get; init; }
}
