using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace debug_failure.Services {
    public class MailgunOptions {
        public string ApiKey { get; set; }
        public string Domain { get; set; }
        public string MailFrom { get; set; }
    }

    public class MailgunEmailSender : IEmailSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<MailgunOptions> _options;
        private readonly ILogger<MailgunEmailSender> _logger;

        public MailgunEmailSender(
            IHttpClientFactory httpClientFactory,
            IOptions<MailgunOptions> options,
            ILogger<MailgunEmailSender> logger) {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = _httpClientFactory.CreateClient("mailgun");

            var auth = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes("api" + ":" + _options.Value.ApiKey));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);


            _logger.LogInformation($"Authorisation: {auth}");

            var form = new Dictionary<string, string>();
            form["from"] = _options.Value.MailFrom;
            form["to"] = email;
            form["subject"] = subject;
            form["html"] = htmlMessage;

            _logger.LogInformation($"Sending email to {email}, subject {subject}");

            return client.PostAsync("https://api.mailgun.net/v2/" + _options.Value.Domain + "/messages", new FormUrlEncodedContent(form));
        }
    }
}
