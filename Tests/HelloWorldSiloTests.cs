using System;
using System.Threading.Tasks;
using GrainInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orleans.TestingHost;

namespace Tests
{
    [TestClass]
    public class HelloWorldSiloTests
    {
        private static TestingSiloHost host;

        [ClassCleanup]
        public static void StopOrleans()
        {
            // Optional. 
            // By default, the next test class which uses TestignSiloHost will
            // cause a fresh Orleans silo environment to be created.
            host.StopAllSilos();
        }

        [TestInitialize]
        public void InitializeOrleans()
        {
            if (host == null)
            {
                host = new TestingSiloHost(
                    new TestingSiloOptions
                    {
                        StartSecondary = false,
                    });
            }
        }

        private static long GetRandomGrainId()
        {
            return new Random().Next();
        }

        [TestMethod]
        public async Task SayHelloTest()
        {
            // The Orleans silo / client test environment is already set up at this point.

            long id = GetRandomGrainId();
            const string greeting = "Bonjour";

            IHello grain = host.GrainFactory.GetGrain<IHello>(id);

            // This will create and call a Hello grain with specified 'id' in one of the test silos.
            string reply = await grain.SayHello(greeting);

            Assert.IsNotNull(reply, "Grain replied with some message");
            string expected = string.Format("You said: {0}, I say: Hello!", greeting);
            Assert.AreEqual(expected, reply, "Grain replied with expected message");
        }
    }
}
