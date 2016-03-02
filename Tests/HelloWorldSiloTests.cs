using System;
using System.Threading.Tasks;
using GrainCollection;
using GrainInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orleans.TestingHost;
using Tests.Stubs;

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
                        AdjustConfig = clusterConfig =>
                        {
                            clusterConfig.Globals.RegisterBootstrapProvider<HelloWorldSiloTestsStartup>("HelloWorldSiloTests");
                        }
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

        [TestMethod]
        public async Task WhenSayingHelloShouldCallFoo()
        {
            long id = GetRandomGrainId();
            IHello grain = host.GrainFactory.GetGrain<IHello>(id);
            var stubFoo = host.GrainFactory.GetGrain<IStubFoo>(id);
            Assert.AreEqual(0, await stubFoo.GetSomeoneSaidHelloCallCount());

            await grain.SayHello("hello 1");
            Assert.AreEqual(1, await stubFoo.GetSomeoneSaidHelloCallCount());

            await grain.SayHello("hello 2");
            Assert.AreEqual(2, await stubFoo.GetSomeoneSaidHelloCallCount());
        }

        [TestMethod]
        public async Task WIP()
        {
            // could talk to Silo appdomain like this
            host.Primary.AppDomain.DoCallBack(DoSomethingInSilo);

            // could instead show exception handling in grain calls by making IFoo throw

            // could use DI also
        }

        public static void DoSomethingInSilo()
        {
            GrainUtilities.GrainResolutionPrefix = "SomethingElse";
        }
    }
}
