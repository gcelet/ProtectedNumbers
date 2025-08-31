// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.DataProtection;

/// <summary>
/// Default implementation of <see cref="IApplicationDataProtector"/> based on ASP.NET Core Data Protection.
/// It formats numeric values with an <see cref="IApplicationDataPreparator"/>, protects the payload, and
/// can restore the original value from a protected representation.
/// </summary>
public class ApplicationDataProtector : IApplicationDataProtector
{
  /// <summary>
  /// Initializes a new protector using the given data-protection provider and payload preparator.
  /// </summary>
  /// <param name="rootProvider">The <see cref="IDataProtectionProvider"/> used to create an <see cref="IDataProtector"/>.</param>
  /// <param name="purposeProvider">The purpose provider that use to build <see cref="IDataProtector"/></param>
  /// <param name="applicationDataPreparator">The preparator that formats and parses numeric payloads.</param>
  public ApplicationDataProtector(IDataProtectionProvider rootProvider,
    IApplicationProtectorPurposeProvider purposeProvider,
    IApplicationDataPreparator applicationDataPreparator)
  {
    ApplicationProtectorPurpose protectorPurpose = purposeProvider.GetApplicationProtectorPurpose();

    RootProvider = rootProvider;
    Preparator = applicationDataPreparator;
    Protector = protectorPurpose.SubPurposes == null
      ? RootProvider.CreateProtector(protectorPurpose.Purpose)
      : RootProvider.CreateProtector(protectorPurpose.Purpose, protectorPurpose.SubPurposes);
  }

  private IApplicationDataPreparator Preparator { get; }

  private IDataProtector Protector { get; }

  private IDataProtectionProvider RootProvider { get; }

  /// <inheritdoc />
  public TProtectedNumber Protect<TProtectedNumber>(TProtectedNumber protectedNumber)
    where TProtectedNumber : struct, IProtectedNumber<TProtectedNumber>
  {
    if (!protectedNumber.IsInitialized())
    {
      throw new ArgumentException("can't protect an uninitialized value", nameof(protectedNumber));
    }

    if (!protectedNumber.HasValue)
    {
      throw new ArgumentException("can't protect without a value", nameof(protectedNumber));
    }

    long value = protectedNumber.Value;
    string stringValue = Preparator.Prepare(value);
    string protectedValue = Protector.Protect(stringValue);

    return protectedNumber.WithProtectedValue(protectedValue);
  }

  /// <inheritdoc />
  public bool TryUnprotect<TProtectedNumber>(TProtectedNumber input, [NotNullWhen(true)] out TProtectedNumber? output)
    where TProtectedNumber : struct, IProtectedNumber<TProtectedNumber>
  {
    output = null;

    if (!input.IsInitialized())
    {
      return false;
    }

    if (!input.HasProtectedValue)
    {
      return false;
    }

    string protectedValue = input.ProtectedValue;

    if (!TryUnprotect(protectedValue, out long value))
    {
      return false;
    }

    output = input.WithValue(value);

    return true;
  }

  /// <inheritdoc />
  public TProtectedNumber Unprotect<TProtectedNumber>(TProtectedNumber protectedNumber)
    where TProtectedNumber : struct, IProtectedNumber<TProtectedNumber>
  {
    if (!protectedNumber.IsInitialized())
    {
      throw new ArgumentException("can't unprotect an uninitialized value", nameof(protectedNumber));
    }

    if (!protectedNumber.HasProtectedValue)
    {
      throw new ArgumentException("can't unprotect without a protected value", nameof(protectedNumber));
    }

    string protectedValue = protectedNumber.ProtectedValue;
    string stringValue = Protector.Unprotect(protectedValue);

    if (!Preparator.TryExtract(stringValue, out long value))
    {
      throw new InvalidOperationException("can't unprotect a corrupted protected value");
    }

    return protectedNumber.WithValue(value);
  }

  private bool TryUnprotect(string protectedValue, out long value)
  {
    try
    {
      string stringValue = Protector.Unprotect(protectedValue);

      if (!Preparator.TryExtract(stringValue, out value))
      {
        value = 0L;
        return false;
      }

      return true;
    }
    catch
    {
      value = 0;
      return false;
    }
  }
}
