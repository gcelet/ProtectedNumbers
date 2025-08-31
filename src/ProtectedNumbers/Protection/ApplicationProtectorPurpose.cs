// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Protection;

/// <summary>
/// Describes the data protection "purpose" used to derive cryptographic keys for protecting numbers.
/// </summary>
/// <remarks>
/// ASP.NET Core Data Protection isolates protected payloads by purpose. Changing the purpose
/// invalidates previously generated protected values. Sub-purposes allow further scoping
/// (for instance per area/controller/action or tenant) while sharing the same root purpose.
/// </remarks>
/// <param name="Purpose">The root purpose. This should be stable for your application (e.g. "ProtectedNumbers").</param>
/// <param name="SubPurposes">Optional additional scopes appended to the purpose chain.</param>
public record ApplicationProtectorPurpose(string Purpose, params string[]? SubPurposes);
