using System.Collections.Generic;

namespace Bulimia.MessengerClient.Domain.Core
{
    public class UpdateInfo
    {
        public List<Chat> Chats { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}