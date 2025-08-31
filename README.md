<!-- markdownlint-disable MD033 MD041 -->
<div align="center">

<img src="PackageIcon.png" alt="ProtectedNumbers" width="150px"/>

# ProtectedNumbers

[![](https://img.shields.io/github/actions/workflow/status/gcelet/ProtectedNumbers/build.yml?branch=main)](https://github.com/gcelet/ProtectedNumbers/actions?query=branch%3Amain)
[![Coverage](https://img.shields.io/coverallsCoverage/github/gcelet/ProtectedNumbers?branch=main)](https://coveralls.io/github/gcelet/ProtectedNumbers?branch=main)
[![](https://img.shields.io/github/release/gcelet/ProtectedNumbers.svg?label=latest%20release&color=007edf)](https://github.com/gcelet/ProtectedNumbers/releases/latest)
[![](https://img.shields.io/nuget/dt/ProtectedNumbers.svg?label=nuget%20downloads&color=007edf&logo=nuget)](https://www.nuget.org/packages/ProtectedNumbers)
[![](https://img.shields.io/librariesio/dependents/nuget/ProtectedNumbers.svg?label=dependent%20libraries)](https://libraries.io/nuget/ProtectedNumbers)
![GitHub Repo stars](https://img.shields.io/github/stars/gcelet/ProtectedNumbers?style=flat)
[![GitHub contributors](https://img.shields.io/github/contributors/gcelet/ProtectedNumbers)](https://github.com/gcelet/ProtectedNumbers/graphs/contributors)
[![GitHub last commit](https://img.shields.io/github/last-commit/gcelet/ProtectedNumbers)](https://github.com/gcelet/ProtectedNumbers)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/m/gcelet/ProtectedNumbers)](https://github.com/gcelet/ProtectedNumbers/graphs/commit-activity)
[![open issues](https://img.shields.io/github/issues/gcelet/ProtectedNumbers)](https://github.com/gcelet/ProtectedNumbers/issues)
![.NET](https://img.shields.io/badge/net-6.0%20%7C%208.0%20%7C%209.0-512BD4)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](https://github.com/gcelet/ProtectedNumbers/pulls)
![](https://img.shields.io/badge/release%20strategy-gitversion%20%2B%20ci-orange.svg)


A .NET library for safe, opaque numeric identifiers you can pass through URLs, JSON, and model binding
</div>

## What problem it solved

When working with a legacy database, most often primary keys are sequential numbers like this:

```json
{
  "id": "4018378694012108869",
  "name": "some data row"
}
```

And if you expose the data as is on a public endpoint, an attacker can guess easily other identifiers:

```json
{
  "id": "4018378694012108868",
  "name": "oops some other row"
}
```

There are ways to mitigate this problem. One of them is to make identifiers unguessable:

```json
{
  "id": "CfDJ8K3W-Fa_koRAlAI935b9MJjgVrkgEMGZ5bvAPECdKZvgMpUPsvhVru9een2MVC0P8e4N4ehwgKgueAGjZlEP4s7gu3AsBLRoQrnUZr8hw4qZYAr-StAlQWCfx2FHTs1EGgEpB3T9YPSGxAxm0XVP7Mc",
  "name": "some data row"
}
```

## What is ProtectedNumbers

- ProtectedNumbers provides a lightweight struct, ProtectedNumber, that carries either a raw numeric value (long) or a protected string representation (for example for URLs or JSON), and can safely round-trip between the two with application-provided protection.
- First-class ASP.NET Core integration is included:
  - Dependency injection helpers via services.AddProtectedNumbers().
  - MVC model binding for ProtectedNumber and ProtectedNumber? parameters.
  - System.Text.Json support.
- Supported target frameworks: net6.0, net8.0, net9.0.

Key semantics of ProtectedNumber:
- Dual representation: Value (long) and ProtectedValue (string). Either side can be present independently.
- Empty sentinel: ProtectedNumber.Empty is initialized but HasValue == false and HasProtectedValue == false. Accessing Value or ProtectedValue on an empty instance throws InvalidOperationException.
- Utility helpers: ProtectedNumber.IsNullOrEmpty(ProtectedNumber?) and initialization helpers to ensure safe usage.


### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [ProtectedNumbers](https://www.nuget.org/packages/ProtectedNumbers/) from the package manager console:

```
PM> Install-Package ProtectedNumbers
```
Or from the .NET CLI as:
```
dotnet add package ProtectedNumbers
```

## How do I get started?

- Add ProtectedNumbers services:

```csharp
// ...
IServiceCollection services;
// ...

services.AddProtectedNumbers();
```

- Replace the property type from int/long to ProtectedNumber:

```csharp
// Before
public class SomeViewModel
{
  // ...
  public long? Id { get; set; }
  // ...
}

// After
public class SomeViewModel
{
  // ...
  public ProtectedNumbers? Id { get;set; }
  // ...
}
```

- Example of json output from an api endpoint:

Before:
```http
GET http://localhost:5000/minimal-api/samples-objects/4018378694012108869
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{
  "id": "4018378694012108869",
  "name": "some data row"
}
```

After:
```http
GET http://localhost:5000/minimal-api/samples-objects/CfDJ8K3W-Fa_koRAlAI935b9MJiwgpOlgJ5oj2hPOyJZZFqlHZB7JZKF5ccvazbH8_u4rtp1Ek-orXITi05_0MFOGj0Erm77rxFfpBQcfHVk04M-eaTiWkTNm54PwtmSdEZY5Iy9pGCCo_fYsFj2K54T89c
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{
  "id": "CfDJ8K3W-Fa_koRAlAI935b9MJjgVrkgEMGZ5bvAPECdKZvgMpUPsvhVru9een2MVC0P8e4N4ehwgKgueAGjZlEP4s7gu3AsBLRoQrnUZr8hw4qZYAr-StAlQWCfx2FHTs1EGgEpB3T9YPSGxAxm0XVP7Mc",
  "name": "some data row"
}
```

### Do you have an issue?

If you're still running into problems, file an issue above.

### License, etc.

ProtectedNumbers is Copyright &copy; 2025 Grégory Célet under the [MIT license](LICENSE).
