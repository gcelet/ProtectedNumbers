// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Validators.Inputs;

using FluentValidation;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models.Inputs;

public class GetByIdInputValidator : AbstractValidator<GetByIdInput>
{
  public GetByIdInputValidator()
  {
    RuleFor(e => e.Id)
      .NotNull()
      ;
    RuleFor(e => e.Id)
      .Must(e => e.HasValue && e.Value.HaveProtectedValueAndValue())
      .When(e => e.Id.HasValue)
      .WithMessage("id must be valid")
      ;
  }
}
