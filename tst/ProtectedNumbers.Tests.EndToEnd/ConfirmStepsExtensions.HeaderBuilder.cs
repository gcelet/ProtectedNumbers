// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd;

using Microsoft.Net.Http.Headers;

public static partial class ConfirmStepsExtensions
{
  private const string UserIdTemplate = "{{" + UserId + "}}";

  extension(HeaderBuilder headerBuilder)
  {
    public HeaderBuilder AppendDefaultHeaders() =>
      headerBuilder
        .Header(HeaderNames.ContentType, "application/json");

    // ReSharper disable once UnusedMethodReturnValue.Local
    public HeaderBuilder AppendVaryHeaders() =>
      headerBuilder
        .Header("X-ProtectedNumbers-Context", UserIdTemplate)
        .Header("X-ProtectedNumbers-Temporal", DateTime.UnixEpoch.Ticks.ToString());
  }
}
