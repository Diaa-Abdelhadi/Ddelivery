using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ddelivery.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiToken = _configuration["MailTrap:ApiToken"];
            var inboxId = _configuration["MailTrap:InboxId"];

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

            var requestBody = new
            {
                to = new[] { new { email = email } },
                from = new { email = "info@ddelivery.com", name = "Ddelivery Admin" },
                subject = subject,
                html = htmlMessage
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"https://sandbox.api.mailtrap.io/api/send/{inboxId}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Console.WriteLine($"Mailtrap API Error: {error}");
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Network Error: {ex.Message}");
            }
        }
    }
}
