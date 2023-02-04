using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bulimia.MessengerClient.Domain.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bulimia.MessengerClient.DAL.Repositories
{
    public class MessageRepository
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public async Task<List<Chat>> GetUserChats(int id)
        {
            var response = await BaseRepository.Client.PostAsync(Api.GetChats + $"?id={id}", null);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            if (content == null)
                return null;

            var chats = JsonConvert.DeserializeObject<List<Chat>>(content);

            return chats;
        }

        public async Task<List<MessageDto>> GetMessages(int senderId, int receiverId)
        {
            var request = new UserChatRequest
            {
                ReceiverId = receiverId,
                SenderId = senderId
            };

            var json = JsonConvert.SerializeObject(request);

            var response = await BaseRepository.Client.PostAsync(Api.GetMessages,
                new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            if (content == null)
                return null;

            var messageRecords = JsonConvert.DeserializeObject<List<MessageRecord>>(content);

            var receiverResponse =
                await BaseRepository.Client.PostAsync(Api.GetUsernameById + $"?id={receiverId}", null);

            var receiverUsername = await receiverResponse.Content.ReadAsStringAsync();

            var senderResponse =
                await BaseRepository.Client.PostAsync(Api.GetUsernameById + $"?id={senderId}", null);

            var senderUsername = await senderResponse.Content.ReadAsStringAsync();

            var items = new List<MessageDto>(messageRecords.Capacity);

            foreach (var message in messageRecords)
            {
                items.Add(Map(message, senderUsername, receiverUsername));
            }

            return items;
        }

        private MessageDto Map(MessageRecord message, string senderUsername, string receiverUsername)
        {
            return new MessageDto            
            {
                Text = message.Text,
                DateTimeOfDelivery = message.DateTimeOfDelivery,
                Id = message.Id,
                ReceiverUsername = receiverUsername,
                SenderUsername = senderUsername
            };
        }

        public async Task<int> CreateMessage(MessageModel message)
        {
            var json = JsonConvert.SerializeObject(message);

            var responce = await BaseRepository.Client.PostAsync(Api.CreateMessage,
                new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await responce.Content.ReadAsStringAsync();

            if (content == null)
                return 0;

            var messageId = JsonConvert.DeserializeObject<int>(content);

            return messageId;
        }
    }
}