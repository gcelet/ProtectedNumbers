// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

public partial class ProtectedNumberTests
{
    [Test]
    public void Sort_Should_Be_Work_Correctly()
    {
        // Inputs
        // Arrange
        // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
        List<ProtectedNumber> list =
        [
            ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.NotInitialized.Invalid.CreateAsNonNullable(),
        ];
        // Act
        list.Sort();
        // Assert
        list.ShouldBe([
            ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty.CreateAsNonNullable(),
            ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic.CreateAsNonNullable(),

            ProtectedNumberTestNamedInstance.NotInitialized.Invalid.CreateAsNonNullable(),
        ]);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsTestCases))]
    public void CompareTo_Should_Be_Consistent_With_Equals(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber left = testCase.Instance ?? ProtectedNumber.Empty;
        ProtectedNumber right = testCase.OtherInstance ?? ProtectedNumber.Empty;
        // Act
        int compareToActual = left.CompareTo(right);
        // Assert
        if (testCase.EqualsExpected)
        {
            compareToActual.ShouldBe(0);
        }
        else
        {
            compareToActual.ShouldNotBe(0);
        }
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateCompareToTestCases))]
    public void CompareTo_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber left = testCase.Instance ?? ProtectedNumber.Empty;
        ProtectedNumber right = testCase.OtherInstance ?? ProtectedNumber.Empty;
        // Act
        int compareToActual = left.CompareTo(right);
        // Assert
        compareToActual.ShouldBe(testCase.CompareToExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateCompareToTestCases))]
    public void OperatorLower_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber left = testCase.Instance ?? ProtectedNumber.Empty;
        ProtectedNumber right = testCase.OtherInstance ?? ProtectedNumber.Empty;
        // Act
        bool actual = left < right;
        // Assert
        actual.ShouldBe(testCase.CompareToExpected < 0);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateCompareToTestCases))]
    public void OperatorGreater_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber left = testCase.Instance ?? ProtectedNumber.Empty;
        ProtectedNumber right = testCase.OtherInstance ?? ProtectedNumber.Empty;
        // Act
        bool actual = left > right;
        // Assert
        actual.ShouldBe(testCase.CompareToExpected > 0);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateCompareToTestCases))]
    public void OperatorLowerOrEqual_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber left = testCase.Instance ?? ProtectedNumber.Empty;
        ProtectedNumber right = testCase.OtherInstance ?? ProtectedNumber.Empty;
        // Act
        bool actual = left <= right;
        // Assert
        actual.ShouldBe(testCase.CompareToExpected <= 0);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateCompareToTestCases))]
    public void OperatorGreaterOrEqual_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber left = testCase.Instance ?? ProtectedNumber.Empty;
        ProtectedNumber right = testCase.OtherInstance ?? ProtectedNumber.Empty;
        // Act
        bool actual = left >= right;
        // Assert
        actual.ShouldBe(testCase.CompareToExpected >= 0);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateCompareToWithLongTestCases))]
    public void CompareTo_With_Long_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        long longValue = testCase.LongValue.GetValueOrDefault();
        // Act
        int compareToActual = instance.CompareTo(longValue);
        // Assert
        compareToActual.ShouldBe(testCase.CompareToExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateCompareToWithStringTestCases))]
    public void CompareTo_With_String_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        string? stringValue = testCase.StringValue;
        // Act
        int compareToActual = instance.CompareTo(stringValue);
        // Assert
        compareToActual.ShouldBe(testCase.CompareToExpected);
    }

    public static partial class ProtectedNumberTestCases
    {
        public static IEnumerable<TestCaseData> EnumerateCompareToTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP
            yield return BuildCompareToTestCase("01-[1-1P == 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("02-[1-1P < 2-2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("03-[2-2P > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-IP
            yield return BuildCompareToTestCase("04-[1-IP1 == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("05-[1-IP1 < 2-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("06-[2-IP1 > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("07-[1-IP1 == 1-IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("08-[1-IP2 == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("09-[1-IP1 < 2-IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("10-[2-IP2 > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            // V-Ø
            yield return BuildCompareToTestCase("11-[1-Ø == 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("12-[1-Ø < 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("13-[2-Ø > 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 1);
            // Ø-VP
            yield return BuildCompareToTestCase("14-[Ø-1P == Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("15-[Ø-1P < Ø-2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("16-[Ø-2P > Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // Ø-IP
            yield return BuildCompareToTestCase("17-[Ø-IP1 == Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("18-[Ø-IP1 < Ø-IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("19-[Ø-IP2 > Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);
            // Ø-Ø
            yield return BuildCompareToTestCase("20-[Ø-Ø == Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("21-[Empty == Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("22-[Ø-Ø == Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("23-[Empty == Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 0);
            // INV
            yield return BuildCompareToTestCase("24-[INV == INV]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: 0);
            // V-VP x V-IP
            yield return BuildCompareToTestCase("25-[1-1P == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("26-[1-IP1 == 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("27-[1-1P < 2-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("28-[2-IP1 > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-VP x V-Ø
            yield return BuildCompareToTestCase("29-[1-1P == 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("30-[1-Ø == 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("31-[1-1P < 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("32-[2-Ø > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-VP x Ø-VP
            yield return BuildCompareToTestCase("33-[1-1P < Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("34-[Ø-1P > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-VP x Ø-IP
            yield return BuildCompareToTestCase("35-[1-1P < Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("36-[Ø-IP1 > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-VP x Ø-Ø
            yield return BuildCompareToTestCase("37-[1-1P < Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("38-[Ø-Ø > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("39-[1-1P < Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("40-[Empty > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-VP x INV
            yield return BuildCompareToTestCase("41-[1-1P < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("42-[INV > 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);

            // V-IP x V-Ø
            yield return BuildCompareToTestCase("43-[1-IP1 == 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("44-[1-Ø == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 0);
            yield return BuildCompareToTestCase("45-[1-IP1 < 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("46-[2-Ø > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            // V-IP x Ø-VP
            yield return BuildCompareToTestCase("47-[1-IP1 < Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("48-[Ø-1P > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("49-[1-IP1 < Ø-2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("50-[Ø-2P > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            // V-IP x Ø-IP
            yield return BuildCompareToTestCase("51-[1-IP1 < Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("52-[Ø-IP1 > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            // V-IP x Ø-Ø
            yield return BuildCompareToTestCase("53-[1-IP1 < Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("54-[Ø-Ø > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("55-[1-IP1 < Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("56-[Empty > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);
            // V-IP x INV
            yield return BuildCompareToTestCase("57-[1-IP1 < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("58-[INV > 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 1);

            // V-Ø x Ø-VP
            yield return BuildCompareToTestCase("59-[1-Ø < Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("60-[Ø-1P > 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("61-[2-Ø < Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("62-[Ø-1P > 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                compareToExpected: 1);
            // V-Ø x Ø-IP
            yield return BuildCompareToTestCase("63-[1-Ø < Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("64-[Ø-IP1 > 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 1);
            // V-Ø x Ø-Ø
            yield return BuildCompareToTestCase("65-[1-Ø < Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("66-[Ø-Ø > 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("67-[1-Ø < Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("68-[Empty > 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 1);
            // V-Ø x INV
            yield return BuildCompareToTestCase("69-[1-Ø < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("70-[INV > 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                compareToExpected: 1);

            // Ø-VP x Ø-IP
            yield return BuildCompareToTestCase("71-[Ø-1P < Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("72-[Ø-IP1 > Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // Ø-VP x Ø-Ø
            yield return BuildCompareToTestCase("73-[Ø-1P < Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("74-[Ø-Ø > Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("75-[Ø-1P < Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("76-[Empty > Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // Ø-VP x INV
            yield return BuildCompareToTestCase("77-[Ø-1P < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("78-[INV > Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);

            // Ø-IP x Ø-Ø
            yield return BuildCompareToTestCase("79-[Ø-IP1 < Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("80-[Ø-Ø > Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("81-[Ø-IP1 < Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("82-[Empty > Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);
            // Ø-IP x INV
            yield return BuildCompareToTestCase("83-[Ø-IP1 < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("84-[INV > Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);

            // Ø-Ø x INV
            yield return BuildCompareToTestCase("85-[Ø-Ø < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("86-[INV > Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 1);
            yield return BuildCompareToTestCase("87-[Empty < INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                compareToExpected: -1);
            yield return BuildCompareToTestCase("88-[INV > Empty]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                compareToExpected: 1);
        }

        public static IEnumerable<TestCaseData> EnumerateCompareToWithLongTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP x V
            yield return BuildCompareToWithLongTestCase("01-[1-1P == 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                1,
                compareToExpected: 0);
            yield return BuildCompareToWithLongTestCase("02-[1-1P < 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                2,
                compareToExpected: -1);

            // V-IP x V
            yield return BuildCompareToWithLongTestCase("03-[1-IP1 == 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                1,
                compareToExpected: 0);
            yield return BuildCompareToWithLongTestCase("04-[1-IP1 < 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                2,
                compareToExpected: -1);

            // V-Ø x V
            yield return BuildCompareToWithLongTestCase("05-[1-Ø == 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                1,
                compareToExpected: 0);
            yield return BuildCompareToWithLongTestCase("06-[1-Ø < 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                2,
                compareToExpected: -1);

            // Ø-VP x V
            yield return BuildCompareToWithLongTestCase("07-[Ø-1P > 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                1,
                compareToExpected: 1);
            yield return BuildCompareToWithLongTestCase("08-[Ø-1P > 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                2,
                compareToExpected: 1);

            // Ø-IP x V
            yield return BuildCompareToWithLongTestCase("09-[Ø-IP1 > 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                1,
                compareToExpected: 1);
            yield return BuildCompareToWithLongTestCase("10-[Ø-IP1 > 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                2,
                compareToExpected: 1);

            // Ø-Ø x V
            yield return BuildCompareToWithLongTestCase("11-[Ø-Ø > 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                1,
                compareToExpected: 1);
            yield return BuildCompareToWithLongTestCase("12-[Ø-Ø > 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                2,
                compareToExpected: 1);
            yield return BuildCompareToWithLongTestCase("13-[Empty > 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                1,
                compareToExpected: 1);
            yield return BuildCompareToWithLongTestCase("14-[Empty > 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                2,
                compareToExpected: 1);

            // INV x V
            yield return BuildCompareToWithLongTestCase("15-[INV > 1]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                1,
                compareToExpected: 1);
        }

        public static IEnumerable<TestCaseData> EnumerateCompareToWithStringTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP x VP
            yield return BuildCompareToWithStringTestCase("01-[1-1P == 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 0);
            yield return BuildCompareToWithStringTestCase("02-[1-1P < 2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                compareToExpected: -1);
            // V-VP x IP
            yield return BuildCompareToWithStringTestCase("03-[1-1P < IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: -1);
            yield return BuildCompareToWithStringTestCase("04-[1-1P < IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                compareToExpected: -1);
            // V-VP x Empty
            yield return BuildCompareToWithStringTestCase("05-[1-1P > Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 1);
            // V-VP x null
            yield return BuildCompareToWithStringTestCase("06-[1-1P > Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 1);
            // V-IP x VP
            yield return BuildCompareToWithStringTestCase("07-[1-IP1 > 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-IP x IP
            yield return BuildCompareToWithStringTestCase("08-[1-IP1 == IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                compareToExpected: 0);
            // V-IP x Empty
            yield return BuildCompareToWithStringTestCase("09-[1-IP1 > Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 1);
            // V-IP x null
            yield return BuildCompareToWithStringTestCase("10-[1-IP1 > Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 1);
            // V-Ø x VP
            yield return BuildCompareToWithStringTestCase("11-[1-Ø > 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // V-Ø x IP
            yield return BuildCompareToWithStringTestCase("12-[1-Ø > IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);
            // V-Ø x Empty
            yield return BuildCompareToWithStringTestCase("13-[1-Ø == Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 0);
            // V-Ø x null
            yield return BuildCompareToWithStringTestCase("14-[1-Ø == Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 0);
            // Ø-VP x VP
            yield return BuildCompareToWithStringTestCase("15-[Ø-1P == 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 0);
            // Ø-VP x IP
            yield return BuildCompareToWithStringTestCase("16-[Ø-1P < IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: -1);
            // Ø-VP x Empty
            yield return BuildCompareToWithStringTestCase("17-[Ø-1P > Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 1);
            // Ø-VP x null
            yield return BuildCompareToWithStringTestCase("18-[Ø-1P > Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 1);
            // Ø-IP x VP
            yield return BuildCompareToWithStringTestCase("19-[Ø-IP1 > 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // Ø-IP x IP
            yield return BuildCompareToWithStringTestCase("20-[Ø-IP1 == IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 0);
            // Ø-IP x Empty
            yield return BuildCompareToWithStringTestCase("21-[Ø-IP1 > Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 1);
            // Ø-IP x null
            yield return BuildCompareToWithStringTestCase("22-[Ø-IP1 > Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 1);
            // Ø-Ø x VP
            yield return BuildCompareToWithStringTestCase("23-[Ø-Ø > 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // Ø-Ø x IP
            yield return BuildCompareToWithStringTestCase("24-[Ø-Ø > IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);
            // Ø-Ø x Empty
            yield return BuildCompareToWithStringTestCase("25-[Ø-Ø == Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 0);
            // Ø-Ø x null
            yield return BuildCompareToWithStringTestCase("26-[Ø-Ø == Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 0);
            // INV x VP
            yield return BuildCompareToWithStringTestCase("27-[INV > 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                compareToExpected: 1);
            // INV x IP
            yield return BuildCompareToWithStringTestCase("28-[INV > IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                compareToExpected: 1);
            // INV x Empty
            yield return BuildCompareToWithStringTestCase("29-[INV > Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                compareToExpected: 1);
            // INV x null
            yield return BuildCompareToWithStringTestCase("30-[INV > Ø]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                compareToExpected: 1);
        }

        private static TestCaseData BuildCompareToTestCase(string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance leftNamedInstance,
            ProtectedNumberTestNamedInstance rightNamedInstance,
            int compareToExpected = 0)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = leftNamedInstance.Create(),
                OtherInstance = rightNamedInstance.Create(),
                CompareToExpected = compareToExpected
            };
            string compareOperator = compareToExpected switch
            {
                0 => "==",
                < 0 => "<",
                > 0 => ">",
            };
            string automaticTestName =
                $"{testNumber:00}-[{leftNamedInstance.Name} {compareOperator} {rightNamedInstance.Name}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }

        private static TestCaseData BuildCompareToWithLongTestCase(string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance namedInstance,
            long longValue,
            int compareToExpected = 0)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = namedInstance.Create(),
                LongValue = longValue,
                CompareToExpected = compareToExpected
            };
            string compareOperator = compareToExpected switch
            {
                0 => "==",
                < 0 => "<",
                > 0 => ">",
            };
            string automaticTestName =
                $"{testNumber:00}-[{namedInstance.Name} {compareOperator} {longValue}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }

        private static TestCaseData BuildCompareToWithStringTestCase(string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance leftNamedInstance,
            ProtectedNumberTestNamedInstance rightNamedInstance,
            int compareToExpected = 0)
        {
            ProtectedNumber right = rightNamedInstance.CreateAsNonNullable();
            string? stringValue = right.HasProtectedValue ? right.ProtectedValue : null;
            ProtectedNumberTestCase testCase = new()
            {
                Instance = leftNamedInstance.Create(),
                StringValue = stringValue,
                CompareToExpected = compareToExpected
            };
            string compareOperator = compareToExpected switch
            {
                0 => "==",
                < 0 => "<",
                > 0 => ">",
            };
            string automaticTestName =
                $"{testNumber:00}-[{leftNamedInstance.Name} {compareOperator} {rightNamedInstance.NameProtectedValuePart}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }
    }
}
