// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

/// <summary>
/// Extension methods for the types in this namespace.
/// </summary>
public static class Extensions
{
  /// <summary>
  /// Attempts to unprotect each <see cref="ProtectedNumber"/> in the provided sequence.
  /// </summary>
  /// <param name="applicationDataProtector">The data protector used to unprotect items.</param>
  /// <param name="protectedNumbers">
  /// The sequence of items to unprotect. If <see langword="null"/>, this method returns
  /// <paramref name="results"/> as an empty sequence and <see langword="true"/>.
  /// </param>
  /// <param name="results">
  /// When the method returns, contains a sequence with the same number of items and in the same order as
  /// <paramref name="protectedNumbers"/>. For each element, if unprotection succeeds, the corresponding
  /// unprotected instance is returned; otherwise, the original element is preserved in the output.
  /// </param>
  /// <returns>
  /// <see langword="true"/> if every element was successfully unprotected; otherwise, <see langword="false"/>.
  /// </returns>
  /// <exception cref="ArgumentNullException">
  /// Thrown when <paramref name="applicationDataProtector"/> is <see langword="null"/>.
  /// </exception>
  /// <remarks>
  /// - The method never throws due to a per-item unprotection failure; it keeps the original element in the
  ///   output for failed items and reports overall success through the returned boolean.
  /// - The output sequence never contains <see langword="null"/> entries.
  /// - Ordering is preserved.
  /// </remarks>
  public static bool TryUnprotect(this IApplicationDataProtector applicationDataProtector,
    IEnumerable<ProtectedNumber>? protectedNumbers,
    out IEnumerable<ProtectedNumber> results)
  {
    if (applicationDataProtector == null)
    {
      throw new ArgumentNullException(nameof(applicationDataProtector));
    }

    List<ProtectedNumber> l = new();

    if (protectedNumbers == null)
    {
      results = l;
      return true;
    }

    int nbOfErrors = 0;

    foreach (ProtectedNumber protectedNumber in protectedNumbers)
    {
      if (applicationDataProtector.TryUnprotect(protectedNumber, out ProtectedNumber? unprotectedNumber))
      {
        l.Add(unprotectedNumber ?? protectedNumber);
      }
      else
      {
        l.Add(protectedNumber);
        nbOfErrors++;
      }
    }

    results = l;
    return nbOfErrors == 0;
  }
}
