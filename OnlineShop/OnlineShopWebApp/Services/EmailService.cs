using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Cryptography;

namespace OnlineShopWebApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOrderKeysAsync(string toEmail, string customerName, Guid orderId, List<(string ProductName, string Key)> keys)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.UserName));
            message.To.Add(new MailboxAddress(customerName, toEmail));
            message.Subject = $"Ваш заказ #{orderId.ToString()[..8].ToUpper()} — ключи активации";

            var body = BuildHtmlBody(customerName, orderId, keys);
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.UserName, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public static string GenerateKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var bytes = new byte[25];
            RandomNumberGenerator.Fill(bytes);
            var segment = (int start) => new string(bytes[start..(start + 5)].Select(b => chars[b % chars.Length]).ToArray());
            return $"{segment(0)}-{segment(5)}-{segment(10)}-{segment(15)}-{segment(20)}";
        }

        private static string BuildHtmlBody(string name, Guid orderId, List<(string ProductName, string Key)> keys)
        {
            var rows = string.Join("", keys.Select(k => $"""
                <tr>
                    <td style="padding:10px 16px;border-bottom:1px solid #2a2a3a;">{k.ProductName}</td>
                    <td style="padding:10px 16px;border-bottom:1px solid #2a2a3a;font-family:monospace;letter-spacing:2px;color:#4fc3f7;">{k.Key}</td>
                </tr>
                """));

            return $"""
                <!DOCTYPE html>
                <html lang="ru">
                <head><meta charset="utf-8"></head>
                <body style="margin:0;padding:0;background:#0f0f1a;font-family:Arial,sans-serif;color:#e0e0e0;">
                  <table width="100%" cellpadding="0" cellspacing="0">
                    <tr><td align="center" style="padding:40px 20px;">
                      <table width="600" cellpadding="0" cellspacing="0" style="background:#1a1a2e;border-radius:12px;overflow:hidden;">
                        <tr>
                          <td style="background:#16213e;padding:24px 32px;">
                            <h1 style="margin:0;font-size:24px;color:#4fc3f7;">SteamKooper</h1>
                          </td>
                        </tr>
                        <tr>
                          <td style="padding:32px;">
                            <h2 style="margin:0 0 8px;color:#ffffff;">Спасибо за покупку, {name}!</h2>
                            <p style="margin:0 0 24px;color:#9e9e9e;">Заказ №&nbsp;{orderId.ToString()[..8].ToUpper()}</p>
                            <p style="margin:0 0 16px;">Ниже представлены ключи активации для ваших игр:</p>
                            <table width="100%" cellpadding="0" cellspacing="0" style="background:#0f0f1a;border-radius:8px;overflow:hidden;">
                              <thead>
                                <tr style="background:#16213e;">
                                  <th style="padding:10px 16px;text-align:left;color:#9e9e9e;font-weight:normal;">Игра</th>
                                  <th style="padding:10px 16px;text-align:left;color:#9e9e9e;font-weight:normal;">Ключ активации</th>
                                </tr>
                              </thead>
                              <tbody>{rows}</tbody>
                            </table>
                            <p style="margin:24px 0 0;font-size:13px;color:#757575;">
                              Введите ключ в библиотеку игр для активации. Каждый ключ действует однократно.
                            </p>
                          </td>
                        </tr>
                        <tr>
                          <td style="background:#16213e;padding:16px 32px;text-align:center;font-size:12px;color:#616161;">
                            © 2026 SteamKooper. Если вы не совершали этот заказ — проигнорируйте письмо.
                          </td>
                        </tr>
                      </table>
                    </td></tr>
                  </table>
                </body>
                </html>
                """;
        }
    }
}
