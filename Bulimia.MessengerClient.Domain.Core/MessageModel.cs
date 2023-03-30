using System;

namespace Bulimia.MessengerClient.Domain.Core
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}