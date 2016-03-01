using System.Data;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace GrainCollection
{
    class FooGrain : Grain, IFoo
    {
        public Task SomeoneSaidHello()
        {
            // do something that we want to mock out here when testing HelloGrain
            throw new DataException("Cannot access the database, make sure the environment is configured correctly");
        }
    }
}
