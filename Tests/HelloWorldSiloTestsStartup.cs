using System.Threading.Tasks;
using GrainCollection;
using Orleans;
using Orleans.Providers;
using Tests.Stubs;

namespace Tests
{
    public class HelloWorldSiloTestsStartup : IBootstrapProvider
    {
        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            this.Name = name;
            GrainUtilities.GrainResolutionPrefix = typeof(StubFooGrain).Namespace;
            return TaskDone.Done;
        }

        public Task Close()
        {
            return TaskDone.Done;
        }

        public string Name { get; private set; }
    }
}