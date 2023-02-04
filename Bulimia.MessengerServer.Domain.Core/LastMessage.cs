namespace Bulimia.MessengerServer.Domain.Core;

public class LastMessage
{
    public string Username { get; set; }
    public DateTime DateTimeOfLastMessage { get; set; }
    public string Text { get; set; }
    public string SenderUsernameOfMessage { get; set; }
}