using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace GrainCollection
{
    class HelloGrain : Grain, IHello
    {
        public async Task<string> SayHello(string msg)
        {
            var id = this.GetPrimaryKeyLong();
            var foo = GrainFactory.GetGrain<IFoo>(id, GrainUtilities.GrainResolutionPrefix);
            await foo.SomeoneSaidHello();
            return string.Format("You said: {0}, I say: Hello!", msg);
        }
    }
}
