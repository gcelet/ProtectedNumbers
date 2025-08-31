// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Endpoints.SampleObjects;

using FastEndpoints;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Repositories;

public class GetAllEndpoint : EndpointWithoutRequest<IEnumerable<SampleObject>>
{
  public GetAllEndpoint(SampleObjectRepository repository)
  {
    Repository = repository;
  }

  private SampleObjectRepository Repository { get; }

  public override void Configure()
  {
    AllowAnonymous();
    Get("/samples-objects");
  }

  public override Task HandleAsync(CancellationToken cancellationToken)
  {
    IEnumerable<SampleObject> all = Repository.GetAll();

    return Send.OkAsync(all, cancellationToken);
  }
}
