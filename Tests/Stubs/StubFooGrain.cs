using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace Tests.Stubs
{
    public interface IStubFoo : IFoo
    {
        Task<int> GetSomeoneSaidHelloCallCount();
    }

    public class StubFooGrain : Grain, IFoo, IStubFoo
    {
        private int someoneSaidHelloCallCount;

        public Task SomeoneSaidHello()
        {
            this.someoneSaidHelloCallCount++;
            return TaskDone.Done;
        }

        public Task<int> GetSomeoneSaidHelloCallCount()
        {
            return Task.FromResult(someoneSaidHelloCallCount);
        }
    }
}