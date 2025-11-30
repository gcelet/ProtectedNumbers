// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.MinimalApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
#if NET6_0
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using ProtectedNumbers.Protection;

#elif NET8_0_OR_GREATER
using Microsoft.AspNetCore.Routing;
#endif

public static class Extensions
{
  public static void RegisterMinimalApiEndpoints(this WebApplication app)
  {
    const string minimalApiPrefix = "/minimal-api";
#if NET6_0
    app.RegisterSampleObjectEndpoints(minimalApiPrefix);
#elif NET8_0_OR_GREATER
    RouteGroupBuilder minimalApi = app.MapGroup(minimalApiPrefix);

    minimalApi.RegisterSampleObjectEndpoints();
#endif
  }

#if NET6_0
  public static ProtectedNumber[]? BindProtectedNumberCollection(this HttpContext httpContext, string parameterName)
  {
    if (!httpContext.Request.Query.TryGetValue(parameterName, out StringValues stringValues))
    {
      return null;
    }

    IApplicationDataProtector? protector = httpContext.RequestServices.GetService<IApplicationDataProtector>();
    string?[] protectedNumberRaws = stringValues.ToArray();

    // HACK: Minimal api on .net 6 don't handle correctly collection binding: we must unprotect manually collections only on .net 6 (it's done automatically/naturally on newer .net versions)
    ProtectedNumber[]? protectedNumbers =
      protectedNumberRaws.Select(s => ProtectedNumber.From(null, s))             // capture protected string
      .Select(pn => protector != null && protector.TryUnprotect(pn, out var unp) // attempt unprotect
        ? unp!.Value
        : pn)                                                                    // keep as-is if unprotect fails
      .ToArray();

    return protectedNumbers;
  }
#endif

}
