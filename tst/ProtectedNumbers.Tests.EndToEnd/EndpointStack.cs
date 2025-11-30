// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

public class EndpointStack
{
  private EndpointStack(string name, string pathPrefix)
  {
    Name = name;
    PathPrefix = pathPrefix;
  }

  public static EndpointStack FastEndpoints { get; } = new("FastEndpoints", "fast-endpoints");

  public static EndpointStack MinimalApi { get; } = new("Minimal Api", "minimal-api");

  public static EndpointStack Mvc { get; } = new("MVC", "mvc");

  public static IEnumerable<EndpointStack> EnumerateStacks()
  {
    yield return Mvc;
    yield return MinimalApi;
    yield return FastEndpoints;
  }

  public string Name { get; }

  public string PathPrefix { get; }

  /// <inheritdoc />
  public override string ToString() => Name;
}
