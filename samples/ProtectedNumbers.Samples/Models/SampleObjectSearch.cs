// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Models;

using Microsoft.AspNetCore.Mvc;

public class SampleObjectSearch
{
  [FromQuery]
  public ProtectedNumber? Id { get; set; }

  [FromQuery]
  public ProtectedNumber[]? Ids { get; set; }
}
