// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

public class WebApi : IEquatable<WebApi>
{
  private WebApi(string name, string resourceName)
  {
    Name = name;
    ResourceName = resourceName;
  }

  public static IEnumerable<WebApi> EnumerateWebApi()
  {
    string[] dotnetVersions = [
      "dotnet6",
      "dotnet8",
      "dotnet9",
      "dotnet10"
    ];

    foreach (string dotnetVersion in dotnetVersions)
    {
      string resourceName = $"webapi-{dotnetVersion}";
      WebApi webApi = new(dotnetVersion, resourceName);

      yield return webApi;
    }
  }

  public static bool operator ==(WebApi? left, WebApi? right)
  {
    return Equals(left, right);
  }

  public static bool operator !=(WebApi? left, WebApi? right)
  {
    return !Equals(left, right);
  }

  public string Name { get; }

  public string ResourceName { get; }

  /// <inheritdoc />
  public bool Equals(WebApi? other)
  {
    if (other is null)
    {
      return false;
    }

    if (ReferenceEquals(this, other))
    {
      return true;
    }

    return Name == other.Name;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj)
  {
    if (obj is null)
    {
      return false;
    }

    if (ReferenceEquals(this, obj))
    {
      return true;
    }

    if (obj.GetType() != GetType())
    {
      return false;
    }

    return Equals((WebApi)obj);
  }

  /// <inheritdoc />
  public override int GetHashCode() => Name.GetHashCode();

  /// <inheritdoc />
  public override string ToString() => Name;
}
