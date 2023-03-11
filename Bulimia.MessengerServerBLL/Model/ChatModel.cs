using System;
using ReactiveUI;

namespace Bulimia.MessengerClient.Model
{
    public class ChatModel : ReactiveObject
    {
        public string Username { get; set; }
        public DateTime Time { get; set; }
        public string LastMessage { get; set; }
    }
}