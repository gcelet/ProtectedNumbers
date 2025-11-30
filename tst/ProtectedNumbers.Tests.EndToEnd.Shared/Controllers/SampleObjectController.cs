// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Controllers;

#if NET6_0
using FluentValidation;

using ProtectedNumbers.Tests.EndToEnd.Shared.Validators;
#endif

using Microsoft.AspNetCore.Mvc;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;
using ProtectedNumbers.Tests.EndToEnd.Shared.Repositories;

[ApiController]
[Route("mvc/samples-objects")]
public class SampleObjectController : ControllerBase
{
  [HttpGet("")]
  public IActionResult GetAll([FromServices] SampleObjectRepository repository)
  {
    IEnumerable<SampleObject> allSampleObjects = repository.GetAll();

    return Ok(allSampleObjects);
  }

  [HttpGet("{id}")]
  public IActionResult GetById(
    [FromServices] SampleObjectRepository repository,
    [FromRoute] ProtectedNumber id)
  {
    SampleObject? sampleObject = repository.GetById(id);

    if (sampleObject == null)
    {
      return NotFound();
    }

    return Ok(sampleObject);
  }

  [HttpPut("")]
  public IActionResult Save([FromServices] SampleObjectRepository repository,
#if NET6_0
    [FromServices] IValidator<SampleObject> validator,
#endif
    [FromBody] SampleObject sampleObject)
  {
#if NET6_0
    // NOTE: Manual validation since FluentValidation AutoValidation is not available for MVC in .NET 6
    var validationResult = validator.Validate(sampleObject);
    if (!validationResult.IsValid)
    {
      ValidationProblemDetails validationProblemDetails = validationResult.ToValidationProblemDetails();
      return BadRequest(validationProblemDetails);
    }
#endif

    ProtectedNumber? id = sampleObject.Id;
    SampleObject? saved = repository.Save(id, s =>
    {
      s.Name = sampleObject.Name;
    });

    if (saved is null)
    {
      return NotFound();
    }

    return Ok(saved);
  }

  [HttpGet("search")]
  public IActionResult Search([FromServices] SampleObjectRepository repository,
    [FromQuery] ProtectedNumber? id, [FromQuery] ProtectedNumber[]? ids)
  {
    SampleObjectSearch search = new()
    {
      Id = id,
      Ids = ids,
    };

    IEnumerable<SampleObject> result = repository.Search(search);

    return Ok(result);
  }

  [HttpGet("search/object")]
  public IActionResult SearchByObject([FromServices] SampleObjectRepository repository,
    #if NET6_0
    [FromQuery]
    #endif
    SampleObjectSearch search)
  {
    IEnumerable<SampleObject> result = repository.Search(search);

    return Ok(result);
  }
}
