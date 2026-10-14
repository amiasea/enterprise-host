using Wolverine;
using Amiasea.Enterprise.Endpoint.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreEndpoints(builder.Configuration);

builder.Host.UseWolverine();

var app = builder.Build();

app.MapCoreEndpoints();

await app.RunWithGraphQLCommandsAsync(args);