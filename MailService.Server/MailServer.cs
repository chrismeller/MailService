using System.Threading;
using System.Threading.Tasks;
using SmtpServer;

namespace MailService.Server
{
	public class MailServer
	{
		private ISmtpServerOptions _options;

		public MailServer(int? numMessages = null, int? memoryLimit = null)
		{
			var options = new SmtpServerOptionsBuilder()
				.ServerName("localhost")
				.Port(25, 587)
				.Port(465, isSecure: true)
				//.Certificate(CreateX509Certificate2())
				.MessageStore(new InMemoryMessageStore(numMessages, memoryLimit))
				.MailboxFilter(new CustomMailboxFilter())
				.UserAuthenticator(new CustomUserAuthenticator())
				.Build();

			_options = options;
		}

		public async Task Run()
		{
			var smtpServer = new SmtpServer.SmtpServer(_options);
			await smtpServer.StartAsync(CancellationToken.None);
		}
	}
}