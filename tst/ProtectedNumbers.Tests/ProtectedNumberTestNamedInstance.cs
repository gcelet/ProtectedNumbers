// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests;

public sealed class ProtectedNumberTestNamedInstance
{
    public const string INVALID_PROTECTION_VALUE1 = "INVALID-PROTECTION-1";

    public const string INVALID_PROTECTION_VALUE2 = "INVALID-PROTECTION-2";

    public required Func<ProtectedNumber?> Create { get; init; }

    public required string Name { get; init; }

    public required string NameProtectedValuePart { get; init; }

    public required string NameValuePart { get; init; }

    public ProtectedNumber CreateAsNonNullable()
    {
        return Create() ?? ProtectedNumber.Empty;
    }

    public static class NotInitialized
    {
        /// <summary>
        /// INV
        /// </summary>
        public static ProtectedNumberTestNamedInstance Invalid { get; } = new()
        {
            Name = "INV",
            NameValuePart = "INV",
            NameProtectedValuePart = "INV",
            Create = () => new ProtectedNumber()
        };

        /// <summary>
        /// null
        /// </summary>
        public static ProtectedNumberTestNamedInstance Null { get; } = new()
        {
            Name = "null",
            NameValuePart = "null",
            NameProtectedValuePart = "null",
            Create = () => null
        };
    }

    public static class WithoutValueAndInvalidProtectedValue
    {
        /// <summary>
        /// Ø-Empty
        /// </summary>
        public static ProtectedNumberTestNamedInstance EmptyProtectionValue { get; } = new()
        {
            Name = "Ø-Empty",
            NameValuePart = "Ø",
            NameProtectedValuePart = "Empty",
            Create = () => ProtectedNumber.From(null, string.Empty)
        };

        /// <summary>
        /// Ø-IP1
        /// </summary>
        public static ProtectedNumberTestNamedInstance InvalidProtectionValue1 { get; } = new()
        {
            Name = "Ø-IP1",
            NameValuePart = "Ø",
            NameProtectedValuePart = "IP1",
            Create = () => ProtectedNumber.From(null, INVALID_PROTECTION_VALUE1)
        };

        /// <summary>
        /// Ø-IP2
        /// </summary>
        public static ProtectedNumberTestNamedInstance InvalidProtectionValue2 { get; } = new()
        {
            Name = "Ø-IP2",
            NameValuePart = "Ø",
            NameProtectedValuePart = "IP2",
            Create = () => ProtectedNumber.From(null, INVALID_PROTECTION_VALUE2)
        };
    }

    public static class WithoutValueAndValidProtectedValue
    {
        /// <summary>
        /// Ø-1P
        /// </summary>
        public static ProtectedNumberTestNamedInstance One { get; } = new()
        {
            Name = "Ø-1P",
            NameValuePart = "Ø",
            NameProtectedValuePart = "1P",
            Create = () => ProtectedNumber.From(null, 1L.ProtectUsingUnitTestAlgorithm())
        };

        /// <summary>
        /// Ø-2P
        /// </summary>
        public static ProtectedNumberTestNamedInstance Two { get; } = new()
        {
            Name = "Ø-2P",
            NameValuePart = "Ø",
            NameProtectedValuePart = "2P",
            Create = () => ProtectedNumber.From(null, 2L.ProtectUsingUnitTestAlgorithm())
        };
    }

    public static class WithoutValueAndWithoutProtectedValue
    {
        /// <summary>
        /// Ø-Ø
        /// </summary>
        public static ProtectedNumberTestNamedInstance Empty { get; } = new()
        {
            Name = "Ø-Ø",
            NameValuePart = "Ø",
            NameProtectedValuePart = "Ø",
            Create = () => ProtectedNumber.From(null, null)
        };

        /// <summary>
        /// Empty
        /// </summary>
        public static ProtectedNumberTestNamedInstance EmptyStatic { get; } = new()
        {
            Name = "Empty",
            NameValuePart = "Empty",
            NameProtectedValuePart = "Empty",
            Create = () => ProtectedNumber.Empty
        };
    }

    public static class WithValueAndInvalidProtectedValue
    {
        /// <summary>
        /// 1-IP1
        /// </summary>
        public static ProtectedNumberTestNamedInstance OneInvalidProtectionValue1 { get; } = new()
        {
            Name = "1-IP1",
            NameValuePart = "1",
            NameProtectedValuePart = "IP1",
            Create = () => ProtectedNumber.From(1L, INVALID_PROTECTION_VALUE1)
        };

        /// <summary>
        /// 1-IP2
        /// </summary>
        public static ProtectedNumberTestNamedInstance OneInvalidProtectionValue2 { get; } = new()
        {
            Name = "1-IP2",
            NameValuePart = "1",
            NameProtectedValuePart = "IP2",
            Create = () => ProtectedNumber.From(1L, INVALID_PROTECTION_VALUE2)
        };

        /// <summary>
        /// 2-IP1
        /// </summary>
        public static ProtectedNumberTestNamedInstance TwoInvalidProtectionValue1 { get; } = new()
        {
            Name = "2-IP1",
            NameValuePart = "2",
            NameProtectedValuePart = "IP1",
            Create = () => ProtectedNumber.From(2L, INVALID_PROTECTION_VALUE1)
        };

        /// <summary>
        /// 2-IP2
        /// </summary>
        public static ProtectedNumberTestNamedInstance TwoInvalidProtectionValue2 { get; } = new()
        {
            Name = "2-IP2",
            NameValuePart = "2",
            NameProtectedValuePart = "IP2",
            Create = () => ProtectedNumber.From(2L, INVALID_PROTECTION_VALUE2)
        };
    }

    public static class WithValueAndValidProtectedValue
    {
        /// <summary>
        /// 1-1P
        /// </summary>
        public static ProtectedNumberTestNamedInstance One { get; } = new()
        {
            Name = "1-1P",
            NameValuePart = "1",
            NameProtectedValuePart = "1P",
            Create = () => ProtectedNumber.From(1L, 1L.ProtectUsingUnitTestAlgorithm())
        };

        /// <summary>
        /// 2-2P
        /// </summary>
        public static ProtectedNumberTestNamedInstance Two { get; } = new()
        {
            Name = "2-2P",
            NameValuePart = "2",
            NameProtectedValuePart = "2P",
            Create = () => ProtectedNumber.From(2L, 2L.ProtectUsingUnitTestAlgorithm())
        };
    }

    public static class WithValueAndWithoutProtectedValue
    {
        /// <summary>
        /// 1-Ø
        /// </summary>
        public static ProtectedNumberTestNamedInstance One { get; } = new()
        {
            Name = "1-Ø",
            NameValuePart = "1",
            NameProtectedValuePart = "Ø",
            Create = () => ProtectedNumber.From(1L, null)
        };

        /// <summary>
        /// 2-Ø
        /// </summary>
        public static ProtectedNumberTestNamedInstance Two { get; } = new()
        {
            Name = "2-Ø",
            NameValuePart = "2",
            NameProtectedValuePart = "Ø",
            Create = () => ProtectedNumber.From(2L, null)
        };
    }
}
