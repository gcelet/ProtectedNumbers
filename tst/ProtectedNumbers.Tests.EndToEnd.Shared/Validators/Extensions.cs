// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared.Validators;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using ProtectedNumbers.Tests.EndToEnd.Shared.Models;
using ProtectedNumbers.Tests.EndToEnd.Shared.Models.Inputs;
using ProtectedNumbers.Tests.EndToEnd.Shared.Validators.Inputs;

public static class Extensions
{
  public static void AddValidators(this IServiceCollection services)
  {
    services.AddScoped<IValidator<SampleObject>, SampleObjectValidator>();
    services.AddScoped<IValidator<SampleObjectSearch>, SampleObjectSearchValidator>();

    services.AddScoped<IValidator<GetByIdInput>, GetByIdInputValidator>();
  }

  public static bool HaveProtectedValueAndValue(this ProtectedNumber protectedNumber)
  {
    return protectedNumber.IsInitialized() &&
           protectedNumber is { HasProtectedValue: true, HasValue: true };
  }

  public static ValidationProblemDetails ToValidationProblemDetails(this ValidationResult validationResult)
  {
    ValidationProblemDetails validationProblemDetails = new(validationResult.ToDictionary())
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "One or more validation errors occurred.",
    };

    return validationProblemDetails;
  }
}
