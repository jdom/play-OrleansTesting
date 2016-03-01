using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IFoo : IGrainWithIntegerKey
    {
        Task SomeoneSaidHello();
    }
}
