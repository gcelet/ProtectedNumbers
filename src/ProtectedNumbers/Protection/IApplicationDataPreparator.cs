// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

/// <summary>
/// Normalizes and denormalizes the numeric payload that will be protected.
/// Implementations may add salting or other formatting concerns.
/// </summary>
public interface IApplicationDataPreparator
{
  /// <summary>
  /// Converts a raw numeric value into its canonical string representation before protection.
  /// </summary>
  /// <param name="value">The value to prepare.</param>
  /// <returns>A stable, parseable string representation.</returns>
  string Prepare(long value);

  /// <summary>
  /// Attempts to parse a previously prepared string back to its numeric value.
  /// </summary>
  /// <param name="stringValue">The string to parse.</param>
  /// <param name="value">When <see langword="true"/>, receives the parsed numeric value.</param>
  /// <returns><see langword="true"/> if parsing succeeded; otherwise, <see langword="false"/>.</returns>
  bool TryExtract(string stringValue, out long value);
}
