ProtectedNumbers – Project Guidelines for Advanced Contributors

Last updated: 2025-08-22 21:25 (local)

Scope
- This document captures project-specific practices to build, test, and extend ProtectedNumbers. It assumes familiarity with .NET, ASP.NET Core, NUnit, Shouldly, dependency injection, and System.Text.Json.

Repository layout
- ProtectedNumbers.sln – solution entry point.
- src\ProtectedNumbers – core library implementing the ProtectedNumber struct and integration helpers.
- samples\ProtectedNumbers.Samples – ASP.NET Core sample application (controllers + minimal API) demonstrating usage.
- tst\ProtectedNumbers.Tests – NUnit test suite for the library.
- build.ps1 / build.cmd / build.sh – helper scripts for CI/local usage.
- GitVersion.yml – versioning via GitVersion; packaging/assembly version derives from branch/commit state.

Build, configuration, and integration notes
1) Building the solution
- Quick build: dotnet build .\ProtectedNumbers.sln -c Debug
- Full local build (Windows PS): .\build.ps1  or .\build.cmd
  - These scripts are CI-friendly wrappers. Prefer them for consistent versioning and packaging behavior.
- Target frameworks: solution includes targets like net6.0, net8.0, and net9.0 where applicable. Multi-targeted code uses conditional compilation symbols (e.g., NET8_0_OR_GREATER) around MVC model-binding plumbing.

2) ASP.NET Core integration
- Register services via AddProtectedNumbers():
  - Adds IHttpContextAccessor.
  - Configures System.Text.Json to make IServiceProvider discoverable by converters.
  - Registers IApplicationDataProtector and default IApplicationDataPreparator.
  - Wires MVC model binding for ProtectedNumber / ProtectedNumber? types.
- Model binding order: ProtectedNumbers.Internal.ProtectedNumbersConfigureMvcOptions inserts the ProtectedNumberModelBinderProvider just before TryParseModelBinderProvider when available, ensuring ProtectedNumber takes precedence over TryParse-based binders.
- Minimal API sample endpoints: see samples\ProtectedNumbers.Samples\MinimalApi\SampleObjectEndpoints.cs for concrete patterns:
  - Binding of ProtectedNumber and ProtectedNumber? from route/query.
  - Unprotecting user-provided ids with IApplicationDataProtector.TryUnprotect and returning ValidationProblem when malformed.

3) System.Text.Json considerations
- Extensions.TryAddServiceProvider(JsonSerializerOptions, IServiceProvider, IHttpContextAccessor?) conditionally injects a “dummy” converter (ServiceProviderDummySystemTextJsonConverter) to expose an IServiceProvider for other converters that need scoped services at runtime.
- This operation is idempotent; avoid manually adding duplicate service-provider exposing converters.

4) ProtectedNumber semantics (high level)
- ProtectedNumber is a struct with dual representation: a clear Value (long) and a ProtectedValue (string) that may be present independently.
- Empty sentinel: ProtectedNumber.Empty is initialized but HasValue == false and HasProtectedValue == false. Accessing Value or ProtectedValue on such instance throws InvalidOperationException.
- Comparisons, equality, and parsing are implemented. There are helpers for TryParse and for ASP.NET binding (BindAsync/TryBind).
- Utility: ProtectedNumber.IsNullOrEmpty(ProtectedNumber?) checks both null and Empty/Ø-Ø case.

5) Packaging & versioning
- GitVersion drives semantic versioning; ensure branch naming aligns with GitVersion.yml rules. Tagged releases pack with the correct version.

Testing information
1) Test framework & libraries
- NUnit is used ([Test], [TestCase], [TestCaseSource], etc.).
- Assertions use Shouldly (e.g., x.ShouldBe(y), x.ShouldBeTrue()).
- Some tests rely on local substitutes/adapters (see tst\ProtectedNumbers.Tests\UnitTestProtectionAlgorithm.cs and SubstituteExtensions.cs) to exercise protection behavior without external secrets.

2) Running tests
- Run all tests across the solution:
  - dotnet test .\ProtectedNumbers.sln -c Debug
- Filter by fully qualified name (FQN) or substring:
  - Example (FQN): dotnet test .\tst\ProtectedNumbers.Tests\ProtectedNumbers.Tests.csproj --filter "FullyQualifiedName~ProtectedNumbers.Tests.ProtectedNumberTests"
  - Example (single test): dotnet test .\tst\ProtectedNumbers.Tests\ProtectedNumbers.Tests.csproj --filter "FullyQualifiedName=ProtectedNumbers.Tests.ProtectedNumberTests.IsNullOrEmpty_Should_Returns_Correctly"
- Note: Inconclusive tests may appear (e.g., a model-binding scenario gated by environment). These are expected and should not fail CI.

3) Adding a new test (demonstrated/verified locally)
- Create a new file under tst\ProtectedNumbers.Tests (namespace ProtectedNumbers.Tests) and add an NUnit test. Example content that was verified locally during guideline authoring:

  File: tst\\ProtectedNumbers.Tests\\GuidelinesDemoTests.cs
  namespace ProtectedNumbers.Tests;
  public class GuidelinesDemoTests
  {
      [Test]
      public void Guidelines_Demo_Empty_IsInitialized()
      {
          var empty = ProtectedNumber.Empty;
          empty.IsInitialized().ShouldBeTrue();
          empty.HasValue.ShouldBeFalse();
      }
  }

- Run it specifically:
  - dotnet test .\tst\ProtectedNumbers.Tests\ProtectedNumbers.Tests.csproj --filter "FullyQualifiedName=ProtectedNumbers.Tests.GuidelinesDemoTests.Guidelines_Demo_Empty_IsInitialized"
- Result from local verification (for documentation only): the test passed alongside the existing suite. After verification, the demo file was removed to avoid adding noise to the repo.

4) Writing meaningful tests for ProtectedNumber
- Prefer using existing named instances/builders in the test suite (see ProtectedNumberTests.* partials) to cover edge cases like:
  - Value present + protected value invalid/valid.
  - Value missing + protected value present/invalid.
  - Empty/Ø-Ø sentinel behavior and exception throwing on invalid access.
- For ASP.NET Core binding tests, prefer using the existing binding helpers and data protection test adapters to avoid flakiness. The suite already contains comprehensive binding cases; replicate their setup if adding new coverage.

5) Test isolation & determinism
- Avoid depending on machine-specific data protection infrastructure in unit tests. Favor the UnitTestProtectionAlgorithm and abstractions already present under tst to keep tests deterministic.
- If you must interact with System.Text.Json converters that require IServiceProvider, use Extensions.TryAddServiceProvider to simulate hosting context in tests.

Additional development guidelines
1) Code style & analyzers
- The codebase uses modern C# and nullable reference types. Pay attention to [NotNullWhen(false)] annotations and the nullability contracts in ProtectedNumber methods.
- Follow existing naming patterns and test naming (Method_Should_Behavior) and for parametrized tests use [TestCase]/[TestCaseSource] with SetName to generate readable cases.

2) API design notes
- ProtectedNumber is a value type; avoid inadvertent boxing when adding interfaces/overrides. Use the provided Equals/CompareTo overloads for long/string/ProtectedNumber where possible.
- Enforce initialization via EnsureInitialized/IsInitialized in code paths that may operate on default(ProtectedNumber).
- For ASP.NET Core, rely on the built-in model binder provider wired by ProtectedNumbersConfigureMvcOptions to bind ProtectedNumber and ProtectedNumber?. Avoid registering competing binders that would shadow its position relative to TryParseModelBinderProvider.

3) Samples & manual testing
- The sample app demonstrates endpoint-level usage. Useful files:
  - samples\\ProtectedNumbers.Samples\\Program.cs – service registration with AddProtectedNumbers.
  - samples\\ProtectedNumbers.Samples\\MinimalApi\\SampleObjectEndpoints.cs – binding, unprotecting, and validation examples.
  - samples\\ProtectedNumbers.Samples\\ProtectedNumbers.Samples.Endpoints.http – HTTP request examples for local testing.
- Run the samples app (typical):
  - dotnet run --project .\samples\ProtectedNumbers.Samples\ProtectedNumbers.Samples.csproj

4) Versioning and releases
- Use GitVersion-compatible branching. Packaging derives versions automatically in CI (see build scripts). When cutting releases, ensure tags are pushed so NuGet package versions align.

Appendix – Verified commands during this guideline authoring
- Build: dotnet build .\ProtectedNumbers.sln -c Debug (succeeded)
- Test (all): dotnet test .\ProtectedNumbers.sln -c Debug (272 passed; 1 inconclusive before demo; 273 passed; 1 inconclusive with demo)
- Test (single demo): dotnet test .\tst\ProtectedNumbers.Tests\ProtectedNumbers.Tests.csproj --filter "FullyQualifiedName=ProtectedNumbers.Tests.GuidelinesDemoTests.Guidelines_Demo_Empty_IsInitialized" (passed)
- Clean-up: the demo test file was removed after verification. No persistent changes remain outside this guidelines file.
