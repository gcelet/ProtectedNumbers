// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

/// <summary>
/// Abstraction that supplies the <see cref="ApplicationProtectorPurpose"/> used by the application
/// to protect and unprotect numeric identifiers.
/// </summary>
/// <remarks>
/// Provide a custom implementation when the purpose needs to vary at runtime (e.g. per-tenant,
/// per-environment, or based on request context). The default implementation pulls values from
/// <c>ProtectedNumbersConfigurationRuntime</c>.
/// </remarks>
public interface IApplicationProtectorPurposeProvider
{
  /// <summary>
  /// Returns the effective purpose (and optional sub-purposes) to use for protecting data.
  /// </summary>
  ApplicationProtectorPurpose GetApplicationProtectorPurpose();
}
