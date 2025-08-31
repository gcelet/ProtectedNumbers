// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

public partial class ProtectedNumberTests
{
    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsTestCases))]
    public void Equals_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber? left = testCase.Instance;
        ProtectedNumber? right = testCase.OtherInstance;
        // Act
        bool equalsActual = left.Equals(right);
        // Assert
        equalsActual.ShouldBe(testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsToLongTestCases))]
    public void Equals_With_Long_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        long longValue = testCase.LongValue.GetValueOrDefault();
        // Act
        bool equalsActual = instance.Equals(longValue);
        // Assert
        equalsActual.ShouldBe(testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateEqualsToStringTestCases))]
    public void Equals_With_String_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        string? stringValue = testCase.StringValue;
        // Act
        bool equalsActual = instance.Equals(stringValue);
        // Assert
        equalsActual.ShouldBe(testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsTestCases))]
    public void GetHashCode_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber? left = testCase.Instance;
        ProtectedNumber? right = testCase.OtherInstance;
        // Act
        int leftHashCode = left.GetHashCode();
        int rightHashCode = right.GetHashCode();
        bool hashCodesEquals = leftHashCode == rightHashCode;
        // Assert
        hashCodesEquals.ShouldBe(testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsTestCases))]
    public void OperatorDifferent_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber? left = testCase.Instance;
        ProtectedNumber? right = testCase.OtherInstance;
        // Act
        bool differentActual = left != right;
        // Assert
        differentActual.ShouldBe(!testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsToLongTestCases))]
    public void OperatorDifferent_With_Long_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        long longValue = testCase.LongValue.GetValueOrDefault();
        // Act
        bool differentActualLeft = instance != longValue;
        bool differentActualRight = longValue != instance;
        // Assert
        differentActualLeft.ShouldBe(!testCase.EqualsExpected);
        differentActualRight.ShouldBe(!testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateEqualsToStringTestCases))]
    public void OperatorDifferent_With_String_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        string? stringValue = testCase.StringValue;
        // Act
        bool differentActualLeft = instance != stringValue;
        bool differentActualRight = stringValue != instance;
        // Assert
        differentActualLeft.ShouldBe(!testCase.EqualsExpected);
        differentActualRight.ShouldBe(!testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsTestCases))]
    public void OperatorEquals_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber? left = testCase.Instance;
        ProtectedNumber? right = testCase.OtherInstance;
        // Act
        bool equalsActual = left == right;
        // Assert
        equalsActual.ShouldBe(testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases), nameof(ProtectedNumberTestCases.EnumerateEqualsToLongTestCases))]
    public void OperatorEquals_With_Long_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        long longValue = testCase.LongValue.GetValueOrDefault();
        // Act
        bool equalsActualLeft = instance == longValue;
        bool equalsActualRight = longValue == instance;
        // Assert
        equalsActualLeft.ShouldBe(testCase.EqualsExpected);
        equalsActualRight.ShouldBe(testCase.EqualsExpected);
    }

    [TestCaseSource(typeof(ProtectedNumberTestCases),
        nameof(ProtectedNumberTestCases.EnumerateEqualsToStringTestCases))]
    public void OperatorEquals_With_String_Should_Returns_Correctly(ProtectedNumberTestCase testCase)
    {
        // Inputs
        // Arrange
        ProtectedNumber instance = testCase.Instance ?? ProtectedNumber.Empty;
        string? stringValue = testCase.StringValue;
        // Act
        bool equalsActualLeft = instance == stringValue;
        bool equalsActualRight = stringValue == instance;
        // Assert
        equalsActualLeft.ShouldBe(testCase.EqualsExpected);
        equalsActualRight.ShouldBe(testCase.EqualsExpected);
    }

    public static partial class ProtectedNumberTestCases
    {
        public static IEnumerable<TestCaseData> EnumerateEqualsTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP
            yield return BuildEqualsTestCase("01-[1-1P == 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("02-[1-1P != 2-2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                equalsExpected: false);
            yield return BuildEqualsTestCase("03-[2-2P != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-IP
            yield return BuildEqualsTestCase("04-[1-IP1 == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: true);
            yield return BuildEqualsTestCase("05-[1-IP1 != 2-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("06-[2-IP1 != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("07-[1-IP1 == 1-IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2,
                equalsExpected: true);
            yield return BuildEqualsTestCase("08-[1-IP2 == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: true);
            yield return BuildEqualsTestCase("09-[1-IP1 != 2-IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2,
                equalsExpected: false);
            yield return BuildEqualsTestCase("10-[2-IP2 != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            // V-Ø
            yield return BuildEqualsTestCase("11-[1-Ø == 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("12-[1-Ø != 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                equalsExpected: false);
            yield return BuildEqualsTestCase("13-[2-Ø != 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: false);
            // Ø-VP
            yield return BuildEqualsTestCase("14-[Ø-1P == Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("15-[Ø-1P != Ø-2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                equalsExpected: false);
            yield return BuildEqualsTestCase("16-[Ø-2P != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // Ø-IP
            yield return BuildEqualsTestCase("17-[Ø-IP1 == Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: true);
            yield return BuildEqualsTestCase("18-[Ø-IP1 != Ø-IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                equalsExpected: false);
            yield return BuildEqualsTestCase("19-[Ø-IP2 != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            // Ø-Ø
            yield return BuildEqualsTestCase("20-[Ø-Ø == Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: true);
            yield return BuildEqualsTestCase("21-[Empty == Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: true);
            yield return BuildEqualsTestCase("22-[Ø-Ø == Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: true);
            yield return BuildEqualsTestCase("23-[Empty == Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: true);
            // INV
            yield return BuildEqualsTestCase("24-[INV == INV]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: true);
            // V-VP x V-IP
            yield return BuildEqualsTestCase("25-[1-1P == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: true);
            yield return BuildEqualsTestCase("26-[1-IP1 == 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("27-[1-1P != 2-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("28-[2-IP1 != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.TwoInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-VP x V-Ø
            yield return BuildEqualsTestCase("29-[1-1P == 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("30-[1-Ø == 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("31-[1-1P != 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                equalsExpected: false);
            yield return BuildEqualsTestCase("32-[2-Ø != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-VP x Ø-VP
            yield return BuildEqualsTestCase("33-[1-1P != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("34-[Ø-1P != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-VP x Ø-IP
            yield return BuildEqualsTestCase("35-[1-1P != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("36-[Ø-IP1 != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-VP x Ø-Ø
            yield return BuildEqualsTestCase("37-[1-1P != Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            yield return BuildEqualsTestCase("38-[Ø-Ø != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("39-[1-1P != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: false);
            yield return BuildEqualsTestCase("40-[Empty != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-VP x INV
            yield return BuildEqualsTestCase("41-[1-1P != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("42-[INV != 1-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);

            // V-IP x V-Ø
            yield return BuildEqualsTestCase("43-[1-IP1 == 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsTestCase("44-[1-Ø == 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: true);
            yield return BuildEqualsTestCase("45-[1-IP1 != 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                equalsExpected: false);
            yield return BuildEqualsTestCase("46-[2-Ø != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            // V-IP x Ø-VP
            yield return BuildEqualsTestCase("47-[1-IP1 != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("48-[Ø-1P != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("49-[1-IP1 != Ø-2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                equalsExpected: false);
            yield return BuildEqualsTestCase("50-[Ø-2P != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            // V-IP x Ø-IP
            yield return BuildEqualsTestCase("51-[1-IP1 != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("52-[Ø-IP1 != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            // V-IP x Ø-Ø
            yield return BuildEqualsTestCase("53-[1-IP1 != Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            yield return BuildEqualsTestCase("54-[Ø-Ø != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("55-[1-IP1 != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: false);
            yield return BuildEqualsTestCase("56-[Empty != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);
            // V-IP x INV
            yield return BuildEqualsTestCase("57-[1-IP1 != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("58-[INV != 1-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: false);

            // V-Ø x Ø-VP
            yield return BuildEqualsTestCase("59-[1-Ø != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("60-[Ø-1P != 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("61-[2-Ø != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("62-[Ø-1P != 2-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.Two,
                equalsExpected: false);
            // V-Ø x Ø-IP
            yield return BuildEqualsTestCase("63-[1-Ø != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("64-[Ø-IP1 != 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: false);
            // V-Ø x Ø-Ø
            yield return BuildEqualsTestCase("65-[1-Ø != Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            yield return BuildEqualsTestCase("66-[Ø-Ø != 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("67-[1-Ø != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: false);
            yield return BuildEqualsTestCase("68-[Empty != 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: false);
            // V-Ø x INV
            yield return BuildEqualsTestCase("69-[1-Ø != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("70-[INV != 1-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                equalsExpected: false);

            // Ø-VP x Ø-IP
            yield return BuildEqualsTestCase("71-[Ø-1P != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("72-[Ø-IP1 != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // Ø-VP x Ø-Ø
            yield return BuildEqualsTestCase("73-[Ø-1P != Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            yield return BuildEqualsTestCase("74-[Ø-Ø != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            yield return BuildEqualsTestCase("75-[Ø-1P != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: false);
            yield return BuildEqualsTestCase("76-[Empty != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // Ø-VP x INV
            yield return BuildEqualsTestCase("77-[Ø-1P != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("78-[INV != Ø-1P]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);

            // Ø-IP x Ø-Ø
            yield return BuildEqualsTestCase("79-[Ø-IP1 != Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            yield return BuildEqualsTestCase("80-[Ø-Ø != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsTestCase("81-[Ø-IP1 != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: false);
            yield return BuildEqualsTestCase("82-[Empty != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            // Ø-IP x INV
            yield return BuildEqualsTestCase("83-[Ø-IP1 != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("84-[INV != Ø-IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);

            // Ø-Ø x INV
            yield return BuildEqualsTestCase("85-[Ø-Ø != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("86-[INV != Ø-Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            yield return BuildEqualsTestCase("87-[Empty != INV]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                equalsExpected: false);
            yield return BuildEqualsTestCase("88-[INV != Empty]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                equalsExpected: false);
        }

        public static IEnumerable<TestCaseData> EnumerateEqualsToLongTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP x V
            yield return BuildEqualsWithLongTestCase("01-[1-1P == 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                1,
                equalsExpected: true);
            yield return BuildEqualsWithLongTestCase("02-[1-1P != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                2,
                equalsExpected: false);

            // V-IP x V
            yield return BuildEqualsWithLongTestCase("03-[1-IP1 == 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                1,
                equalsExpected: true);
            yield return BuildEqualsWithLongTestCase("04-[1-IP1 != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                2,
                equalsExpected: false);

            // V-Ø x V
            yield return BuildEqualsWithLongTestCase("05-[1-Ø == 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                1,
                equalsExpected: true);
            yield return BuildEqualsWithLongTestCase("06-[1-Ø != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                2,
                equalsExpected: false);

            // Ø-VP x V
            yield return BuildEqualsWithLongTestCase("07-[Ø-1P != 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                1,
                equalsExpected: false);
            yield return BuildEqualsWithLongTestCase("08-[Ø-1P != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                2,
                equalsExpected: false);

            // Ø-IP x V
            yield return BuildEqualsWithLongTestCase("09-[Ø-IP1 != 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                1,
                equalsExpected: false);
            yield return BuildEqualsWithLongTestCase("10-[Ø-IP1 != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                2,
                equalsExpected: false);

            // Ø-Ø x V
            yield return BuildEqualsWithLongTestCase("11-[Ø-Ø != 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                1,
                equalsExpected: false);
            yield return BuildEqualsWithLongTestCase("12-[Ø-Ø != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                2,
                equalsExpected: false);
            yield return BuildEqualsWithLongTestCase("13-[Empty != 1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                1,
                equalsExpected: false);
            yield return BuildEqualsWithLongTestCase("14-[Empty != 2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.EmptyStatic,
                2,
                equalsExpected: false);

            // INV x V
            yield return BuildEqualsWithLongTestCase("15-[INV != 1]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                1,
                equalsExpected: false);
        }

        public static IEnumerable<TestCaseData> EnumerateEqualsToStringTestCases()
        {
            // All categories: [V-VP / V-IP / V-Ø / Ø-VP / Ø-IP / Ø-Ø / INV]
            int testNumber = 1;
            // V-VP x VP
            yield return BuildEqualsWithStringTestCase("01-[1-1P == 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: true);
            yield return BuildEqualsWithStringTestCase("02-[1-1P != 2P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.Two,
                equalsExpected: false);
            // V-VP x IP
            yield return BuildEqualsWithStringTestCase("03-[1-1P != IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            yield return BuildEqualsWithStringTestCase("04-[1-1P != IP2]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue2,
                equalsExpected: false);
            // V-VP x Empty
            yield return BuildEqualsWithStringTestCase("05-[1-1P != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // V-VP x null
            yield return BuildEqualsWithStringTestCase("06-[1-1P != Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            // V-IP x VP
            yield return BuildEqualsWithStringTestCase("07-[1-IP1 != 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-IP x IP
            yield return BuildEqualsWithStringTestCase("08-[1-IP1 == IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                equalsExpected: true);
            // V-IP x Empty
            yield return BuildEqualsWithStringTestCase("09-[1-IP1 != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // V-IP x null
            yield return BuildEqualsWithStringTestCase("10-[1-IP1 != Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndInvalidProtectedValue.OneInvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            // V-Ø x VP
            yield return BuildEqualsWithStringTestCase("11-[1-Ø != 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // V-Ø x IP
            yield return BuildEqualsWithStringTestCase("12-[1-Ø != IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            // V-Ø x Empty
            yield return BuildEqualsWithStringTestCase("13-[1-Ø != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // V-Ø x null
            yield return BuildEqualsWithStringTestCase("14-[1-Ø != Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithValueAndWithoutProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            // Ø-VP x VP
            yield return BuildEqualsWithStringTestCase("15-[Ø-1P == 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: true);
            // Ø-VP x IP
            yield return BuildEqualsWithStringTestCase("16-[Ø-1P != IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            // Ø-VP x Empty
            yield return BuildEqualsWithStringTestCase("17-[Ø-1P != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // Ø-VP x null
            yield return BuildEqualsWithStringTestCase("18-[Ø-1P != Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            // Ø-IP x VP
            yield return BuildEqualsWithStringTestCase("19-[Ø-IP1 != 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // Ø-IP x IP
            yield return BuildEqualsWithStringTestCase("20-[Ø-IP1 == IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: true);
            // Ø-IP x Empty
            yield return BuildEqualsWithStringTestCase("21-[Ø-IP1 != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // Ø-IP x null
            yield return BuildEqualsWithStringTestCase("22-[Ø-IP1 != Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            // Ø-Ø x VP
            yield return BuildEqualsWithStringTestCase("23-[Ø-Ø != 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // Ø-Ø x IP
            yield return BuildEqualsWithStringTestCase("24-[Ø-Ø != IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            // Ø-Ø x Empty
            yield return BuildEqualsWithStringTestCase("25-[Ø-Ø != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // Ø-Ø x null
            yield return BuildEqualsWithStringTestCase("26-[Ø-Ø != Ø]", testNumber++,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
            // INV x VP
            yield return BuildEqualsWithStringTestCase("27-[INV != 1P]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndValidProtectedValue.One,
                equalsExpected: false);
            // INV x IP
            yield return BuildEqualsWithStringTestCase("28-[INV != IP1]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.InvalidProtectionValue1,
                equalsExpected: false);
            // INV x Empty
            yield return BuildEqualsWithStringTestCase("29-[INV != Empty]", testNumber++,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndInvalidProtectedValue.EmptyProtectionValue,
                equalsExpected: false);
            // INV x null
            yield return BuildEqualsWithStringTestCase("30-[INV != Ø]", testNumber,
                ProtectedNumberTestNamedInstance.NotInitialized.Invalid,
                ProtectedNumberTestNamedInstance.WithoutValueAndWithoutProtectedValue.Empty,
                equalsExpected: false);
        }

        private static TestCaseData BuildEqualsTestCase(
            string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance leftNamedInstance,
            ProtectedNumberTestNamedInstance rightNamedInstance,
            bool equalsExpected = true)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = leftNamedInstance.Create(),
                OtherInstance = rightNamedInstance.Create(),
                EqualsExpected = equalsExpected
            };
            string automaticTestName =
                $"{testNumber:00}-[{leftNamedInstance.Name} {(equalsExpected ? "==" : "!=")} {rightNamedInstance.Name}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }

        private static TestCaseData BuildEqualsWithLongTestCase(
            string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance namedInstance,
            long? longValue,
            bool equalsExpected = true)
        {
            ProtectedNumberTestCase testCase = new()
            {
                Instance = namedInstance.Create(),
                LongValue = longValue,
                EqualsExpected = equalsExpected
            };
            string automaticTestName =
                $"{testNumber:00}-[{namedInstance.Name} {(equalsExpected ? "==" : "!=")} {(longValue.HasValue ? longValue.Value.ToString() : "null")}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }

        private static TestCaseData BuildEqualsWithStringTestCase(
            string manualTestName, int testNumber,
            ProtectedNumberTestNamedInstance leftNamedInstance,
            ProtectedNumberTestNamedInstance rightNamedInstance,
            bool equalsExpected = true)
        {
            ProtectedNumber right = rightNamedInstance.CreateAsNonNullable();
            string? stringValue = right.HasProtectedValue ? right.ProtectedValue : null;
            ProtectedNumberTestCase testCase = new()
            {
                Instance = leftNamedInstance.Create(),
                StringValue = stringValue,
                EqualsExpected = equalsExpected
            };
            string automaticTestName =
                $"{testNumber:00}-[{leftNamedInstance.Name} {(equalsExpected ? "==" : "!=")} {rightNamedInstance.NameProtectedValuePart}]";

            System.Diagnostics.Debug.Assert(string.Equals(manualTestName, automaticTestName, StringComparison.Ordinal),
                $"[WARN] manual test name don't match automatic test name: manual={manualTestName} / automatic={automaticTestName}");

            return new TestCaseData(testCase)
                    .SetName(automaticTestName)
                ;
        }
    }
}
