using System;
using System.Threading;
using System.Threading.Tasks;
using SmtpServer;
using SmtpServer.Authentication;

namespace MailService.Server
{
	public class CustomUserAuthenticator : IUserAuthenticator, IUserAuthenticatorFactory
	{
		public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken token)
		{
			Console.WriteLine("User={0} Password={1}", user, password);

			return Task.FromResult(user.Length > 4);
		}

		public IUserAuthenticator CreateInstance(ISessionContext context)
		{
			return new CustomUserAuthenticator();
		}
	}
}