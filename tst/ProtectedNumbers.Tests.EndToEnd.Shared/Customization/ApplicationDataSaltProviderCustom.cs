// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Customization;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using ProtectedNumbers.Protection;

public class ApplicationDataSaltProviderCustom : IApplicationDataSaltProvider
{
  public ApplicationDataSaltProviderCustom(IHttpContextAccessor httpContextAccessor)
  {
    HttpContextAccessor = httpContextAccessor;
  }

  private IHttpContextAccessor HttpContextAccessor { get; }

  /// <inheritdoc />
  public ProtectionNumberSalt GenerateSalt()
  {
    string saltPrefix = string.Empty;
    string saltSuffix = string.Empty;

    HttpContext? httpContext = HttpContextAccessor.HttpContext;

    if (httpContext != null)
    {
      if (httpContext.Request.Headers.TryGetValue("X-ProtectedNumbers-Context", out StringValues contextValues))
      {
        saltPrefix = contextValues.ToString();
      }
      if (httpContext.Request.Headers.TryGetValue("X-ProtectedNumbers-Temporal", out StringValues temporalValues))
      {
        saltSuffix = DateTime.UnixEpoch.Ticks.ToString();
      }
    }

    ProtectionNumberSalt salt = new(saltPrefix, saltSuffix);

    return salt;
  }
}
