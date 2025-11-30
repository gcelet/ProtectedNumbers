// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Validators;

using FluentValidation;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;

public class SampleObjectValidator : AbstractValidator<SampleObject>
{
  public SampleObjectValidator()
  {
    RuleFor(e => e.Id)
      .Must(e => e.HasValue && e.Value.HaveProtectedValueAndValue())
      .When(e => e.Id.HasValue)
      .WithMessage("id must be valid")
      ;

    RuleFor(e => e.Name)
      .NotEmpty()
      .WithMessage("name is required")
      ;
  }
}
