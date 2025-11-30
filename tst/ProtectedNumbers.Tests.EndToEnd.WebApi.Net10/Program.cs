// Copyright (c) Grégory Célet. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddProtectedNumbersEndToEnd("WebApi.Net10");

WebApplication app = builder.Build();

app.UseProtectedNumbersEndToEnd();
app.Run();
