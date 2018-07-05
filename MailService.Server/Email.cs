using System;
using System.Collections.Generic;

namespace MailService.Server
{
	public class Email
	{
		public string From { get; set; }
		public List<string> To { get; set; }
		public List<string> Cc { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool HadAttachments { get; set; }

		public DateTimeOffset ReceivedAt { get; set; }
	}
}