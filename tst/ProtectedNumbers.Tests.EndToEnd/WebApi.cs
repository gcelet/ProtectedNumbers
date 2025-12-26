// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

public class WebApi : IEquatable<WebApi>
{
  private WebApi(string name, string resourceName, bool isCustom)
  {
    Name = name;
    ResourceName = resourceName;
    IsCustom = isCustom;
  }

  public static IEnumerable<WebApi> EnumerateCustomWebApi() => EnumerateWebApi(
    includeDotnet6: true, includeDotnet8: true, includeDotnet9: true, includeDotnet10: true,
    includeDefault: false, includeCustom: true);

  public static IEnumerable<WebApi> EnumerateDefaultWebApi() => EnumerateWebApi(
    includeDotnet6: true, includeDotnet8: true, includeDotnet9: true, includeDotnet10: true,
    includeDefault: true, includeCustom: false);

  public static IEnumerable<WebApi> EnumerateWebApi() => EnumerateWebApi(
    includeDotnet6: true, includeDotnet8: true, includeDotnet9: true, includeDotnet10: true,
    includeDefault: true, includeCustom: true);

  public static bool operator ==(WebApi? left, WebApi? right)
  {
    return Equals(left, right);
  }

  public static bool operator !=(WebApi? left, WebApi? right)
  {
    return !Equals(left, right);
  }

  private static IEnumerable<WebApi> EnumerateWebApi(
    bool includeDotnet6, bool includeDotnet8, bool includeDotnet9, bool includeDotnet10,
    bool includeDefault, bool includeCustom)
  {
    if (!includeDefault && !includeCustom)
    {
      yield break;
    }

    List<string> dotnetVersions = new();
    if (includeDotnet6)
    {
      dotnetVersions.Add("dotnet6");
    }
    if (includeDotnet8)
    {
      dotnetVersions.Add("dotnet8");
    }
    if (includeDotnet9)
    {
      dotnetVersions.Add("dotnet9");
    }
    if (includeDotnet10)
    {
      dotnetVersions.Add("dotnet10");
    }

    foreach (string dotnetVersion in dotnetVersions)
    {
      string resourceNameBase = $"webapi-{dotnetVersion}";

      if (includeDefault)
      {
        string nameDefault = $"{dotnetVersion}-default";
        string resourceNameDefault = $"{resourceNameBase}-default";
        WebApi webApiDefault = new(nameDefault, resourceNameDefault, false);

        yield return webApiDefault;
      }

      if (includeCustom)
      {
        string nameCustom = $"{dotnetVersion}-custom";
        string resourceNameCustom = $"{resourceNameBase}-custom";
        WebApi webApiCustom = new(nameCustom, resourceNameCustom, true);

        yield return webApiCustom;
      }
    }
  }

  public bool IsCustom { get; }

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

    return string.Equals(ResourceName, other.ResourceName, StringComparison.Ordinal);
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
  public override int GetHashCode() => ResourceName.GetHashCode();

  /// <inheritdoc />
  public override string ToString() => Name;
}
