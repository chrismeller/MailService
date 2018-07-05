# MailService
A .NET Core console app that hosts an SMTP server that will accept any email delivered to it and store it in-memory.

It also supports limiting the total number of messages that will be retained at any given time as well as the overall "memory use" (as roughly calculated based purely on the size of the email body).

This was inspired by [a blog post on the original architecture of Mailinator](http://mailinator.blogspot.com/2007/01/architecture-of-mailinator.html) and done purely as a fun coding project.
