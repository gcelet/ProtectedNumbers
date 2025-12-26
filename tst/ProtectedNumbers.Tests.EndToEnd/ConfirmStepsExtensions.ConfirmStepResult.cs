// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

public static partial class ConfirmStepsExtensions
{
  extension(ConfirmStepResult<ScenarioData> confirmStepResult)
  {
    public void ShouldBeSuccessfulOnEveryStep(WebApi webApi, EndpointStack endpointStack)
    {
      confirmStepResult.ShouldSatisfyAllConditions(
        $"should be a successful confirm result without exception thrown on [{webApi.Name}-{endpointStack.Name}]",
        r => r.Status.ShouldBe(ConfirmStatus.Success),
        r => r.Exception.ShouldBeNull(),
        r => r.StepResults.ShouldAllBe(sr => sr.State == StepState.Done && sr.Status == ConfirmStatus.Success)
      );
    }
  }
}
