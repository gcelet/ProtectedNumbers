// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Endpoints.SampleObjects;

using FastEndpoints;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Repositories;

public class SearchEndpoint : Endpoint<SampleObjectSearch, IEnumerable<SampleObject>>
{
  public SearchEndpoint(SampleObjectRepository repository)
  {
    Repository = repository;
  }

  private SampleObjectRepository Repository { get; }

  public override void Configure()
  {
    AllowAnonymous();
    Get("/samples-objects/search", "/samples-objects/search/object");
  }

  public override Task HandleAsync(SampleObjectSearch input, CancellationToken cancellationToken)
  {
    IEnumerable<SampleObject> result = Repository.Search(input);

    return Send.OkAsync(result, cancellationToken);
  }
}
