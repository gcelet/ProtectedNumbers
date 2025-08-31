// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.MinimalApi;

public static class Extensions
{
  public static void RegisterMinimalApiEndpoints(this WebApplication app)
  {
    RouteGroupBuilder minimalApi = app.MapGroup("/minimal-api");

    minimalApi.RegisterSampleObjectEndpoints();
  }
}
