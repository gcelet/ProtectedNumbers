// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using ProtectedNumbers.Protection;

public partial class ProtectedNumberTests
{
    private static void DoJsonDeserializationAndAssertResult(string? json, JsonSerializerOptions jsonSerializerOptions,
        ProtectedNumber? idExpected, Exception? exceptionExpected)
    {
        if (json == null)
        {
            Assert.Fail("can't use null string as input for parse json");
            return;
        }

        // Act
        ProtectedNumber? idActual = null;

        if (exceptionExpected == null)
        {
            idActual = jsonSerializerOptions.ParseJson<ProtectedNumber?>(json);
        }
        else
        {
            Should.Throw(() => { idActual = jsonSerializerOptions.ParseJson<ProtectedNumber?>(json); },
                exceptionExpected.GetType());
        }

        // Assert
        if (exceptionExpected == null)
        {
            idActual.ShouldBe(idExpected);
        }
    }

    private static void DoJsonSerializationAndAssertResult(ProtectedNumber? input,
        JsonSerializerOptions jsonSerializerOptions,
        string? jsonExcepted, Exception? exceptionExpected)
    {
        // Act
        string jsonActual = "";

        if (exceptionExpected == null)
        {
            jsonActual = jsonSerializerOptions.ToJson(input);
        }
        else
        {
            Should.Throw(() => { jsonActual = jsonSerializerOptions.ToJson(input); },
                exceptionExpected.GetType());
        }

        // Assert
        if (exceptionExpected == null)
        {
            jsonActual.ShouldBe(jsonExcepted);
        }
    }

    private static JsonSerializerOptions GetJsonSerializerOptions(JsonSerializerOptions? jsonSerializerOptions,
        bool includeServiceProvider, bool includeHttpContext, bool includeApplicationDataProtector)
    {
        if (jsonSerializerOptions != null)
        {
            return jsonSerializerOptions;
        }

        if (!includeServiceProvider)
        {
            jsonSerializerOptions =
                JsonExtensions.BuildJsonSerializerOptions();
            return jsonSerializerOptions;
        }

        IHttpContextAccessor substituteForHttpContextAccessor = Substitute.For<IHttpContextAccessor>();

        if (includeHttpContext)
        {
            substituteForHttpContextAccessor.SetupHttpContextAsDefault();
        }
        else
        {
            substituteForHttpContextAccessor.SetupHttpContextAsNull();
        }

        IServiceProvider substituteForServiceProvider = Substitute.For<IServiceProvider>();

        substituteForServiceProvider.SetupAllServiceThrows();

        jsonSerializerOptions =
            JsonExtensions.BuildJsonSerializerOptions(substituteForServiceProvider, substituteForHttpContextAccessor);

        if (!includeApplicationDataProtector)
        {
            substituteForServiceProvider.SetupForService<IApplicationDataProtector>(null);
            return jsonSerializerOptions;
        }

        IApplicationDataProtector substituteForApplicationDataProtector = Substitute.For<IApplicationDataProtector>();

        substituteForApplicationDataProtector.SetupForApplicationDataProtector();
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        substituteForServiceProvider.SetupForService<IApplicationDataProtector>(substituteForApplicationDataProtector);

        return jsonSerializerOptions;
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateJsonDeserializationTestCases))]
    public void JsonDeserialization_Should_Work_Correctly_When_NoProtectionServiceAvailable(
        ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions(testCase.JsonSerializerOptions, true, false, false);
        // Act & Asset
        DoJsonDeserializationAndAssertResult(testCase.StringValue, jsonSerializerOptions,
            testCase.OtherInstance, testCase.ExceptionExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateJsonDeserializationTestCases))]
    public void JsonDeserialization_Should_Work_Correctly_When_NoServiceProviderAvailable(
        ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions(testCase.JsonSerializerOptions, false, false, false);
        // Act & Asset
        DoJsonDeserializationAndAssertResult(testCase.StringValue, jsonSerializerOptions,
            testCase.OtherInstance, testCase.ExceptionExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateJsonDeserializationTestCases))]
    public void JsonDeserialization_Should_Work_Correctly_When_ProtectionServiceAvailable(
        ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions(testCase.JsonSerializerOptions, true, false, true);
        // Act & Asset
        DoJsonDeserializationAndAssertResult(testCase.StringValue, jsonSerializerOptions,
            testCase.AnotherInstance, testCase.ExceptionExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateJsonSerializationTestCases))]
    public void JsonSerialization_Should_Work_Correctly_When_NoProtectionServiceAvailable(
        ProtectedNumberTestCase testCase)
    {
        // Inputs
        ProtectedNumber? input = testCase.Instance;
        // Arrange
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions(testCase.JsonSerializerOptions, true, false, false);
        // Act & Asset
        DoJsonSerializationAndAssertResult(input, jsonSerializerOptions, testCase.StringValue,
            testCase.ExceptionExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateJsonSerializationTestCases))]
    public void JsonSerialization_Should_Work_Correctly_When_NoServiceProviderAvailable(
        ProtectedNumberTestCase testCase)
    {
        // Inputs
        ProtectedNumber? input = testCase.Instance;
        // Arrange
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions(testCase.JsonSerializerOptions, false, false, false);
        // Act & Asset
        DoJsonSerializationAndAssertResult(input, jsonSerializerOptions, testCase.StringValue,
            testCase.ExceptionExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateJsonSerializationTestCases))]
    public void JsonSerialization_Should_Work_Correctly_When_ProtectionServiceAvailable(
        ProtectedNumberTestCase testCase)
    {
        // Inputs
        ProtectedNumber? input = testCase.Instance;
        // Arrange
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions(testCase.JsonSerializerOptions, true, false, true);
        // Act & Asset
        DoJsonSerializationAndAssertResult(input, jsonSerializerOptions, testCase.OtherStringValue,
            testCase.ExceptionExpected);
    }

    public class ProtectedNumberJsonValueNameInstance
    {
        public static ProtectedNumberJsonValueNameInstance Empty { get; } = new()
        {
            Name = "Ø",
            Create = () => QuoteValue(string.Empty)
        };

        public static ProtectedNumberJsonValueNameInstance InvalidProtectedValue1 { get; } = new()
        {
            Name = "IP1",
            Create = () => QuoteValue(ProtectedNumberTestNamedInstance.INVALID_PROTECTION_VALUE1)
        };

        public static ProtectedNumberJsonValueNameInstance InvalidProtectedValue2 { get; } = new()
        {
            Name = "IP2",
            Create = () => QuoteValue(ProtectedNumberTestNamedInstance.INVALID_PROTECTION_VALUE2)
        };

        public static ProtectedNumberJsonValueNameInstance Null { get; } = new()
        {
            Name = "null",
            Create = () => "null"
        };

        public static ProtectedNumberJsonValueNameInstance OneProtectedValue { get; } = new()
        {
            Name = "1P",
            Create = () => QuoteValue(1L.ProtectUsingUnitTestAlgorithm())
        };

        public static ProtectedNumberJsonValueNameInstance Throw { get; } = new()
        {
            Name = "throw",
            Create = () => "throw new Exception();"
        };

        public static ProtectedNumberJsonValueNameInstance TwoProtectedValue { get; } = new()
        {
            Name = "2P",
            Create = () => QuoteValue(2L.ProtectUsingUnitTestAlgorithm())
        };

        private static string QuoteValue(string value)
        {
            return $"\"{JsonEncodedText.Encode(value)}\"";
        }

        public required Func<string> Create { get; init; }

        public required string Name { get; init; }
    }

    public static partial class ProtectedNumberTestCases
    {
        public static IEnumerable<TestCaseData> EnumerateJsonDeserializationTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // VP
            yield return BuildJsonDeserializationTestCase("01-[1P -> Ø-1P / 1-1P]", testNumber++,
                ProtectedNumberJsonValueNameInstance.OneProtectedValue,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One);
            yield return BuildJsonDeserializationTestCase("02-[2P -> Ø-2P / 2-2P]", testNumber++,
                ProtectedNumberJsonValueNameInstance.TwoProtectedValue,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two);
            // IP
            yield return BuildJsonDeserializationTestCase("03-[IP1 -> Ø-IP1 / Ø-IP1]", testNumber++,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1);
            yield return BuildJsonDeserializationTestCase("04-[IP2 -> Ø-IP2 / Ø-IP2]", testNumber++,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2);
            // Ø
            yield return BuildJsonDeserializationTestCase("05-[Ø -> Ø-Ø / Ø-Ø]", testNumber++,
                ProtectedNumberJsonValueNameInstance.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty);
            // null
            yield return BuildJsonDeserializationTestCase("06-[null -> null / null]", testNumber,
                ProtectedNumberJsonValueNameInstance.Null,
                ProtectedNumberTestNamedInstance.NotInitialized.Null,
                ProtectedNumberTestNamedInstance.NotInitialized.Null);
        }

        public static IEnumerable<TestCaseData> EnumerateJsonSerializationTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP
            yield return BuildJsonSerializationTestCase("01-[1-1P -> 1P / 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberJsonValueNameInstance.OneProtectedValue,
                ProtectedNumberJsonValueNameInstance.OneProtectedValue);
            yield return BuildJsonSerializationTestCase("02-[2-2P -> 2P / 2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                ProtectedNumberJsonValueNameInstance.TwoProtectedValue,
                ProtectedNumberJsonValueNameInstance.TwoProtectedValue);
            // V-IP
            yield return BuildJsonSerializationTestCase("03-[1-IP1 -> IP1 / IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1);
            yield return BuildJsonSerializationTestCase("04-[1-IP2 -> IP2 / IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2);
            yield return BuildJsonSerializationTestCase("05-[2-IP1 -> IP1 / IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1);
            yield return BuildJsonSerializationTestCase("06-[2-IP2 -> IP2 / IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2);
            // V-Ø
            yield return BuildJsonSerializationTestCase("07-[1-Ø -> null / 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberJsonValueNameInstance.Null,
                ProtectedNumberJsonValueNameInstance.OneProtectedValue);
            yield return BuildJsonSerializationTestCase("08-[2-Ø -> null / 2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberJsonValueNameInstance.Null,
                ProtectedNumberJsonValueNameInstance.TwoProtectedValue);
            // Ø-VP
            yield return BuildJsonSerializationTestCase("09-[Ø-1P -> 1P / 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberJsonValueNameInstance.OneProtectedValue,
                ProtectedNumberJsonValueNameInstance.OneProtectedValue);
            yield return BuildJsonSerializationTestCase("10-[Ø-2P -> 2P / 2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                ProtectedNumberJsonValueNameInstance.TwoProtectedValue,
                ProtectedNumberJsonValueNameInstance.TwoProtectedValue);
            // Ø-IP
            yield return BuildJsonSerializationTestCase("11-[Ø-IP1 -> IP1 / IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue1);
            yield return BuildJsonSerializationTestCase("12-[Ø-IP2 -> IP2 / IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2,
                ProtectedNumberJsonValueNameInstance.InvalidProtectedValue2);
            // Ø-Ø
            yield return BuildJsonSerializationTestCase("13-[Ø-Ø -> null / null]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberJsonValueNameInstance.Null,
                ProtectedNumberJsonValueNameInstance.Null);
            yield return BuildJsonSerializationTestCase("14-[Empty -> null / null]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberJsonValueNameInstance.Null,
                ProtectedNumberJsonValueNameInstance.Null);
            // INV
            yield return BuildJsonSerializationTestCase("15-[INV -> throw / throw]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberJsonValueNameInstance.Throw,
                ProtectedNumberJsonValueNameInstance.Throw,
                new NotSupportedException());
        }

        private static TestCaseData BuildJsonDeserializationTestCase(string manualTestName, int testNumber,
            ProtectedNumberJsonValueNameInstance jsonValueNameInstance,
            ProtectedNumberTestNamedInstance expectedNamedInstanceNoProtectionService,
            ProtectedNumberTestNamedInstance expectedNamedInstanceProtectionService,
            Exception? exceptionExpected = null)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = null,
                StringValue = jsonValueNameInstance.Create(),
                OtherInstance = expectedNamedInstanceNoProtectionService.Create(),
                AnotherInstance = expectedNamedInstanceProtectionService.Create(),
                ExceptionExpected = exceptionExpected
            };
            string automaticTestName =
                $"{testNumber:00}-[{jsonValueNameInstance.Name} -> {expectedNamedInstanceNoProtectionService.Name} / {expectedNamedInstanceProtectionService.Name}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }

        private static TestCaseData BuildJsonSerializationTestCase(string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance namedInstance,
            ProtectedNumberJsonValueNameInstance expectedJsonValueNameInstanceNoProtectionService,
            ProtectedNumberJsonValueNameInstance expectedJsonValueNameInstanceProtectionService,
            Exception? exceptionExpected = null)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = namedInstance.Create(),
                StringValue = expectedJsonValueNameInstanceNoProtectionService.Create(),
                OtherStringValue = expectedJsonValueNameInstanceProtectionService.Create(),
                ExceptionExpected = exceptionExpected
            };
            string automaticTestName =
                $"{testNumber:00}-[{namedInstance.Name} -> {expectedJsonValueNameInstanceNoProtectionService.Name} / {expectedJsonValueNameInstanceProtectionService.Name}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }
    }
}
