// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Internal;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

/// <summary>
/// Provides a model binder for <see cref="ProtectedNumber"/> types to ASP.NET Core MVC.
/// </summary>
/// <remarks>
/// When the requested model type is <see cref="ProtectedNumber"/> or a nullable <see cref="ProtectedNumber"/>,
/// this provider returns a <see cref="BinderTypeModelBinder"/> that delegates to
/// <see cref="ProtectedNumber.ProtectedNumberModelBinder"/> for the actual binding.
/// </remarks>
internal class ProtectedNumberModelBinderProvider : IModelBinderProvider
{
  /// <inheritdoc />
  public IModelBinder? GetBinder(ModelBinderProviderContext context)
  {
    if (context == null)
    {
      throw new ArgumentNullException(nameof(context));
    }

    if (context.Metadata.ModelType == typeof(ProtectedNumber) ||
        context.Metadata.ModelType == typeof(ProtectedNumber?))
    {
      return new BinderTypeModelBinder(typeof(ProtectedNumber.ProtectedNumberModelBinder));
    }

    return null;
  }
}
