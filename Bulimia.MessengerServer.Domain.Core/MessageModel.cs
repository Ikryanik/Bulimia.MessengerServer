namespace Bulimia.MessengerServer.Domain.Core;

public class MessageModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; }
    
    /// <summary>
    /// Идентификатор отправителя
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// Идентификатор получателя
    /// </summary>
    public int ReceiverId { get; set; }
}