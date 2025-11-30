// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

using ConfirmSteps.Steps.Http;

using NUnit.Framework.Internal;

using static StepBuilderExtensions;

[TestFixtureSource(typeof(WebApiWithEndpointStackFixtureData), nameof(WebApiWithEndpointStackFixtureData.FixtureParams))]
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
  public async Task AllSteps_Should_Returns_Correctly()
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
            .UseConst(STEP_PATH_PREFIX, EndpointStack.PathPrefix)
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
    ScenarioData data = new();
    // Act
    using CancellationTokenSource cts = new();
    ConfirmStepResult<ScenarioData> confirmResult =
      await scenario.ConfirmSteps(data, cts.Token);
    // Assert
    confirmResult.ShouldSatisfyAllConditions($"should be a successful confirm result without exception thrown on [{WebApi.Name}-{EndpointStack.Name}]",
      r => r.Status.ShouldBe(ConfirmStatus.Success),
      r => r.Exception.ShouldBeNull(),
      r => r.StepResults.ShouldAllBe(sr => sr.State == StepState.Done && sr.Status == ConfirmStatus.Success)
    );
  }
}
