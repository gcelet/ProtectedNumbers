// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

using System.Text;

/// <summary>
/// An <see cref="ApplicationDataPreparator"/> that prefixes the prepared payload with a salt value.
/// This makes the protected representation vary across calls for the same numeric value.
/// </summary>
public class ApplicationDataPreparatorWithSalt : ApplicationDataPreparator
{
  /// <summary>
  /// Initializes a new instance of the preparator using the specified salt provider.
  /// </summary>
  /// <param name="saltProvider">Provider used to generate salt values.</param>
  public ApplicationDataPreparatorWithSalt(IApplicationDataSaltProvider saltProvider)
  {
    SaltProvider = saltProvider;
  }

  private IApplicationDataSaltProvider SaltProvider { get; }

  /// <summary>
  /// Produces a salted string by concatenating a generated salt, a separator, and the base prepared value.
  /// </summary>
  /// <param name="value">The raw numeric value to prepare.</param>
  /// <returns>A salted, canonical string representation.</returns>
  public override string Prepare(long value)
  {
    ProtectionNumberSalt salt = SaltProvider.GenerateSalt();
    StringBuilder sb = new(32);

    if (!string.IsNullOrEmpty(salt.SaltPrefix))
    {
      sb.Append(salt.SaltPrefix).Append('>');
    }

    sb.Append(base.Prepare(value));

    if (!string.IsNullOrEmpty(salt.SaltSuffix))
    {
      sb.Append('>').Append(salt.SaltSuffix);
    }

    string stringValue = sb.ToString();

    return stringValue;
  }
}
