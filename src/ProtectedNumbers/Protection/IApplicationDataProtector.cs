// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides methods to protect (encrypt/sign) and unprotect application data carried by a <see cref="IProtectedNumber{TProtectedNumber}"/>.
/// </summary>
public interface IApplicationDataProtector
{
  /// <summary>
  /// Produces a new <see cref="IProtectedNumber{TProtectedNumber}"/> that contains a protected representation for the provided raw value.
  /// </summary>
  /// <param name="protectedNumber">An initialized instance that must have a raw <see cref="IProtectedNumber{TProtectedNumber}.Value"/>.</param>
  /// <returns>A new protected instance containing the original value and its protected representation.</returns>
  /// <exception cref="ArgumentException">Thrown when the input is uninitialized or does not carry a raw value.</exception>
  TProtectedNumber Protect<TProtectedNumber>(TProtectedNumber protectedNumber)
    where TProtectedNumber : struct, IProtectedNumber<TProtectedNumber>;

  /// <summary>
  /// Attempts to unprotect the provided instance.
  /// </summary>
  /// <param name="input">The instance expected to hold a protected representation.</param>
  /// <param name="output">When the method returns <see langword="true"/>, receives the corresponding unprotected instance.</param>
  /// <returns><see langword="true"/> if unprotection succeeded; otherwise, <see langword="false"/>.</returns>
  bool TryUnprotect<TProtectedNumber>(TProtectedNumber input, [NotNullWhen(true)] out TProtectedNumber? output)
    where TProtectedNumber : struct, IProtectedNumber<TProtectedNumber>;

  /// <summary>
  /// Unprotects the provided instance or throws if it cannot be unprotected.
  /// </summary>
  /// <param name="protectedNumber">The instance holding a protected representation.</param>
  /// <returns>The unprotected instance.</returns>
  /// <exception cref="ArgumentException">Thrown when the input is uninitialized or has no protected value.</exception>
  /// <exception cref="InvalidOperationException">Thrown when the protected payload is corrupted or invalid.</exception>
  TProtectedNumber Unprotect<TProtectedNumber>(TProtectedNumber protectedNumber)
    where TProtectedNumber : struct, IProtectedNumber<TProtectedNumber>;
}
