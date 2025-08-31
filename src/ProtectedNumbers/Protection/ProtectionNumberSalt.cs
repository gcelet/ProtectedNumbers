namespace ProtectedNumbers.Protection;

/// <summary>
/// Represents a structure holding prefix and suffix salt values.
/// This record is used for incorporating additional security in operations
/// by combining these salts with sensitive data.
/// </summary>
/// <param name="SaltPrefix">The prefix salt.</param>
/// <param name="SaltSuffix">The suffix salt.</param>
public record ProtectionNumberSalt(string? SaltPrefix, string? SaltSuffix);
