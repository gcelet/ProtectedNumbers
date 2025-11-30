// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Endpoints.SampleObjects;

using FastEndpoints;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;
using ProtectedNumbers.Tests.EndToEnd.Shared.Repositories;
using ProtectedNumbers.Tests.EndToEnd.Shared.Validators;

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
    Validator<SampleObjectValidator>();
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
#if NET6_0
      return SendNotFoundAsync(cancellationToken);
#elif NET8_0_OR_GREATER
      return Send.NotFoundAsync(cancellationToken);
#endif
    }

#if NET6_0
    return SendOkAsync(saved, cancellationToken);
#elif NET8_0_OR_GREATER
    return Send.OkAsync(saved, cancellationToken);
#endif
  }
}
