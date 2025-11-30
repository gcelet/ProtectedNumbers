// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net6>("webapi-dotnet6");
builder.AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net8>("webapi-dotnet8");
builder.AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net9>("webapi-dotnet9");
builder.AddProject<Projects.ProtectedNumbers_Tests_EndToEnd_WebApi_Net10>("webapi-dotnet10");

builder.Build().Run();
