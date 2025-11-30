// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Validators;

using FluentValidation;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;

public class SampleObjectSearchValidator : AbstractValidator<SampleObjectSearch>
{
  public SampleObjectSearchValidator()
  {
    RuleFor(i => i.Id)
      .Must(i => i.HasValue && i.Value.HaveProtectedValueAndValue())
      .When(i => i.Id.HasValue)
      .WithMessage("id must be valid")
      ;

    RuleForEach(i => i.Ids)
      .Must(i => i.HaveProtectedValueAndValue())
      .When(i => i.Ids != null && i.Ids.Length > 0)
      .WithMessage("ids must be valid")
      ;
  }
}
