// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Internal;

using System.Text.Json;

/// <summary>
/// Internal helpers for integrating an <see cref="IServiceProvider"/> with System.Text.Json options.
/// </summary>
internal static class Extensions
{
  /// <summary>
  /// Gets the <see cref="IServiceProvider"/> embedded in <paramref name="options"/>, or throws if none is found.
  /// </summary>
  /// <param name="options">The serializer options to inspect.</param>
  /// <returns>The required <see cref="IServiceProvider"/>.</returns>
  /// <exception cref="InvalidOperationException">Thrown when no service provider was added to the converters.</exception>
  public static IServiceProvider GetRequiredServiceProvider(this JsonSerializerOptions options) =>
    options.GetServiceProvider()
    ?? throw new InvalidOperationException("No service provider found in JSON converters");

  /// <summary>
  /// Attempts to get the <see cref="IServiceProvider"/> previously added to <see cref="JsonSerializerOptions.Converters"/>.
  /// </summary>
  /// <param name="options">The serializer options to inspect.</param>
  /// <returns>The service provider, or <see langword="null"/> if not present.</returns>
  public static IServiceProvider? GetServiceProvider(this JsonSerializerOptions options) =>
    options.Converters.OfType<IServiceProvider>().FirstOrDefault();
}
