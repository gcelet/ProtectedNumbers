// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.MinimalApi;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProtectedNumbers.Protection;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Repositories;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

public static class SampleObjectEndpoints
{
  public static void RegisterSampleObjectEndpoints(this RouteGroupBuilder minimalApi)
  {
    RouteGroupBuilder sampleObjectEndpoints = minimalApi.MapGroup("/samples-objects")
        .AddFluentValidationAutoValidation()
      ;

    sampleObjectEndpoints.MapGet("/", GetAll);
    sampleObjectEndpoints.MapGet("/{id}", GetById);
    sampleObjectEndpoints.MapGet("/search", Search);
    sampleObjectEndpoints.MapGet("/search/object", SearchByObject);
    sampleObjectEndpoints.MapPut("/", Save);
  }

  private static Ok<IEnumerable<SampleObject>> GetAll(SampleObjectRepository repository)
  {
    IEnumerable<SampleObject> all = repository.GetAll();

    return TypedResults.Ok(all);
  }

  private static Results<Ok<SampleObject>, NotFound> GetById(
    SampleObjectRepository repository,
    ProtectedNumber id)
  {
    SampleObject? item = repository.GetById(id);

    if (item is null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(item);
  }

  private static Results<Ok<SampleObject>, NotFound> Save(
    SampleObjectRepository repository,
    [FromBody] SampleObject sampleObject)
  {
    ProtectedNumber? id = sampleObject.Id;
    SampleObject? saved = repository.Save(id, s =>
    {
      s.Name = sampleObject.Name;
    });

    if (saved is null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(saved);
  }

  private static Ok<IEnumerable<SampleObject>> Search(
    SampleObjectRepository repository,
    [FromQuery] ProtectedNumber? id, [FromQuery] ProtectedNumber[]? ids)
  {
    SampleObjectSearch search = new()
    {
      Id = id,
      Ids = ids,
    };

    IEnumerable<SampleObject> result = repository.Search(search);

    return TypedResults.Ok(result);
  }

  private static Ok<IEnumerable<SampleObject>> SearchByObject(
    IApplicationDataProtector applicationDataProtector,
    SampleObjectRepository repository,
    [AsParameters] SampleObjectSearch search)
  {
    IEnumerable<SampleObject> result = repository.Search(search);

    return TypedResults.Ok(result);
  }
}
