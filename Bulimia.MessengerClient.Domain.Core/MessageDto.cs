using System;

namespace Bulimia.MessengerClient.Domain.Core
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
        public DateTime DateTimeOfDelivery { get; set; }
        public string Text { get; set; }
    }
}