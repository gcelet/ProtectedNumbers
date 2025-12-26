// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
namespace ProtectedNumbers.Tests.EndToEnd.Shared;

using FastEndpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProtectedNumbers.Tests.EndToEnd.Shared.Customization;
using ProtectedNumbers.Tests.EndToEnd.Shared.MinimalApi;
using ProtectedNumbers.Tests.EndToEnd.Shared.Repositories;
using ProtectedNumbers.Tests.EndToEnd.Shared.Validators;

#if NET8_0_OR_GREATER
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Configuration;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Configuration;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
#endif

public static class Extensions
{
  public static void AddProtectedNumbersEndToEnd(this WebApplicationBuilder builder, string webAppName)
  {
    EndToEndOptions endToEndOptions = new();

    builder.Configuration.Bind("EndToEnd", endToEndOptions);

    if (!string.IsNullOrEmpty(endToEndOptions.DataProtectionKeysPath) && !Directory.Exists(endToEndOptions.DataProtectionKeysPath!))
    {
      string absolutePath = Path.GetFullPath(endToEndOptions.DataProtectionKeysPath!);

      throw new DirectoryNotFoundException($"Data protection keys path does not exist: {absolutePath}");
    }

    builder.Services.AddHttpLogging(opts =>
    {
#if NET8_0_OR_GREATER
      opts.CombineLogs = true;
#endif
      opts.LoggingFields = HttpLoggingFields.All;
    });

    // Required for ProtectedNumbers to work correctly
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddDataProtection(opts =>
      {
        opts.ApplicationDiscriminator = $"ProtectedNumbers.Tests.EndToEnd.{webAppName}";
      })
      .PersistKeysToFileSystem(new DirectoryInfo(endToEndOptions.DataProtectionKeysPath!) )
      ;

    builder.Services.AddProtectedNumbers(opts =>
    {
      if (endToEndOptions.UseCustomSaltProvider)
      {
        opts.UseApplicationDataSaltProvider<ApplicationDataSaltProviderCustom>();
      }
    });

    builder.Services.AddControllers();
    builder.Services.AddFastEndpoints();

#if NET6_0
#elif NET8_0_OR_GREATER
    builder.Services.AddFluentValidationAutoValidation((AutoValidationMvcConfiguration cfg) =>
    {
      cfg.ValidationStrategy = ValidationStrategy.All;
    });

    builder.Services.AddFluentValidationAutoValidation((AutoValidationEndpointsConfiguration _) =>
    {
    });
#endif

    builder.Services.AddSingleton<SampleObjectRepository>();
    builder.Services.AddValidators();
  }

  public static void UseProtectedNumbersEndToEnd(this WebApplication app)
  {
    app.UseHttpLogging();
    app.RegisterMinimalApiEndpoints();
    app.MapControllers();
    app.UseFastEndpoints(cfg =>
    {
      cfg.Endpoints.RoutePrefix = "fast-endpoints";
    });
  }
}
