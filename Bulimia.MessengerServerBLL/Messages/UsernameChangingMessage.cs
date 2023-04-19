using System.Windows;

namespace Bulimia.MessengerClient.Messages
{
    public class UsernameChangingMessage
    {
        public string Username { get; set; }
        public bool UsernameVisibility => !string.IsNullOrWhiteSpace(Username);

        public UsernameChangingMessage(string username)
        {
            Username = username;
        }
    }
}