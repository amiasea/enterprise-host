using Amiasea.Enterprise.Endpoint.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreEndpoints(builder.Configuration);

var app = builder.Build();

app.MapCoreEndpoints();

app.Run();