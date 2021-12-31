using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Autohmation
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Domain.System.Instance.Run();
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
