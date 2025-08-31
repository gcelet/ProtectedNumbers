// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Validators;

using FluentValidation;
using ProtectedNumbers.Samples.Models;

public class SampleObjectValidator : AbstractValidator<SampleObject>
{
  public SampleObjectValidator()
  {
    RuleFor(e => e.Id)
      .Must(e => e.HasValue && e.Value.IsInitialized() && e.Value.HasValue)
      .When(e => e.Id.HasValue)
      .WithMessage("id must be valid")
      ;

    RuleFor(e => e.Name)
      .NotEmpty()
      .WithMessage("name is required")
      ;
  }
}
