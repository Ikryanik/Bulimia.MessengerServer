namespace Bulimia.MessengerServer.Domain.Core;

public class UserChatRequest
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
}