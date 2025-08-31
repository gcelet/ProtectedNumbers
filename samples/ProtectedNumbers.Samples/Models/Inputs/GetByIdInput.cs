// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Models.Inputs;

using Microsoft.AspNetCore.Mvc;

public class GetByIdInput
{
  [FromRoute]
  public ProtectedNumber? Id { get; set; }
}
