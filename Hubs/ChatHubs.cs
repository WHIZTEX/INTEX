using Microsoft.AspNetCore.SignalR;
using Ganss.Xss;

namespace newfeature
{
    public class ChatHub : Hub
    {
        private readonly HtmlSanitizer _sanitizer;

        public ChatHub()
        {
            _sanitizer = new HtmlSanitizer();
            // Optionally add or remove allowed tags
            _sanitizer.AllowedTags.Remove("script");
        }

        public async Task SendMessage(string user, string message)
        {
            // Sanitize the message to remove any harmful scripts or HTML
            string sanitizedMessage = _sanitizer.Sanitize(message);

            // Broadcast the sanitized message
            await Clients.All.SendAsync("ReceiveMessage", user, sanitizedMessage);
        }
    }
}
