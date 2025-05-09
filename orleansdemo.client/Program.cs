using orleansdemo.graininterfaces;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddKeyedAzureTableClient("clustering");
builder.UseOrleansClient();

var app = builder.Build();

app.MapGet("/counter/{grainId}", async (IClusterClient client, string grainId) =>
{
  var grain = client.GetGrain<ICounterGrain>(grainId);
  return await grain.Get();
});

app.MapPost("/counter/{grainId}", async (IClusterClient client, string grainId) =>
{
  var grain = client.GetGrain<ICounterGrain>(grainId);
  return await grain.Increment();
});

app.MapGet("/ping", () => "OK");

app.UseFileServer();

await app.RunAsync();