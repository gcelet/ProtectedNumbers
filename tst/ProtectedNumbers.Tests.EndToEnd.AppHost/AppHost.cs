// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

// Default options
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net6>("webapi-dotnet6-default")
  ;
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net8>("webapi-dotnet8-default")
  ;
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net9>("webapi-dotnet9-default")
  ;
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net10>("webapi-dotnet10-default")
  ;

// Custom options
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net6>("webapi-dotnet6-custom")
  .WithEnvironment("ENDTOEND__USECUSTOMSALTPROVIDER", "true")
  ;
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net8>("webapi-dotnet8-custom")
  .WithEnvironment("ENDTOEND__USECUSTOMSALTPROVIDER", "true")
  ;
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net9>("webapi-dotnet9-custom")
  .WithEnvironment("ENDTOEND__USECUSTOMSALTPROVIDER", "true")
  ;
builder
  .AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net10>("webapi-dotnet10-custom")
  .WithEnvironment("ENDTOEND__USECUSTOMSALTPROVIDER", "true")
  ;

builder.Build().Run();
