// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Samples.Validators;

using FluentValidation;
using ProtectedNumbers.Samples.Models;
using ProtectedNumbers.Samples.Models.Inputs;
using ProtectedNumbers.Samples.Validators.Inputs;

public static class Extensions
{
  public static void AddValidators(this IServiceCollection services)
  {
    services.AddScoped<IValidator<SampleObject>, SampleObjectValidator>();
    services.AddScoped<IValidator<SampleObjectSearch>, SampleObjectSearchValidator>();

    services.AddScoped<IValidator<GetByIdInput>, GetByIdInputValidator>();
  }
}
