using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MimeKit;
using SmtpServer;
using SmtpServer.Mail;
using SmtpServer.Protocol;
using SmtpServer.Storage;

namespace MailService.Server
{
	public class InMemoryMessageStore : MessageStore
	{
		private static List<Email> _messages = new List<Email>();
		private static int? _numMessages { get; set; }
		private static int? _memoryLimit { get; set; }

		public InMemoryMessageStore(int? numMessages = null, int? memoryLimit = null)
		{
			_numMessages = numMessages;
			_memoryLimit = memoryLimit;
		}

		public override Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, CancellationToken cancellationToken)
		{
			var textMessage = (ITextMessage)transaction.Message;

			var message = MimeKit.MimeMessage.Load(textMessage.Content);
			Console.WriteLine("New message from {0}: {1}", message.From, message.Subject);

			var email = new Email()
			{
				Body = message.HtmlBody,
				Cc = message.Cc.Cast<MailboxAddress>().Select(x => x.Address).ToList(),
				From = message.From.ToString(),
				HadAttachments = message.Attachments.Any(),
				Subject = message.Subject,
				To = message.To.Cast<MailboxAddress>().Select(x => x.Address).ToList(),

				ReceivedAt = DateTimeOffset.UtcNow,
			};

			_messages.Add(email);

			TrimStorage();

			return Task.FromResult(SmtpResponse.Ok);
		}

		private void TrimStorage()
		{
			var startNum = _messages.Count;
			var startSize = _messages.Sum(x => Encoding.Unicode.GetByteCount(x.Body));

			if (_numMessages.HasValue)
			{
				_messages = _messages.OrderByDescending(x => x.ReceivedAt).Take(_numMessages.Value).ToList();
			}

			if (_memoryLimit.HasValue)
			{
				var total = 0;
				_messages = _messages.OrderByDescending(x => x.ReceivedAt).TakeWhile(x =>
				{
					var size = Encoding.Unicode.GetByteCount(x.Body);

					total = total + size;

					return (total <= _memoryLimit.Value);
				}).ToList();
			}

			var endNum = _messages.Count;
			var endSize = _messages.Sum(x => Encoding.Unicode.GetByteCount(x.Body));

			if (startNum != endNum || startSize != endSize)
			{
				Console.WriteLine("Message storage trimmed:");
				Console.WriteLine("\tStart Number {0}, End Number {1}", startNum, endNum);
				Console.WriteLine("\tStart Size {0}, End Size {1}", startSize, endSize);
			}
		}
	}
}