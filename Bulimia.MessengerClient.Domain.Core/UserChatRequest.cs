namespace Bulimia.MessengerClient.Domain.Core
{
    public class UserChatRequest
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}