using System;

namespace Bulimia.MessengerClient.Domain.Core
{
    public class Chat
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime DateTimeOfLastMessage { get; set; }
        public string LastMessage { get; set; }
        public string SenderUsernameOfMessage { get; set; }
    }
}