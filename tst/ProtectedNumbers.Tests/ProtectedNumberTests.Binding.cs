// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

public partial class ProtectedNumberTests
{
    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateModelBindingTestCases))]
    public void ModelBinding_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        // Act
        // Assert
    }

    public static partial class ProtectedNumberTestCases
    {
        public static IEnumerable<TestCaseData> EnumerateModelBindingTestCases()
        {
            yield break;
        }

        private static TestCaseData BuildModelBindingTestCase(string manualTestName,
            ProtectedNumberTestNamedInstance namedInstance)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = namedInstance.Create(),
            };
            string automaticTestName = $"[{namedInstance.Name} =>]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }
    }
}
