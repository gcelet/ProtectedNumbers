// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using FastEndpoints;
using Microsoft.AspNetCore.DataProtection;
using ProtectedNumbers;
using ProtectedNumbers.Samples.MinimalApi;
using ProtectedNumbers.Samples.Repositories;
using ProtectedNumbers.Samples.Validators;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Configuration;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Configuration;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

// Required for ProtectedNumbers to work correctly
services.AddHttpContextAccessor();
services.AddDataProtection(opts =>
  {
    opts.ApplicationDiscriminator = "ProtectedNumbers.Samples";
  })
  .PersistKeysToFileSystem(new DirectoryInfo("./DataProtectionKeys"))
  ;

services.AddProtectedNumbers();

services.AddControllers();
services.AddFastEndpoints();
services.AddFluentValidationAutoValidation((AutoValidationMvcConfiguration cfg) =>
{
  cfg.ValidationStrategy = ValidationStrategy.All;
});

services.AddFluentValidationAutoValidation((AutoValidationEndpointsConfiguration _) =>
{
});

services.AddSingleton<SampleObjectRepository>();
services.AddValidators();

WebApplication app = builder.Build();

app.RegisterMinimalApiEndpoints();
app.MapControllers();
app.UseFastEndpoints(cfg =>
{
  cfg.Endpoints.RoutePrefix = "fast-endpoints";
});

app.Run();
