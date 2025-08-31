// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

using System.Text.Json;

[TestFixture]
public partial class ProtectedNumberTests
{
    [Test]
    public void NewInstance_Using_Default_Constructor_Should_Not_Be_Initialized_And_Throw_On_Members()
    {
        // Inputs
        // Arrange
        // Act
        ProtectedNumber invalidInstance = new();
        // Assert
        invalidInstance.IsInitialized().ShouldBeFalse();
        Should.Throw<NotSupportedException>(() =>
        {
            bool _ = invalidInstance.HasValue;
        });
        Should.Throw<NotSupportedException>(() =>
        {
            long _ = invalidInstance.Value;
        });
        Should.Throw<NotSupportedException>(() =>
        {
            bool _ = invalidInstance.HasProtectedValue;
        });
        Should.Throw<NotSupportedException>(() =>
        {
            string _ = invalidInstance.ProtectedValue;
        });
    }

    public class ProtectedNumberTestCase
    {
        public ProtectedNumber? AnotherInstance { get; init; }

        public int CompareToExpected { get; init; }

        public bool EqualsExpected { get; init; }

        public Exception? ExceptionExpected { get; set; }

        public ProtectedNumber? Instance { get; init; }

        public bool IsNullOrEmptyExpected { get; init; }

        public JsonSerializerOptions? JsonSerializerOptions { get; init; }

        public long? LongValue { get; set; }

        public ProtectedNumber? OtherInstance { get; init; }

        public string? OtherStringValue { get; set; }

        public string? StringValue { get; set; }
    }

    public static partial class ProtectedNumberTestCases
    {
    }
}
