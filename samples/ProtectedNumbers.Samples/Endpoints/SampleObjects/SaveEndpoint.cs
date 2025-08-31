// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Endpoints.SampleObjects;

using FastEndpoints;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Repositories;

public class SaveEndpoint : Endpoint<SampleObject, SampleObject>
{
  public SaveEndpoint(SampleObjectRepository repository)
  {
    Repository = repository;
  }

  private SampleObjectRepository Repository { get; }

  public override void Configure()
  {
    AllowAnonymous();
    Put("/samples-objects");
  }

  public override Task HandleAsync(SampleObject input, CancellationToken cancellationToken)
  {
    SampleObject? saved = Repository.Save(input.Id, s =>
    {
      s.Name = input.Name;
    });

    if (saved == null)
    {
      return Send.NotFoundAsync(cancellationToken);
    }

    return Send.OkAsync(saved, cancellationToken);
  }
}
