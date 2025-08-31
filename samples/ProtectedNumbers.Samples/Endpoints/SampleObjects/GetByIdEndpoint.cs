// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Endpoints.SampleObjects;

using FastEndpoints;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Models.Inputs;
using ProtectedNumbers.Samples.Repositories;

public class GetByIdEndpoint : Endpoint<GetByIdInput, SampleObject>
{
  public GetByIdEndpoint(SampleObjectRepository repository)
  {
    Repository = repository;
  }

  private SampleObjectRepository Repository { get; }

  public override void Configure()
  {
    AllowAnonymous();
    Get("/samples-objects/{id}");
  }

  public override Task HandleAsync(GetByIdInput input, CancellationToken cancellationToken)
  {
    SampleObject? sampleObject = Repository.GetById(input.Id.GetValueOrDefault(ProtectedNumber.Empty));

    if (sampleObject == null)
    {
      return Send.NotFoundAsync(cancellationToken);
    }

    return Send.OkAsync(sampleObject, cancellationToken);
  }
}
