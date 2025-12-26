// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

using ConfirmSteps.Steps.Http;

using NUnit.Framework.Internal;

using static ConfirmStepsExtensions;

[TestFixtureSource(typeof(WebApiWithEndpointStackFixtureData), nameof(WebApiWithEndpointStackFixtureData.All))]
public class CheckAllEndpointBindingsTests
{
  public CheckAllEndpointBindingsTests(WebApi webApi, EndpointStack endpointStack)
  {
    WebApi = webApi;
    EndpointStack = endpointStack;
  }

  private EndpointStack EndpointStack { get; }

  private WebApi WebApi { get; }

  [Test]
  [CancelAfter(30_000)]
  public async Task AllSteps_Should_Returns_Correctly(CancellationToken cancellationToken)
  {
    // Arrange
    HttpClient? httpClient = WebApiProvider.GetHttpClient(WebApi);

    if (httpClient == null)
    {
      throw new NUnitException();
    }

    Scenario<ScenarioData> scenario =
        Scenario.New<ScenarioData>("[All-Endpoints-Returns-Correctly]")
          .WithServices(s => s.AddExternalHttpClient(httpClient))
          .WithGlobals(b => b
            .UseConst(StepPathPrefix, EndpointStack.PathPrefix)
            .UseObject(UserId, d => d.UserId)
          )
          .WithSteps(s => s
            .GetAll()
            .GetByProtectedId()
            .SearchByProtectedId()
            .SearchObjectByProtectedId()
            .SaveByProtectedId()
            .GetByUnprotectedId()
            .SearchByUnprotectedId()
            .SearchObjectByUnprotectedId()
            .SaveByUnprotectedId()
          )
          .Build()
      ;

    ScenarioData data = new()
    {
      UserId = 26390
    };
    // Act
    ConfirmStepResult<ScenarioData> confirmResult =
      await scenario.ConfirmSteps(data, cancellationToken);
    // Assert
    confirmResult.ShouldBeSuccessfulOnEveryStep(WebApi, EndpointStack);
  }
}
