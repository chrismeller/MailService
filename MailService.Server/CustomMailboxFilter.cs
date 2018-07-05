using System;
using System.Threading;
using System.Threading.Tasks;
using SmtpServer;
using SmtpServer.Mail;
using SmtpServer.Storage;

namespace MailService.Server
{
	public class CustomMailboxFilter : IMailboxFilter, IMailboxFilterFactory
	{
		public Task<MailboxFilterResult> CanAcceptFromAsync(ISessionContext context, IMailbox from, int size, CancellationToken cancellationToken)
		{
			if (String.Equals(@from.Host, "test.com"))
			{
				return Task.FromResult(MailboxFilterResult.Yes);
			}

			return Task.FromResult(MailboxFilterResult.NoPermanently);
		}

		public Task<MailboxFilterResult> CanDeliverToAsync(ISessionContext context, IMailbox to, IMailbox from, CancellationToken cancellationToken)
		{
			return Task.FromResult(MailboxFilterResult.Yes);
		}

		public IMailboxFilter CreateInstance(ISessionContext context)
		{
			return new CustomMailboxFilter();
		}
	}
}