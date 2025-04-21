using Orleans.Runtime;
using orleansdemo.graininterfaces;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddKeyedAzureTableClient("clustering");
builder.AddKeyedAzureBlobClient("grain-state");
builder.UseOrleans();

var app = builder.Build();

app.MapGet("/", () => "OK");

await app.RunAsync();

public sealed class CounterGrain(
    [PersistentState("count")] IPersistentState<int> count) : ICounterGrain
{
  private int state = 0;

  public ValueTask<int> Get()
  {
    return ValueTask.FromResult(state);
    // return ValueTask.FromResult(count.State);
  }

  public async ValueTask<int> Increment()
  {
    // var result = ++count.State;
    //    await count.WriteStateAsync()
    state++;
    return await ValueTask.FromResult(state);
  }
}