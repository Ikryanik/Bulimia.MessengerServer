namespace Bulimia.MessengerServer.Domain.Core
{
    public class MessageDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }
    }
}