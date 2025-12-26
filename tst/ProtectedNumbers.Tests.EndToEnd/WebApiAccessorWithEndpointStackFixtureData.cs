// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace ProtectedNumbers.Tests.EndToEnd;

using System.Collections;

public static class WebApiWithEndpointStackFixtureData
{
  public static IEnumerable All()
  {
    foreach (WebApi webApi in WebApi.EnumerateWebApi())
    {
      foreach (EndpointStack endpointStack in EndpointStack.EnumerateStacks())
      {
        yield return new TestFixtureData(webApi, endpointStack);
      }
    }
  }

  public static IEnumerable OnlyCustoms()
  {
    foreach (WebApi webApi in WebApi.EnumerateCustomWebApi())
    {
      foreach (EndpointStack endpointStack in EndpointStack.EnumerateStacks())
      {
        yield return new TestFixtureData(webApi, endpointStack);
      }
    }
  }

  public static IEnumerable OnlyDefaults()
  {
    foreach (WebApi webApi in WebApi.EnumerateDefaultWebApi())
    {
      foreach (EndpointStack endpointStack in EndpointStack.EnumerateStacks())
      {
        yield return new TestFixtureData(webApi, endpointStack);
      }
    }
  }
}
