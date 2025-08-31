// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Controllers;

using Microsoft.AspNetCore.Mvc;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Repositories;

[Route("mvc/samples-objects")]
public class SampleObjectController : Controller
{
  [HttpGet("")]
  public IActionResult GetAll([FromServices] SampleObjectRepository repository)
  {
    IEnumerable<SampleObject> allSampleObjects = repository.GetAll();

    return Json(allSampleObjects);
  }

  [HttpGet("{id}")]
  public IActionResult GetById([FromServices] SampleObjectRepository repository, [FromRoute] ProtectedNumber id)
  {
    SampleObject? sampleObject = repository.GetById(id);

    if (sampleObject == null)
    {
      return NotFound();
    }

    return Json(sampleObject);
  }

  [HttpPut("")]
  public IActionResult Save([FromServices] SampleObjectRepository repository,
    [FromBody] SampleObject sampleObject)
  {
    ProtectedNumber? id = sampleObject.Id;
    SampleObject? saved = repository.Save(id, s =>
    {
      s.Name = sampleObject.Name;
    });

    if (saved is null)
    {
      return NotFound();
    }

    return Json(saved);
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

    return Json(result);
  }

  [HttpGet("search/object")]
  public IActionResult SearchByObject([FromServices] SampleObjectRepository repository,
    SampleObjectSearch search)
  {
    IEnumerable<SampleObject> result = repository.Search(search);

    return Json(result);
  }
}
