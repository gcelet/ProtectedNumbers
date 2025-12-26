// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

using System.Text.Json;

using JsonCons.JsonPath;

public static partial class ConfirmStepsExtensions
{
  public const string StepPathPrefix = "PATH_PREFIX";

  public const string UserId = "USER_ID";

  private const string SampleObjectProtectedId = "SAMPLE_OBJECT_PROTECTED_ID";

  private const string SampleObjectProtectedIdTemplate = "{{" + SampleObjectProtectedId + "}}";

  private const string SampleObjectsPath = "samples-objects";

  private const string SampleObjectUnprotectedId = "SAMPLE_OBJECT_UNPROTECTED_ID";

  private const string SampleObjectUnprotectedIdTemplate = "{{" + SampleObjectUnprotectedId + "}}";

  private const string StepPathPrefixTemplate = "{{" + StepPathPrefix + "}}";

  extension(IStepBuilderAppender<ScenarioData> stepBuilderAppender)
  {
    public IStepBuilderAppender<ScenarioData> GetAll() =>
      stepBuilderAppender
        .HttpStep("GET-All",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath)
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            ),
          step => step
            .VerifyJson((response, stepContext) =>
            {
              response.ShouldSatisfyAllConditions("should be an OK JSON response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.OK),
                r => r.Response.ShouldNotBeNull()
              );

              JsonElement rootElement = response.Response!.RootElement;

              rootElement.ShouldSatisfyAllConditions("should be a JSON array with at least one item",
                e => e.ValueKind.ShouldBe(JsonValueKind.Array),
                e => e.GetArrayLength().ShouldBeGreaterThan(0)
              );

              IList<JsonElement> idJsonElements = JsonSelector.Select(response.Response.RootElement, "$[*].id");
              IList<JsonElement> nameJsonElements = JsonSelector.Select(response.Response.RootElement, "$[*].name");

              idJsonElements.ShouldAllBe(elt => elt.ValueKind == JsonValueKind.String, "should all be JSON string");
              nameJsonElements.ShouldAllBe(elt => elt.ValueKind == JsonValueKind.String, "should all be JSON string");

              // TODO: Should be in Extract instead of VerifyJson: need to add feature in ConfirmSteps.Net
              stepContext.ScenarioContext.Data.ProtectedIds = idJsonElements
                .Select(elt => elt.GetString())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray()!;

              stepContext.ScenarioContext.Data.DefaultProtectedId =
                stepContext.ScenarioContext.Data.ProtectedIds.FirstOrDefault();

              stepContext.ScenarioContext.Data.Names = nameJsonElements
                .Select(elt => elt.GetString())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray()!;

              stepContext.ScenarioContext.Data.DefaultName = stepContext.ScenarioContext.Data.Names.FirstOrDefault();

              stepContext.Vars[SampleObjectProtectedId] = stepContext.ScenarioContext.Data.DefaultProtectedId ?? string.Empty;
              stepContext.Vars[SampleObjectUnprotectedId] = stepContext.ScenarioContext.Data.DefaultName ?? string.Empty;
            })
        );

    public IStepBuilderAppender<ScenarioData> GetByProtectedId() =>
      stepBuilderAppender
        .HttpStep("GET-By-ProtectedId",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath, SampleObjectProtectedIdTemplate)
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            ),
          step => step
            .VerifyJson((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be an OK JSON response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.OK),
                r => r.Response.ShouldNotBeNull()
              );

              JsonElement rootElement = response.Response!.RootElement;

              rootElement.ShouldSatisfyAllConditions("should be a JSON object",
                e => e.ValueKind.ShouldBe(JsonValueKind.Object)
              );

              IList<JsonElement> idJsonElements = JsonSelector.Select(response.Response.RootElement, "$.id");

              idJsonElements.ShouldAllBe(elt => elt.ValueKind == JsonValueKind.String, "should all be JSON string");
            })
        );

    public IStepBuilderAppender<ScenarioData> GetByUnprotectedId() =>
      stepBuilderAppender
        .HttpStep("GET-By-UnprotectedId",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath, SampleObjectUnprotectedIdTemplate)
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            ),
          step => step
            .Verify((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be a BadRequest response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
              );
            })
        );

    public IStepBuilderAppender<ScenarioData> SaveByProtectedId() =>
      stepBuilderAppender
        .HttpStep("PUT-Save-By-ProtectedId",
          () => RequestBuilder.Put()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath)
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            )
            .WithBody(@$"{{ ""id"": ""{SampleObjectProtectedIdTemplate}"", ""name"": ""E2E Update""}}"),
          step => step
            .VerifyJson((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be an OK JSON response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.OK),
                r => r.Response.ShouldNotBeNull()
              );

              JsonElement rootElement = response.Response!.RootElement;

              rootElement.ShouldSatisfyAllConditions("should be a JSON object",
                e => e.ValueKind.ShouldBe(JsonValueKind.Object)
              );

              IList<JsonElement> idJsonElements = JsonSelector.Select(response.Response.RootElement, "$.id");

              idJsonElements.ShouldAllBe(elt => elt.ValueKind == JsonValueKind.String, "should all be JSON string");
            })
        );

    public IStepBuilderAppender<ScenarioData> SaveByUnprotectedId() =>
      stepBuilderAppender
        .HttpStep("PUT-Save-By-UnprotectedId",
          () => RequestBuilder.Put()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath)
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            )
            .WithBody(@$"{{ ""id"": ""{SampleObjectUnprotectedIdTemplate}"", ""name"": ""E2E Update""}}"),
          step => step
            .Verify((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be a BadRequest response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
              );
            })
        );

    public IStepBuilderAppender<ScenarioData> SearchByProtectedId() =>
      stepBuilderAppender
        .HttpStep("GET-Search-By-ProtectedId",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath, "search")
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            )
            .WithQueryString(q => q
              .Append("id", SampleObjectProtectedIdTemplate)
              .Append("ids", SampleObjectProtectedIdTemplate)
            ),
          step => step
            .VerifyJson((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be an OK JSON response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.OK),
                r => r.Response.ShouldNotBeNull()
              );

              JsonElement rootElement = response.Response!.RootElement;

              rootElement.ShouldSatisfyAllConditions("should be a JSON array with at least one item",
                e => e.ValueKind.ShouldBe(JsonValueKind.Array),
                e => e.GetArrayLength().ShouldBe(1)
              );

              IList<JsonElement> idJsonElements = JsonSelector.Select(response.Response.RootElement, "$[*].id");

              idJsonElements.ShouldAllBe(elt => elt.ValueKind == JsonValueKind.String, "should all be JSON string");
            })
        );

    public IStepBuilderAppender<ScenarioData> SearchByUnprotectedId() =>
      stepBuilderAppender
        .HttpStep("GET-Search-By-UnprotectedId",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath, "search")
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            )
            .WithQueryString(q => q
              .Append("id", SampleObjectUnprotectedIdTemplate)
              .Append("ids", SampleObjectUnprotectedIdTemplate)
            ),
          step => step
            .Verify((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be a BadRequest response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
              );
            })
        );

    public IStepBuilderAppender<ScenarioData> SearchObjectByProtectedId() =>
      stepBuilderAppender
        .HttpStep("GET-SearchObject-By-ProtectedId",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath, "search", "object")
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            )
            .WithQueryString(q => q
              .Append("id", SampleObjectProtectedIdTemplate)
              .Append("ids", SampleObjectProtectedIdTemplate)
            ),
          step => step
            .VerifyJson((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be an OK JSON response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.OK),
                r => r.Response.ShouldNotBeNull()
              );

              JsonElement rootElement = response.Response!.RootElement;

              rootElement.ShouldSatisfyAllConditions("should be a JSON array with at least one item",
                e => e.ValueKind.ShouldBe(JsonValueKind.Array),
                e => e.GetArrayLength().ShouldBe(1)
              );

              IList<JsonElement> idJsonElements = JsonSelector.Select(response.Response.RootElement, "$[*].id");

              idJsonElements.ShouldAllBe(elt => elt.ValueKind == JsonValueKind.String, "should all be JSON string");
            })
        );

    public IStepBuilderAppender<ScenarioData> SearchObjectByUnprotectedId() =>
      stepBuilderAppender
        .HttpStep("GET-SearchObject-By-UnprotectedId",
          () => RequestBuilder.Get()
            .AppendPathSegments(StepPathPrefixTemplate, SampleObjectsPath, "search", "object")
            .WithHeaders(h => h
              .AppendDefaultHeaders()
              .AppendVaryHeaders()
            )
            .WithQueryString(q => q
              .Append("id", SampleObjectUnprotectedIdTemplate)
              .Append("ids", SampleObjectUnprotectedIdTemplate)
            ),
          step => step
            .Verify((response, _) =>
            {
              response.ShouldSatisfyAllConditions("should be a BadRequest response",
                r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
              );
            })
        );
  }
}
