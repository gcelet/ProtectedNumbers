// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers;

#if NET8_0_OR_GREATER
using System.Reflection;

using Microsoft.AspNetCore.Http;
#endif

/// <summary>
/// Defines a numeric value type that can hold either a raw numeric value or a corresponding protected representation
/// (for example: hashed, encrypted, or otherwise obfuscated). Implementations provide comparison and equality
/// semantics against both forms and allow detecting initialization and emptiness states.
/// </summary>
/// <typeparam name="TSelf">The concrete protected number type implementing this interface.</typeparam>
public interface IProtectedNumber<TSelf> :
#if NET6_0
  IEquatable<TSelf>,
  IComparable<TSelf>
#elif NET8_0_OR_GREATER
  IEquatable<TSelf>,
  IComparable<TSelf>,
  IParsable<TSelf>
#endif
  where TSelf : struct, IProtectedNumber<TSelf>
{
  /// <summary>
  /// Gets a value indicating whether a protected representation is present.
  /// </summary>
  /// <remarks>
  /// When this property is <see langword="true"/>, <see cref="ProtectedValue"/> can be read; otherwise, accessing
  /// <see cref="ProtectedValue"/> should throw an <see cref="InvalidOperationException"/>.
  /// </remarks>
  bool HasProtectedValue { get; }

  /// <summary>
  /// Gets a value indicating whether the raw numeric value is present.
  /// </summary>
  /// <remarks>
  /// When this property is <see langword="true"/>, <see cref="Value"/> can be read; otherwise, accessing
  /// <see cref="Value"/> should throw an <see cref="InvalidOperationException"/>.
  /// </remarks>
  bool HasValue { get; }

  /// <summary>
  /// Gets the protected representation of the number.
  /// </summary>
  /// <value>A non-null, non-empty string representing the protected value.</value>
  /// <exception cref="InvalidOperationException">Thrown when no protected value is present.</exception>
  string ProtectedValue { get; }

  /// <summary>
  /// Gets the raw numeric value.
  /// </summary>
  /// <value>The numeric value.</value>
  /// <exception cref="InvalidOperationException">Thrown when no raw value is present.</exception>
  long Value { get; }

  /// <summary>
  /// Determines whether this instance and the specified instance are equal using the provided comparer.
  /// </summary>
  /// <param name="other">The other protected number to compare with this instance.</param>
  /// <param name="comparer">The equality comparer to use for comparison.</param>
  /// <returns><see langword="true"/> if the comparer considers the two instances equal; otherwise, <see langword="false"/>.</returns>
  bool Equals(TSelf other, IEqualityComparer<TSelf> comparer);

  /// <summary>
  /// Determines whether this instance equals the specified raw numeric value.
  /// </summary>
  /// <param name="value">The numeric value to compare to.</param>
  /// <returns><see langword="true"/> if this instance represents the same numeric value; otherwise, <see langword="false"/>.</returns>
  bool Equals(long value);

  /// <summary>
  /// Determines whether this instance equals the specified protected representation.
  /// </summary>
  /// <param name="protectedValue">The protected representation to compare to.</param>
  /// <returns><see langword="true"/> if this instance represents the same protected value; otherwise, <see langword="false"/>.</returns>
  bool Equals(string? protectedValue);

  /// <summary>
  /// Indicates whether this instance was properly initialized.
  /// </summary>
  /// <remarks>
  /// An uninitialized instance typically results from using the default constructor. Implementations may treat
  /// uninitialized values differently from Empty, which represents a valid, initialized "no value" state.
  /// </remarks>
  /// <returns><see langword="true"/> if the instance has been initialized; otherwise, <see langword="false"/>.</returns>
  bool IsInitialized();

  /// <summary>
  /// Returns a new instance based on the current one with the specified raw numeric value set.
  /// </summary>
  /// <param name="value">The raw numeric value to associate with the instance.</param>
  /// <returns>
  /// A new initialized instance that preserves the current protected representation (if any) and has
  /// <see cref="HasValue"/> equal to <see langword="true"/> and <see cref="Value"/> equal to <paramref name="value"/>.
  /// </returns>
  /// <remarks>
  /// Implementations are expected to be immutable; this method does not modify the current instance but returns
  /// a new one. Calling this method on an uninitialized instance should result in a <see cref="NotSupportedException"/>.
  /// </remarks>
  TSelf WithValue(long value);

  /// <summary>
  /// Returns a new instance based on the current one with the specified protected representation set.
  /// </summary>
  /// <param name="protectedValue">The protected representation to associate with the instance. If empty, the protected
  /// representation is considered absent and <see cref="HasProtectedValue"/> will be <see langword="false"/>.</param>
  /// <returns>
  /// A new initialized instance that preserves the current raw numeric value (if any) and reflects the provided
  /// protected representation.
  /// </returns>
  /// <remarks>
  /// Implementations are expected to be immutable; this method does not modify the current instance but returns
  /// a new one. Calling this method on an uninitialized instance should result in a <see cref="NotSupportedException"/>.
  /// </remarks>
  TSelf WithProtectedValue(string protectedValue);

#if NET8_0_OR_GREATER
  /// <summary>
  /// Gets an empty instance representing the absence of both raw and protected values, but in an initialized state.
  /// </summary>
  static abstract TSelf Empty { get; }

  /// <summary>
  /// Creates an instance from either a raw numeric value, a protected representation, or both being null to produce <see cref="Empty"/>.
  /// </summary>
  /// <param name="value">The raw numeric value, or <see langword="null"/>.</param>
  /// <param name="protectedValue">The protected representation, or <see langword="null"/> or empty.</param>
  /// <returns>A new initialized instance.</returns>
  static abstract TSelf From(long? value, string? protectedValue);

  /// <summary>
  /// Determines whether the specified protected number is <see langword="null"/> or equals <see cref="Empty"/>.
  /// </summary>
  /// <param name="protectedNumber">The instance to test.</param>
  /// <returns><see langword="true"/> if the instance is <see langword="null"/> or empty; otherwise, <see langword="false"/>.</returns>
  static abstract bool IsNullOrEmpty(TSelf? protectedNumber);

  /// <summary>
  /// Binds a value for this protected number type from the current HTTP request context (Minimal APIs binding).
  /// </summary>
  /// <param name="context">The current HTTP request context.</param>
  /// <param name="parameter">The endpoint parameter metadata describing the target argument.</param>
  /// <remarks>
  /// Implementations should attempt to resolve the parameter by name from route values first, then from the query string.
  /// If a non-empty protected representation is found and an <see cref="ProtectedNumbers.Protection.IApplicationDataProtector"/>
  /// service is available, they may attempt to unprotect it. If unprotection fails or no protector is registered, the raw
  /// protected representation should be returned as-is.
  /// </remarks>
  /// <returns>
  /// A task that yields either an initialized protected number or <see langword="null"/> when a value cannot be bound.
  /// </returns>
  static abstract ValueTask<TSelf?> BindAsync(HttpContext context, ParameterInfo parameter);

  /// <summary>
  /// Attempts to bind a ProtectedNumber from the given HTTP request context and input value.
  /// </summary>
  /// <param name="context">The current HTTP request context used to resolve services such as the data protector.</param>
  /// <param name="inputValue">The raw input value to bind, typically coming from route or query; may be null or empty.</param>
  /// <param name="protectedNumber">
  /// When this method returns, contains the resulting ProtectedNumber. If an application data protector is available
  /// and the <paramref name="inputValue"/> represents a protected value that can be successfully unprotected,
  /// this will contain the unprotected number; otherwise it will encapsulate the raw <paramref name="inputValue"/>.
  /// </param>
  /// <returns>
  /// True when binding succeeded, including cases where <paramref name="inputValue"/> is null/empty or no data protector
  /// is registered. Returns false only when a data protector is present but unprotecting the provided protected value fails.
  /// </returns>
  /// <remarks>
  /// This helper is intended for Minimal APIs and other binding scenarios where there is no ModelState to record errors.
  /// Therefore, a failure to unprotect results in a <see langword="false"/> return value, allowing the caller to decide how to react.
  /// </remarks>
  static abstract bool TryBind(HttpContext context, string? inputValue, out TSelf protectedNumber);

  /// <summary>
  /// Determines equality between two protected numbers.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if the two instances are considered equal; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator ==(TSelf left, TSelf right);

  /// <summary>
  /// Determines equality between a protected number and a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if they represent the same numeric value; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator ==(TSelf left, long? right);

  /// <summary>
  /// Determines equality between a raw numeric value and a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they represent the same numeric value; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator ==(long? left, TSelf right);

  /// <summary>
  /// Determines equality between a protected number and a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if they represent the same protected value; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator ==(TSelf left, string? right);

  /// <summary>
  /// Determines equality between a protected representation and a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they represent the same protected value; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator ==(string? left, TSelf right);

  /// <summary>
  /// Determines inequality between two protected numbers.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator !=(TSelf left, TSelf right);

  /// <summary>
  /// Determines inequality between a protected number and a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator !=(TSelf left, long? right);

  /// <summary>
  /// Determines inequality between a raw numeric value and a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator !=(long? left, TSelf right);

  /// <summary>
  /// Determines inequality between a protected number and a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator !=(TSelf left, string? right);

  /// <summary>
  /// Determines inequality between a protected representation and a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator !=(string? left, TSelf right);

  /// <summary>
  /// Determines whether one protected number is less than another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <(TSelf left, TSelf right);

  /// <summary>
  /// Determines whether one protected number is less than or equal to another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <=(TSelf left, TSelf right);

  /// <summary>
  /// Determines whether one protected number is greater than another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >(TSelf left, TSelf right);

  /// <summary>
  /// Determines whether one protected number is greater than or equal to another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >=(TSelf left, TSelf right);

  /// <summary>
  /// Determines whether a protected number is less than a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <(TSelf left, long right);

  /// <summary>
  /// Determines whether a protected number is less than or equal to a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <=(TSelf left, long right);

  /// <summary>
  /// Determines whether a protected number is greater than a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >(TSelf left, long right);

  /// <summary>
  /// Determines whether a protected number is greater than or equal to a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >=(TSelf left, long right);

  /// <summary>
  /// Determines whether a raw numeric value is less than a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <(long left, TSelf right);

  /// <summary>
  /// Determines whether a raw numeric value is less than or equal to a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <=(long left, TSelf right);

  /// <summary>
  /// Determines whether a raw numeric value is greater than a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >(long left, TSelf right);

  /// <summary>
  /// Determines whether a raw numeric value is greater than or equal to a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >=(long left, TSelf right);

  /// <summary>
  /// Determines whether a protected number is less than a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <(TSelf left, string? right);

  /// <summary>
  /// Determines whether a protected number is less than or equal to a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <=(TSelf left, string? right);

  /// <summary>
  /// Determines whether a protected number is greater than a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >(TSelf left, string? right);

  /// <summary>
  /// Determines whether a protected number is greater than or equal to a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >=(TSelf left, string? right);

  /// <summary>
  /// Determines whether a protected representation is less than a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <(string? left, TSelf right);

  /// <summary>
  /// Determines whether a protected representation is less than or equal to a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator <=(string? left, TSelf right);

  /// <summary>
  /// Determines whether a protected representation is greater than a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >(string? left, TSelf right);

  /// <summary>
  /// Determines whether a protected representation is greater than or equal to a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  static abstract bool operator >=(string? left, TSelf right);
#endif
}
