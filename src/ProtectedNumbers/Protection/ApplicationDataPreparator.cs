// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

/// <summary>
/// Base implementation of <see cref="IApplicationDataPreparator"/> that formats a 64-bit integer into
/// a fixed-width, zero-padded decimal string and parses it back.
/// </summary>
public abstract class ApplicationDataPreparator : IApplicationDataPreparator
{
  /// <inheritdoc />
  public virtual string Prepare(long value)
  {
    /*
     *   long.MaxValue:  9,223,372,036,854,775,807
     *  ulong.MaxValue: 18,446,744,073,709,551,615
     *  format        : 00,000,000,000,000,000,000
     */
    string stringValue = value.ToString("00000000000000000000");

    return stringValue;
  }

  /// <inheritdoc />
  public virtual bool TryExtract(string stringValue, out long value) => long.TryParse(stringValue, out value);
}
