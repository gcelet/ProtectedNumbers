// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace ProtectedNumbers.Tests.EndToEnd;

using System.Text.Json;

using JsonCons.JsonPath;

using Microsoft.Net.Http.Headers;

public static class StepBuilderExtensions
{
  public const string SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE = "{{" + SAMPLE_OBJECT_PROTECTED_ID + "}}";

  public const string SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE = "{{" + SAMPLE_OBJECT_UNPROTECTED_ID + "}}";

  public const string STEP_PATH_PREFIX = "PATH_PREFIX";

  private const string SAMPLE_OBJECT_PROTECTED_ID = "SAMPLE_OBJECT_PROTECTED_ID";

  private const string SAMPLE_OBJECT_UNPROTECTED_ID = "SAMPLE_OBJECT_UNPROTECTED_ID";

  private const string SAMPLE_OBJECTS_PATH = "samples-objects";

  private const string STEP_PATH_PREFIX_TEMPLATE = "{{" + STEP_PATH_PREFIX + "}}";

  public static IStepBuilderAppender<ScenarioData> GetAll(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-All",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH)
          .WithHeaders(h => h.AppendDefaultHeaders()),
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

            stepContext.ScenarioContext.Data.DefaultProtectedId = stepContext.ScenarioContext.Data.ProtectedIds.FirstOrDefault();
            stepContext.ScenarioContext.Data.Names = nameJsonElements
              .Select(elt => elt.GetString())
              .Where(s => !string.IsNullOrEmpty(s))
              .ToArray()!;

            stepContext.ScenarioContext.Data.DefaultName = stepContext.ScenarioContext.Data.Names.FirstOrDefault();

            stepContext.Vars[SAMPLE_OBJECT_PROTECTED_ID] = stepContext.ScenarioContext.Data.DefaultProtectedId ?? string.Empty;
            stepContext.Vars[SAMPLE_OBJECT_UNPROTECTED_ID] = stepContext.ScenarioContext.Data.DefaultName ?? string.Empty;
          })
      );

  public static IStepBuilderAppender<ScenarioData> GetByProtectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-By-ProtectedId",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH, SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE)
          .WithHeaders(h => h.AppendDefaultHeaders()),
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

  public static IStepBuilderAppender<ScenarioData> GetByUnprotectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-By-UnprotectedId",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH, SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE)
          .WithHeaders(h => h.AppendDefaultHeaders()),
        step => step
          .Verify((response, _) =>
          {
            response.ShouldSatisfyAllConditions("should be a BadRequest response",
              r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
            );
          })
      );

  public static IStepBuilderAppender<ScenarioData> SaveByProtectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("PUT-Save-By-ProtectedId",
        () => RequestBuilder.Put()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH)
          .WithHeaders(h => h.AppendDefaultHeaders())
          .WithBody(@$"{{ ""id"": ""{SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE}"", ""name"": ""E2E Update""}}"),
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

  public static IStepBuilderAppender<ScenarioData> SaveByUnprotectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("PUT-Save-By-UnprotectedId",
        () => RequestBuilder.Put()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH)
          .WithHeaders(h => h.AppendDefaultHeaders())
          .WithBody(@$"{{ ""id"": ""{SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE}"", ""name"": ""E2E Update""}}"),
        step => step
          .Verify((response, _) =>
          {
            response.ShouldSatisfyAllConditions("should be a BadRequest response",
              r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
            );
          })
      );

  public static IStepBuilderAppender<ScenarioData> SearchByProtectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-Search-By-ProtectedId",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH, "search")
          .WithHeaders(h => h.AppendDefaultHeaders())
          .WithQueryString(q => q
            .Append("id", SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE)
            .Append("ids", SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE)
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

  public static IStepBuilderAppender<ScenarioData> SearchByUnprotectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-Search-By-UnprotectedId",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH, "search")
          .WithHeaders(h => h.AppendDefaultHeaders())
          .WithQueryString(q => q
            .Append("id", SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE)
            .Append("ids", SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE)
          ),
        step => step
          .Verify((response, _) =>
          {
            response.ShouldSatisfyAllConditions("should be a BadRequest response",
              r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
            );
          })
      );

  public static IStepBuilderAppender<ScenarioData> SearchObjectByProtectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-SearchObject-By-ProtectedId",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH, "search", "object")
          .WithHeaders(h => h.AppendDefaultHeaders())
          .WithQueryString(q => q
            .Append("id", SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE)
            .Append("ids", SAMPLE_OBJECT_PROTECTED_ID_TEMPLATE)
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

  public static IStepBuilderAppender<ScenarioData> SearchObjectByUnprotectedId(
    this IStepBuilderAppender<ScenarioData> stepBuilderAppender) =>
    stepBuilderAppender
      .HttpStep("GET-SearchObject-By-UnprotectedId",
        () => RequestBuilder.Get()
          .AppendPathSegments(STEP_PATH_PREFIX_TEMPLATE, SAMPLE_OBJECTS_PATH, "search", "object")
          .WithHeaders(h => h.AppendDefaultHeaders())
          .WithQueryString(q => q
            .Append("id", SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE)
            .Append("ids", SAMPLE_OBJECT_UNPROTECTED_ID_TEMPLATE)
          ),
        step => step
          .Verify((response, _) =>
          {
            response.ShouldSatisfyAllConditions("should be a BadRequest response",
              r => r.StatusCode.ShouldBe(HttpStatusCode.BadRequest)
            );
          })
      );

  private static HeaderBuilder AppendDefaultHeaders(this HeaderBuilder headerBuilder) =>
    headerBuilder
      .Header(HeaderNames.ContentType, "application/json");
}
