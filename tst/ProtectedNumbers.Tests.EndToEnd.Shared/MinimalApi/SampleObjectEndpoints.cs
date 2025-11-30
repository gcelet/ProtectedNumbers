// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.MinimalApi;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;
using ProtectedNumbers.Tests.EndToEnd.Shared.Repositories;
using ProtectedNumbers.Tests.EndToEnd.Shared.Validators;

#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Routing;

using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
#endif

public static class SampleObjectEndpoints
{
#if NET6_0
  public static void RegisterSampleObjectEndpoints(this WebApplication app, string prefix)
  {
    string groupPath = $"{prefix}/samples-objects";

    app.MapGet($"{groupPath}/", GetAll);
    app.MapGet(groupPath + "/{id}", GetById);
    app.MapGet($"{groupPath}/search", Search);
    app.MapGet($"{groupPath}/search/object", SearchByObject);
    app.MapPut($"{groupPath}/", Save);
  }
#elif NET8_0_OR_GREATER
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
#endif

  private static Ok<IEnumerable<SampleObject>> GetAll(SampleObjectRepository repository)
  {
    IEnumerable<SampleObject> all = repository.GetAll();

    return TypedResults.Ok(all);
  }

  private static Results<Ok<SampleObject>, BadRequest, NotFound> GetById(
    SampleObjectRepository repository,
    ProtectedNumber id)
  {
    if (!id.HaveProtectedValueAndValue())
    {
      return TypedResults.BadRequest();
    }

    SampleObject? item = repository.GetById(id);

    if (item is null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(item);
  }

  private static Results<Ok<SampleObject>, NotFound, BadRequest<ValidationProblemDetails>> Save(
    SampleObjectRepository repository,
#if NET6_0
    [FromServices] IValidator<SampleObject> validator,
#endif
    [FromBody] SampleObject sampleObject)
  {
#if NET6_0
    // NOTE: Manual validation since FluentValidation AutoValidation is not available for Minimal APIs in .NET 6
    var validationResult = validator.Validate(sampleObject);
    if (!validationResult.IsValid)
    {
      ValidationProblemDetails problemDetails = validationResult.ToValidationProblemDetails();
      return TypedResults.BadRequest(problemDetails);
    }
#endif

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

  private static Results<Ok<IEnumerable<SampleObject>>, BadRequest<ValidationProblemDetails>> Search(
#if NET6_0
    HttpContext httpContext,
#endif
    [FromServices] SampleObjectRepository repository,
    [FromServices] IValidator<SampleObjectSearch> validator,
    [FromQuery] ProtectedNumber? id
#if NET8_0_OR_GREATER
    , [FromQuery] ProtectedNumber[]? ids
#endif
  )
  {
#if NET6_0
    ProtectedNumber[]? ids = httpContext.BindProtectedNumberCollection("ids");
#endif
    SampleObjectSearch search = new()
    {
      Id = id,
      Ids = ids,
    };

    // NOTE: Manual validation since independant parameters binding does not support FluentValidation AutoValidation
    ValidationResult? validationResult = validator.Validate(search);
    if (!validationResult.IsValid)
    {
      ValidationProblemDetails problemDetails = validationResult.ToValidationProblemDetails();

      return TypedResults.BadRequest(problemDetails);
    }

    IEnumerable<SampleObject> result = repository.Search(search);
    return TypedResults.Ok(result);
  }

  private static Results<Ok<IEnumerable<SampleObject>>, BadRequest<ValidationProblemDetails>> SearchByObject(
    [FromServices] SampleObjectRepository repository,
#if NET6_0
    [FromServices] IValidator<SampleObjectSearch> validator,
#endif
#if NET8_0_OR_GREATER
    [AsParameters]
#endif
    SampleObjectSearch search)
  {
#if NET6_0
    // NOTE: Manual validation since FluentValidation AutoValidation is not available for Minimal APIs in .NET 6
    var validationResult = validator.Validate(search);
    if (!validationResult.IsValid)
    {
      ValidationProblemDetails problemDetails = validationResult.ToValidationProblemDetails();
      return TypedResults.BadRequest(problemDetails);
    }
#endif

    IEnumerable<SampleObject> result = repository.Search(search);

    return TypedResults.Ok(result);
  }
}
