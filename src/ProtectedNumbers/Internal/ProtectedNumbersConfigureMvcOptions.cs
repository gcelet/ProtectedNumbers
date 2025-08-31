// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Internal;

#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
#endif
using Microsoft.Extensions.Options;

#if NET6_0
/// <summary>
/// Configures ASP.NET Core MVC options to register a model binder provider for <see cref="ProtectedNumber"/>.
/// </summary>
#else
/// <summary>
/// Configures ASP.NET Core MVC options to register a model binder provider for <see cref="ProtectedNumber"/>.
/// </summary>
/// <remarks>
/// The provider is inserted just before <see cref="TryParseModelBinderProvider"/> when present so that
/// binding for <see cref="ProtectedNumber"/> and nullable ProtectedNumber takes precedence over any
/// generic TryParse-based binding. If the TryParse provider is not found, the provider is appended.
/// </remarks>
#endif
internal class ProtectedNumbersConfigureMvcOptions : IConfigureOptions<Microsoft.AspNetCore.Mvc.MvcOptions>
{
  /// <summary>
  /// Adds the <see cref="ProtectedNumberModelBinderProvider"/> to the MVC <paramref name="options"/> with the appropriate ordering.
  /// </summary>
  /// <param name="options">The MVC options to configure.</param>
  public void Configure(Microsoft.AspNetCore.Mvc.MvcOptions options)
  {
    int indexOfTryParseModelBinderProvider = -1;

#if NET8_0_OR_GREATER
    for (int i = options.ModelBinderProviders.Count - 1; i >= 0; i--)
    {
      IModelBinderProvider modelBinderProvider = options.ModelBinderProviders[i];

      if (modelBinderProvider is TryParseModelBinderProvider)
      {
        indexOfTryParseModelBinderProvider = i;
        break;
      }
    }
#endif

    if (indexOfTryParseModelBinderProvider != -1)
    {
      options.ModelBinderProviders.Insert(indexOfTryParseModelBinderProvider, new ProtectedNumberModelBinderProvider());
    }
    else
    {
      options.ModelBinderProviders.Add(new ProtectedNumberModelBinderProvider());
    }
  }
}
