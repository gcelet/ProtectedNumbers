// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

public partial class ProtectedNumberTests
{
    [Test]
    public void Empty_Should_Be_Initialized_Without_Value_And_ProtectedValue()
    {
        // Inputs
        // Arrange
        // Act
        ProtectedNumber empty = ProtectedNumber.Empty;
        // Assert
        empty.IsInitialized().ShouldBeTrue();
        empty.HasValue.ShouldBeFalse();
        empty.HasProtectedValue.ShouldBeFalse();
        Should.Throw<InvalidOperationException>(() =>
        {
            long _ = empty.Value;
        });
        Should.Throw<InvalidOperationException>(() =>
        {
            string _ = empty.ProtectedValue;
        });
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateIsNullOrEmptyTestCases))]
    public void IsNullOrEmpty_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber? instance = testCase.Instance;
        // Act
        bool isNullOrEmptyActual = ProtectedNumber.IsNullOrEmpty(instance);
        // Assert
        isNullOrEmptyActual.ShouldBe(testCase.IsNullOrEmptyExpected);
    }

    public static partial class ProtectedNumberTestCases
    {
        public static IEnumerable<TestCaseData> EnumerateIsNullOrEmptyTestCases()
        {
            yield return BuildIsNullOrEmptyTestCase("[1-IP1 => Non empty]",
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                isNullOrEmptyExpected: false
            );
            yield return BuildIsNullOrEmptyTestCase("[1-1P => Non empty]",
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                isNullOrEmptyExpected: false
            );
            yield return BuildIsNullOrEmptyTestCase("[1-Ø => Non empty]",
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                isNullOrEmptyExpected: false
            );
            yield return BuildIsNullOrEmptyTestCase("[Empty => Empty]",
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                isNullOrEmptyExpected: true
            );
            yield return BuildIsNullOrEmptyTestCase("[Ø-1P => Non empty]",
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                isNullOrEmptyExpected: false
            );
            yield return BuildIsNullOrEmptyTestCase("[Ø-IP1 => Non empty]",
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                isNullOrEmptyExpected: false
            );
            yield return BuildIsNullOrEmptyTestCase("[Ø-Ø => Empty]",
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                isNullOrEmptyExpected: true
            );
        }

        private static TestCaseData BuildIsNullOrEmptyTestCase(string manualTestName,
            ProtectedNumberTestNamedInstance namedInstance,
            bool isNullOrEmptyExpected = false)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = namedInstance.Create(),
                IsNullOrEmptyExpected = isNullOrEmptyExpected,
            };
            string automaticTestName = $"[{namedInstance.Name} => {(isNullOrEmptyExpected ? "Empty" : "Non empty")}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }
    }
}
