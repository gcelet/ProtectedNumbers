// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Models;

using Microsoft.AspNetCore.Mvc;

#if NET6_0
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProtectedNumbers.Protection;
using ProtectedNumbers.Tests.EndToEnd.Shared.MinimalApi;
#endif


public class SampleObjectSearch
{
  [FromQuery]
  public ProtectedNumber? Id { get; set; }

  [FromQuery]
  public ProtectedNumber[]? Ids { get; set; }

#if NET6_0
  // Minimal APIs in .NET 6 will use this instead of inferring body binding
  public static ValueTask<SampleObjectSearch> BindAsync(HttpContext httpContext)
  {
    // Optional: resolve services if you need to unprotect ids
    IApplicationDataProtector? protector = httpContext.RequestServices.GetService<IApplicationDataProtector>();
    ProtectedNumber? id = null;

    if (httpContext.Request.Query.TryGetValue("id", out var idVals) && idVals.Count > 0)
    {
      id = ProtectedNumber.From(null, idVals[0]);
      if (protector is not null && protector.TryUnprotect(id.Value, out var unp))
      {
        id = unp!.Value;
      }
    }

    ProtectedNumber[]? ids = httpContext.BindProtectedNumberCollection("ids");

    return ValueTask.FromResult(new SampleObjectSearch { Id = id, Ids = ids });
  }
#endif
}
