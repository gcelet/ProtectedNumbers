// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

using System.Text.RegularExpressions;

// ReSharper disable once PartialTypeWithSinglePart
public static partial class UnitTestProtectionAlgorithm
{
    private const string ProtectionTag = "ut-protection";

    public static string ProtectUsingUnitTestAlgorithm(this long value)
    {
        //  long.MaxValue:  9,223,372,036,854,775,807
        // ulong.MaxValue: 18,446,744,073,709,551,615
        // format        : 00,000,000,000,000,000,000
        return $"<{ProtectionTag}>{value:00000000000000000000}</{ProtectionTag}>";
    }

    public static bool TryUnprotectUsingUnitTestAlgorithm(string protectedValue, out long value)
    {
        Regex regex = UnitTestProtectedValueRegex();
        Match match = regex.Match(protectedValue);

        if (!match.Success)
        {
            value = long.MinValue;
            return false;
        }

        Group valueGroup = match.Groups["value"];

        if (!long.TryParse(valueGroup.Value, out value))
        {
            value = long.MinValue;
            return false;
        }

        return true;
    }

    public static long UnprotectUsingUnitTestAlgorithm(this string protectedValue)
    {
        if (!TryUnprotectUsingUnitTestAlgorithm(protectedValue, out long value))
        {
            throw new ArgumentException($"Can't unprotect value '{protectedValue}'", nameof(protectedValue));
        }

        return value;
    }

    private const string UnitTestProtectedValueRegexPattern =
        "^.*<ut-protection>(?<value>[0-9]{20})</ut-protection>.*$";

#if NET8_0_OR_GREATER
    [GeneratedRegex(UnitTestProtectedValueRegexPattern,
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant |
        RegexOptions.ExplicitCapture)]
    private static partial Regex UnitTestProtectedValueRegex();
#elif NET6_0
    private static Regex UnitTestProtectedValueRegex()
    {
        return UnitTestProtectedValueRegexForNet6;
    }

    private static readonly Regex UnitTestProtectedValueRegexForNet6 = new(UnitTestProtectedValueRegexPattern,
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant |
        RegexOptions.ExplicitCapture);
#endif
}
