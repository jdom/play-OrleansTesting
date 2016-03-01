using System;
using System.Net;
using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace Host
{
    class Program
    {
        private static SiloHost siloHost;

        static void Main(string[] args)
        {
            // Orleans should run in its own AppDomain, we set it up like this
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
                new AppDomainSetup()
                {
                    AppDomainInitializer = InitSilo
                });


            DoSomeClientWork().Wait();

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            // We do a clean shutdown in the other AppDomain
            hostDomain.DoCallBack(ShutdownSilo);
        }

        static async Task DoSomeClientWork()
        {
            // Orleans comes with a rich XML configuration but we're just going to setup a basic config
            var clientconfig = ClientConfiguration.LocalhostSilo();

            GrainClient.Initialize(clientconfig);

            var friend = GrainClient.GrainFactory.GetGrain<IHello>(0);
            var result = await friend.SayHello("Goodbye");
            Console.WriteLine(result);

        }

        static void InitSilo(string[] args)
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            siloHost = new SiloHost(Dns.GetHostName(), config);

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node", siloHost.Name, siloHost.Type));

        }

        static void ShutdownSilo()
        {
            if (siloHost != null)
            {
                siloHost.Dispose();
                GC.SuppressFinalize(siloHost);
                siloHost = null;
            }
        }
    }
}
