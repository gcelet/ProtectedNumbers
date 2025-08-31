// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

/// <summary>
/// Provides salt values used by preparators that prefix and/or suffix prepared payloads.
/// </summary>
public interface IApplicationDataSaltProvider
{
  /// <summary>
  /// Generates a salt suitable for inclusion in prepared values.
  /// </summary>
  /// <returns>A salt.</returns>
  ProtectionNumberSalt GenerateSalt();
}
