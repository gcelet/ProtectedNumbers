// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Customization;

public class EndToEndOptions
{
  public string? DataProtectionKeysPath { get; set; }

  public bool UseCustomSaltProvider { get; set; }
}
