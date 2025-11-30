// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
#if NET6_0
namespace ProtectedNumbers.Internal;

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

using ProtectedNumbers;

/// <summary>
/// Disables child validation for the <see cref="ProtectedNumber"/> value object on .NET 6 MVC,
/// preventing the validator from invoking its getters during traversal.
/// </summary>
internal sealed class DisableChildValidationForProtectedNumber : IValidationMetadataProvider
{
  public void CreateValidationMetadata(ValidationMetadataProviderContext context)
  {
    if (context.Key.ModelType == typeof(ProtectedNumber))
    {
      context.ValidationMetadata.ValidateChildren = false;
    }
  }
}
#endif
