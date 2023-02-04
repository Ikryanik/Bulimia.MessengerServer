using System;

namespace Bulimia.MessengerClient.Domain.Core
{
    public class MessageRecord
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime DateTimeOfDelivery { get; set; }
        public string Text { get; set; }
    }
}