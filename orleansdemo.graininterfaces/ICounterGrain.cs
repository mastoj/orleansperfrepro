namespace orleansdemo.graininterfaces;
using Orleans;

public interface ICounterGrain : IGrainWithStringKey
{
    ValueTask<int> Increment();
    ValueTask<int> Get();
}