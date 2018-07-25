using System;
using Topshelf;

namespace JobManager_RemoteServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<QuartzServer>();
                x.SetDescription(Configuration.Description);
                x.SetDisplayName(Configuration.DisplayName);
                x.SetServiceName(Configuration.ServiceName);
                x.EnablePauseAndContinue();
            });

            Console.ReadKey();
        }
    }
}
