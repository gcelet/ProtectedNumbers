// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

using ProtectedNumbers.Internal;
using ProtectedNumbers.Protection;

#if NET6_0
/// <summary>
/// Represents a numeric value that can hold either a raw long value or a protected string representation
/// (for example: hashed, encrypted, or otherwise obfuscated). Provides equality and comparison semantics
/// against both forms and exposes initialization and emptiness states. See <see cref="IProtectedNumber{T}"/>
/// for the general contract implemented by this type.
/// </summary>
#else
/// <inheritdoc cref="IProtectedNumber{T}"/>
#endif
[System.Text.Json.Serialization.JsonConverter(typeof(ProtectedNumberSystemTextJsonConverter))]
[Microsoft.AspNetCore.Mvc.ModelBinder(typeof(ProtectedNumberModelBinder))]
[DebuggerDisplay("[Value={value}/ProtectedValue={protectedValue}]")]
public readonly struct ProtectedNumber : IProtectedNumber<ProtectedNumber>,
  IEquatable<long>, IComparable<long>,
  IEquatable<string>, IComparable<string>
{
  private readonly bool hasProtectedValue;

  private readonly bool isInitialized;

  private readonly string? protectedValue;

#if DEBUG
  private readonly StackTrace? stackTrace = null;
#endif

  private readonly long? value;

  /// <summary>
  /// Initializes an uninitialized instance of <see cref="ProtectedNumber"/>.
  /// </summary>
  /// <remarks>
  /// An instance created with this constructor is not initialized and will throw if members are accessed
  /// that require initialization. Use <see cref="From(long?, string?)"/> or <see cref="Empty"/> to get
  /// an initialized instance.
  /// </remarks>
  public ProtectedNumber()
  {
#if DEBUG
    stackTrace = new StackTrace();
#endif
    value = null;
    hasProtectedValue = false;
    protectedValue = null;
    isInitialized = false;
  }

  private ProtectedNumber(long? value, string? protectedValue)
  {
    this.value = value;
    hasProtectedValue = !string.IsNullOrEmpty(protectedValue);
    this.protectedValue = protectedValue;
    isInitialized = true;
  }

#if NET6_0
  /// <summary>
  /// Gets an empty instance representing the absence of both raw and protected values, but in an initialized state.
  /// </summary>
#else
  /// <inheritdoc cref="IProtectedNumber{T}.Empty"/>
#endif
  public static ProtectedNumber Empty { get; } = new(null, null);

#if NET6_0
  /// <summary>
  /// Creates an instance from either a raw numeric value, a protected representation, or both being null to produce <see cref="Empty"/>.
  /// </summary>
  /// <param name="value">The raw numeric value, or <see langword="null"/>.</param>
  /// <param name="protectedValue">The protected representation, or <see langword="null"/> or empty.</param>
  /// <returns>A new initialized instance.</returns>
#else
  /// <inheritdoc cref="IProtectedNumber{T}.From"/>
#endif
  public static ProtectedNumber From(long? value, string? protectedValue)
  {
    if (!value.HasValue && string.IsNullOrEmpty(protectedValue))
    {
      return Empty;
    }

    return new ProtectedNumber(value, protectedValue);
  }

#if NET6_0
  /// <summary>
  /// Determines whether the specified protected number is <see langword="null"/> or equals <see cref="Empty"/>.
  /// </summary>
  /// <param name="protectedNumber">The instance to test.</param>
  /// <returns><see langword="true"/> if the instance is <see langword="null"/> or empty; otherwise, <see langword="false"/>.</returns>
#else
  /// <inheritdoc cref="IProtectedNumber{T}.Empty"/>
#endif
  public static bool IsNullOrEmpty([NotNullWhen(false)] ProtectedNumber? protectedNumber)
  {
    return !protectedNumber.HasValue || protectedNumber.Value.Equals(Empty);
  }

  /// <summary>
  /// Determines equality between two protected numbers.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if the two instances are considered equal; otherwise, <see langword="false"/>.</returns>
  public static bool operator ==(ProtectedNumber left, ProtectedNumber right)
  {
    return left.Equals(right);
  }

  /// <summary>
  /// Determines equality between a protected number and a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if they represent the same numeric value; otherwise, <see langword="false"/>.</returns>
  public static bool operator ==(ProtectedNumber left, long? right)
  {
    return right.HasValue && left.Equals(right.GetValueOrDefault());
  }

  /// <summary>
  /// Determines equality between a raw numeric value and a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they represent the same numeric value; otherwise, <see langword="false"/>.</returns>
  public static bool operator ==(long? left, ProtectedNumber right)
  {
    return left.HasValue && right.Equals(left.GetValueOrDefault());
  }

  /// <summary>
  /// Determines equality between a protected number and a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if they represent the same protected value; otherwise, <see langword="false"/>.</returns>
  public static bool operator ==(ProtectedNumber left, string? right)
  {
    return left.Equals(right);
  }

  /// <summary>
  /// Determines equality between a protected representation and a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they represent the same protected value; otherwise, <see langword="false"/>.</returns>
  public static bool operator ==(string? left, ProtectedNumber right)
  {
    return right.Equals(left);
  }

  /// <summary>
  /// Determines inequality between two protected numbers.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
  public static bool operator !=(ProtectedNumber left, ProtectedNumber right)
  {
    return !(left == right);
  }

  /// <summary>
  /// Determines inequality between a raw numeric value and a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  public static bool operator !=(long? left, ProtectedNumber right)
  {
    return !(left == right);
  }

  /// <summary>
  /// Determines inequality between a protected number and a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  public static bool operator !=(ProtectedNumber left, long? right)
  {
    return !(left == right);
  }

  /// <summary>
  /// Determines inequality between a protected representation and a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  public static bool operator !=(string? left, ProtectedNumber right)
  {
    return !(left == right);
  }

  /// <summary>
  /// Determines inequality between a protected number and a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if they are not equal; otherwise, <see langword="false"/>.</returns>
  public static bool operator !=(ProtectedNumber left, string? right)
  {
    return !(left == right);
  }

  /// <summary>
  /// Determines whether one protected number is less than another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <(ProtectedNumber left, ProtectedNumber right)
  {
    return left.CompareTo(right) < 0;
  }

  /// <summary>
  /// Determines whether one protected number is less than or equal to another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <=(ProtectedNumber left, ProtectedNumber right)
  {
    return left.CompareTo(right) <= 0;
  }

  /// <summary>
  /// Determines whether one protected number is greater than another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >(ProtectedNumber left, ProtectedNumber right)
  {
    return left.CompareTo(right) > 0;
  }

  /// <summary>
  /// Determines whether one protected number is greater than or equal to another.
  /// </summary>
  /// <param name="left">The left operand.</param>
  /// <param name="right">The right operand.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >=(ProtectedNumber left, ProtectedNumber right)
  {
    return left.CompareTo(right) >= 0;
  }

  /// <summary>
  /// Determines whether a protected number is less than a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <(ProtectedNumber left, long right)
  {
    return left.CompareTo(right) < 0;
  }

  /// <summary>
  /// Determines whether a protected number is less than or equal to a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <=(ProtectedNumber left, long right)
  {
    return left.CompareTo(right) <= 0;
  }

  /// <summary>
  /// Determines whether a protected number is greater than a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >(ProtectedNumber left, long right)
  {
    return left.CompareTo(right) > 0;
  }

  /// <summary>
  /// Determines whether a protected number is greater than or equal to a raw numeric value.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The raw numeric value.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >=(ProtectedNumber left, long right)
  {
    return left.CompareTo(right) >= 0;
  }

  /// <summary>
  /// Determines whether a raw numeric value is less than a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <(long left, ProtectedNumber right)
  {
    return right.CompareTo(left) > 0;
  }

  /// <summary>
  /// Determines whether a raw numeric value is less than or equal to a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <=(long left, ProtectedNumber right)
  {
    return right.CompareTo(left) >= 0;
  }

  /// <summary>
  /// Determines whether a raw numeric value is greater than a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >(long left, ProtectedNumber right)
  {
    return right.CompareTo(left) < 0;
  }

  /// <summary>
  /// Determines whether a raw numeric value is greater than or equal to a protected number.
  /// </summary>
  /// <param name="left">The raw numeric value.</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >=(long left, ProtectedNumber right)
  {
    return right.CompareTo(left) <= 0;
  }

  /// <summary>
  /// Determines whether a protected number is less than a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <(ProtectedNumber left, string? right)
  {
    return left.CompareTo(right) < 0;
  }

  /// <summary>
  /// Determines whether a protected number is less than or equal to a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <=(ProtectedNumber left, string? right)
  {
    return left.CompareTo(right) <= 0;
  }

  /// <summary>
  /// Determines whether a protected number is greater than a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >(ProtectedNumber left, string? right)
  {
    return left.CompareTo(right) > 0;
  }

  /// <summary>
  /// Determines whether a protected number is greater than or equal to a protected representation.
  /// </summary>
  /// <param name="left">The protected number.</param>
  /// <param name="right">The protected representation (string).</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >=(ProtectedNumber left, string? right)
  {
    return left.CompareTo(right) >= 0;
  }

  /// <summary>
  /// Determines whether a protected representation is less than a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <(string? left, ProtectedNumber right)
  {
    return right.CompareTo(left) > 0;
  }

  /// <summary>
  /// Determines whether a protected representation is less than or equal to a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator <=(string? left, ProtectedNumber right)
  {
    return right.CompareTo(left) >= 0;
  }

  /// <summary>
  /// Determines whether a protected representation is greater than a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >(string? left, ProtectedNumber right)
  {
    return right.CompareTo(left) < 0;
  }

  /// <summary>
  /// Determines whether a protected representation is greater than or equal to a protected number.
  /// </summary>
  /// <param name="left">The protected representation (string).</param>
  /// <param name="right">The protected number.</param>
  /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
  public static bool operator >=(string? left, ProtectedNumber right)
  {
    return right.CompareTo(left) <= 0;
  }

  private static ProtectedNumber Deserialize(long? value, string? protectedValue) => new(value, protectedValue);

  /// <summary>
  /// Gets a value indicating whether a protected representation is present.
  /// </summary>
  /// <remarks>
  /// When this property is <see langword="true"/>, <see cref="ProtectedValue"/> can be read; otherwise, accessing
  /// <see cref="ProtectedValue"/> throws an <see cref="InvalidOperationException"/>.
  /// </remarks>
  public bool HasProtectedValue
  {
    get
    {
      EnsureInitialized();
      return hasProtectedValue;
    }
  }

  /// <summary>
  /// Gets a value indicating whether the raw numeric value is present.
  /// </summary>
  /// <inheritdoc/>
  public bool HasValue
  {
    get
    {
      EnsureInitialized();
      return value != null;
    }
  }

  /// <summary>
  /// Gets the protected representation of the number.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when no protected value is present.</exception>
  /// <inheritdoc/>
  public string ProtectedValue
  {
    get
    {
      EnsureInitialized();
      if (!hasProtectedValue)
      {
        throw new InvalidOperationException("No protected value is set");
      }

      return protectedValue!;
    }
  }

  /// <summary>
  /// Gets the raw numeric value.
  /// </summary>
  /// <exception cref="InvalidOperationException">Thrown when no raw value is present.</exception>
  /// <inheritdoc/>
  public long Value
  {
    get
    {
      EnsureInitialized();
      if (!value.HasValue)
      {
        throw new InvalidOperationException("No value is set");
      }

      return value.Value;
    }
  }

  /// <inheritdoc/>
  public override bool Equals(object? obj) => obj is ProtectedNumber protectedNumber && Equals(protectedNumber);

  /// <inheritdoc/>
  public bool Equals(ProtectedNumber other)
  {
    if (!isInitialized && !other.isInitialized)
    {
      return true;
    }

    if (!isInitialized || !other.isInitialized)
    {
      return false;
    }

    if (value != null && other.value != null)
    {
      return EqualityComparer<long>.Default.Equals(Value, other.Value);
    }

    if (value != null || other.value != null)
    {
      return false;
    }

    if (hasProtectedValue && other.hasProtectedValue)
    {
      return string.Equals(protectedValue, other.protectedValue, StringComparison.Ordinal);
    }

    if (hasProtectedValue || other.hasProtectedValue)
    {
      return false;
    }

    if (value == null && other.value == null && !hasProtectedValue && !other.hasProtectedValue)
    {
      return true;
    }

    return false;
  }

  /// <inheritdoc cref="IEquatable{T}.Equals" />
  public bool Equals(long l) => isInitialized && value.Equals(l);

  /// <inheritdoc cref="IEquatable{T}.Equals" />
  public bool Equals(string? s) => isInitialized && hasProtectedValue &&
                                   StringComparer.Ordinal.Equals(protectedValue, s);

  /// <inheritdoc/>
  public override int GetHashCode()
  {
    if (!isInitialized)
    {
      return 0;
    }

    int salt = value != null
      ? 250924889
      : -1521134295;

    // NOTE: If we have a value, we discard the protected value because
    int protectedValueHashCode = value != null
      ? 0
      : protectedValue?.GetHashCode(StringComparison.Ordinal) ?? 0;

    return HashCode.Combine(isInitialized, salt, value, protectedValueHashCode);
  }

  /// <inheritdoc/>
  public override string ToString()
  {
    StringBuilder stringBuilder = new("[", 32);

    stringBuilder
      .Append(value != null ? value.Value.ToString() : "Ø")
      .Append("-")
      .Append(hasProtectedValue ? protectedValue! : "Ø")
      .Append("]")
      ;

    return stringBuilder.ToString();
  }

  /// <inheritdoc/>
  public int CompareTo(string? other)
  {
    if (!isInitialized)
    {
      return 1;
    }

    if (!hasProtectedValue)
    {
      return other == null ? 0 : 1;
    }

    int protectedValueCompare = string.CompareOrdinal(protectedValue, other);

    return protectedValueCompare switch
    {
      0 => 0,
      < 0 => -1,
      > 0 => 1,
    };
  }

  /// <inheritdoc/>
  public int CompareTo(long other)
  {
    if (!isInitialized || !value.HasValue)
    {
      return 1;
    }

    return value.Value.CompareTo(other);
  }

  /// <inheritdoc/>
  public int CompareTo(ProtectedNumber other)
  {
    /*
     * Less than zero: This instance precedes other in the sort order.
     * Zero: This instance occurs in the same position in the sort order as others.
     * Greater than zero: This instance follows other in the sort order.
     */

    if (!isInitialized && !other.isInitialized)
    {
      return 0;
    }

    if (!isInitialized && other.isInitialized)
    {
      return 1;
    }

    if (isInitialized && !other.isInitialized)
    {
      return -1;
    }

    if (value == null && other.value != null)
    {
      return 1;
    }

    if (value != null && other.value == null)
    {
      return -1;
    }

    if (value != null && other.value != null)
    {
      int valueCompare = value.Value.CompareTo(other.value.Value);

      return valueCompare;
    }

    if (!hasProtectedValue && other.hasProtectedValue)
    {
      return 1;
    }

    if (hasProtectedValue && !other.hasProtectedValue)
    {
      return -1;
    }

    if (hasProtectedValue && other.hasProtectedValue)
    {
      int protectedValueCompare = string.CompareOrdinal(protectedValue, other.protectedValue);

      return protectedValueCompare switch
      {
        0 => 0,
        < 0 => -1,
        > 0 => 1,
      };
    }

    return 0;
  }

  /// <inheritdoc cref="IComparable" />
  public int CompareTo(object? other)
  {
    if (other is null)
    {
      return 1;
    }

    if (other is ProtectedNumber protectedNumber)
    {
      return CompareTo(protectedNumber);
    }

    throw new ArgumentException("Cannot compare to object as it is not of type ProtectedNumber", nameof(other));
  }

  /// <inheritdoc/>
  public bool Equals(ProtectedNumber other, IEqualityComparer<ProtectedNumber> comparer) => comparer.Equals(this, other);

  /// <inheritdoc/>
  public bool IsInitialized() => isInitialized;

  /// <inheritdoc />
  public ProtectedNumber WithValue(long newValue)
  {
    EnsureInitialized();

    return From(newValue, protectedValue);
  }

  /// <inheritdoc />
  public ProtectedNumber WithProtectedValue(string newProtectedValue)
  {
    EnsureInitialized();

    return From(value, newProtectedValue);
  }

  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
  private void EnsureInitialized()
  {
    if (!IsInitialized())
    {
#if DEBUG
      throw new NotSupportedException($"Use of uninitialized {nameof(ProtectedNumber)} at: " + stackTrace);
#else
            throw new NotSupportedException($"Use of uninitialized {nameof(ProtectedNumber)}");
#endif
    }
  }

#if NET6_0
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
#else
  /// <inheritdoc cref="IProtectedNumber{T}.BindAsync"/>
#endif
  public static ValueTask<ProtectedNumber?> BindAsync(HttpContext context, ParameterInfo parameter)
  {
    if (context is null)
    {
      throw new ArgumentNullException(nameof(context));
    }

    if (parameter is null)
    {
      throw new ArgumentNullException(nameof(parameter));
    }

    string? name = parameter.Name;
    string? inputValue = null;

    if (!string.IsNullOrEmpty(name))
    {
      if (context.Request.RouteValues.TryGetValue(name, out object? routeValue))
      {
        inputValue = routeValue?.ToString();
      }

      if (string.IsNullOrEmpty(inputValue) &&
          context.Request.Query.TryGetValue(name, out StringValues queryValues) && queryValues.Count > 0)
      {
        inputValue = queryValues[0];
      }
    }

    TryBind(context, inputValue, out ProtectedNumber protectedNumber);

    return ValueTask.FromResult<ProtectedNumber?>(protectedNumber);
  }

#if NET6_0
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
#else
  /// <inheritdoc cref="IProtectedNumber{T}.TryBind"/>
#endif
  public static bool TryBind(HttpContext context, string? inputValue, out ProtectedNumber protectedNumber)
  {
    protectedNumber = From(null, inputValue);
    IApplicationDataProtector? applicationDataProtector =
      context.RequestServices.GetService<IApplicationDataProtector>();

    if (string.IsNullOrEmpty(inputValue) || applicationDataProtector is null)
    {
      return true;
    }

    if (applicationDataProtector.TryUnprotect(protectedNumber, out ProtectedNumber? unprotected))
    {
      protectedNumber = unprotected.Value;
      return true;
    }

    // If unprotect fails, return false to indicate error in the raw protected number (no ModelState in Minimal APIs).
    return false;
  }

#if NET6_0
  /// <summary>
  /// Attempts to parse the specified string into a protected number using an optional format provider.
  /// </summary>
  /// <param name="s">The input string, expected to be a protected representation; may be <see langword="null"/>.</param>
  /// <param name="provider">An optional format provider (ignored).</param>
  /// <param name="result">When this method returns, contains the resulting protected number.</param>
  /// <returns>
  /// Always returns <see langword="true"/>. The resulting instance encapsulates the provided string as a protected representation.
  /// </returns>
#else
  /// <inheritdoc cref="IParsable{T}.TryParse(string,IFormatProvider,out T)"/>
#endif
  public static bool TryParse(string? s, IFormatProvider? provider, out ProtectedNumber result)
  {
    // HACK: To provide automatic unprotection during model binding, we need to instantiate an IHttpContextAccessor to access services
    // I would prefer that BindAsync is always used because it has an HttpContext parameter.
    // But for query string parameter in Minimal APi, it's required to provide a static TryParse method...
    // that doesn't have a HttpContext parameter
    IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
    HttpContext? httpContext = httpContextAccessor.HttpContext;

    if (httpContext == null || !TryBind(httpContext, s, out result))
    {
      result = From(null, s);
    }

    return true;
  }

#if NET6_0
  /// <summary>Parses a string into a value.</summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s" />.</param>
  /// <exception cref="T:System.ArgumentNullException">
  /// <paramref name="s" /> is <see langword="null" />.</exception>
  /// <exception cref="T:System.FormatException">
  /// <paramref name="s" /> is not in the correct format.</exception>
  /// <exception cref="T:System.OverflowException">
  /// <paramref name="s" /> is not representable by.</exception>
  /// <returns>The result of parsing <paramref name="s" />.</returns>
#else
  /// <inheritdoc cref="IParsable{T}.Parse(string,IFormatProvider)"/>
#endif
  public static ProtectedNumber Parse(string? s, IFormatProvider? provider)
  {
    ProtectedNumber protectedNumber = TryParse(s, provider, out ProtectedNumber result)
      ? result
      : From(null, s);

    return protectedNumber;
  }

  internal class ProtectedNumberModelBinder : IModelBinder
  {
    /// <inheritdoc />
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
      string modelName = bindingContext.ModelName;
      ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

      if (valueProviderResult == ValueProviderResult.None)
      {
        if (bindingContext.ModelMetadata.IsNullableValueType)
        {
          return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(Empty);

        return Task.CompletedTask;
      }

      string? protectedValue = valueProviderResult.FirstValue;
      HttpContext httpContext = bindingContext.HttpContext;

      if (!TryBind(httpContext, protectedValue, out ProtectedNumber protectedNumber))
      {
        bindingContext.ModelState.TryAddModelError(modelName, "invalid protected number");
      }

      bindingContext.Result = ModelBindingResult.Success(protectedNumber);
      return Task.CompletedTask;
    }
  }

  private class ProtectedNumberSystemTextJsonConverter : System.Text.Json.Serialization.JsonConverter<ProtectedNumber>
  {
    private static ProtectedNumber DeserializeJson(
      string? protectedValue,
      System.Text.Json.JsonSerializerOptions options)
    {
      ProtectedNumber protectedNumber = From(null, protectedValue);

      if (protectedNumber.hasProtectedValue &&
          TryGetApplicationDataProtector(options, out IApplicationDataProtector? applicationDataProtector) &&
          applicationDataProtector.TryUnprotect(protectedNumber, out ProtectedNumber? unprotectedNumber))
      {
        protectedNumber = unprotectedNumber ?? protectedNumber;
      }

      return protectedNumber;
    }

    private static string? SerializeJson(
      ProtectedNumber protectedNumber,
      System.Text.Json.JsonSerializerOptions options)
    {
      protectedNumber.EnsureInitialized();
      if (protectedNumber.hasProtectedValue)
      {
        return protectedNumber.protectedValue;
      }

      if (protectedNumber.value == null ||
          !TryGetApplicationDataProtector(options, out IApplicationDataProtector? applicationDataProtector))
      {
        return null;
      }

      ProtectedNumber otherProtectedNumber = applicationDataProtector.Protect(protectedNumber);

      return otherProtectedNumber.hasProtectedValue ? otherProtectedNumber.protectedValue : null;
    }

    private static bool TryGetApplicationDataProtector(
      System.Text.Json.JsonSerializerOptions options,
      [NotNullWhen(true)] out IApplicationDataProtector? applicationDataProtector)
    {
      IServiceProvider? serviceProvider = options.GetServiceProvider();

      if (serviceProvider == null)
      {
        applicationDataProtector = null;
        return false;
      }

      applicationDataProtector =
        serviceProvider.GetService<IApplicationDataProtector>();

      return applicationDataProtector != null;
    }

    /// <inheritdoc/>
    public override ProtectedNumber Read(
      ref System.Text.Json.Utf8JsonReader reader,
      Type typeToConvert,
      System.Text.Json.JsonSerializerOptions options) =>
      DeserializeJson(reader.GetString(), options);

    /// <inheritdoc/>
    public override ProtectedNumber ReadAsPropertyName(
      ref System.Text.Json.Utf8JsonReader reader,
      Type typeToConvert, System.Text.Json.JsonSerializerOptions options) =>
      DeserializeJson(reader.GetString(), options);

    /// <inheritdoc/>
    public override void Write(
      System.Text.Json.Utf8JsonWriter writer,
      ProtectedNumber value,
      System.Text.Json.JsonSerializerOptions options) =>
      System.Text.Json.JsonSerializer.Serialize(writer, SerializeJson(value, options), options);

    /// <inheritdoc/>
    public override void WriteAsPropertyName(
      System.Text.Json.Utf8JsonWriter writer,
      ProtectedNumber value,
      System.Text.Json.JsonSerializerOptions options)
    {
      string? serializeAsPropertyName = SerializeJson(value, options);

      if (serializeAsPropertyName == null)
      {
        throw new ArgumentException("can't be use as property name", nameof(value));
      }

      writer.WritePropertyName(serializeAsPropertyName);
    }
  }
}
