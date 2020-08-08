﻿using AdminLTE.StarterKit.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdminLTE.StarterKit.Services
{
	public class EmailService : IEmailService
	{
		private readonly SMTPSettings _settings;

		public EmailService(IOptions<SMTPSettings> appSettings)
		{
			_settings = appSettings.Value;
		}

		public async Task Send(string to, string subject, string html, string from = null, List<IFormFile> files = null)
		{
			// create message
			var email = new MimeMessage();
			email.Sender = MailboxAddress.Parse(from ?? _settings.EmailFrom);
			email.To.Add(MailboxAddress.Parse(to));
			email.Subject = subject;
			var builder = new BodyBuilder();

			if (files != null)
			{
				byte[] fileBytes;

				foreach (var file in files)
				{
					if (file.Length > 0)
					{
						using (var ms = new MemoryStream())
						{
							file.CopyTo(ms);
							fileBytes = ms.ToArray();
						}

						builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
					}
				}
			}

			builder.HtmlBody = html;
			email.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			smtp.Connect(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
			smtp.Authenticate(_settings.SmtpUser, _settings.SmtpPass);
			await smtp.SendAsync(email);
			smtp.Disconnect(true);
		}
	}
}
