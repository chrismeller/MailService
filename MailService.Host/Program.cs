using System;
using System.Threading.Tasks;
using MailService.Server;

namespace MailService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().ConfigureAwait(false).GetAwaiter().GetResult();
        }

	    private static async Task Run()
	    {
		    var server = new MailServer(25, 1000);
		    await server.Run();
	    }
    }
}
