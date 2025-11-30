// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Endpoints.SampleObjects;

using FastEndpoints;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;
using ProtectedNumbers.Tests.EndToEnd.Shared.Repositories;

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

#if NET6_0
    return SendOkAsync(all, cancellationToken);
#elif NET8_0_OR_GREATER
    return Send.OkAsync(all, cancellationToken);
#endif
  }
}
