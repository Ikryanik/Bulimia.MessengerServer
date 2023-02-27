using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            if (content == null)
                return null;

            var chats = JsonConvert.DeserializeObject<List<Chat>>(content);

            return chats;
        }

        public async Task<List<MessageDto>> GetMessages(int mainUserId, int companionUserId)
        {
            var request = new UserChatRequest
            {
                ReceiverId = mainUserId,
                SenderId = companionUserId
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

            var companionResponse =
                await BaseRepository.Client.PostAsync(Api.GetUsernameById + $"?id={companionUserId}", null);

            var companionUsername = await companionResponse.Content.ReadAsStringAsync();

            var mainUserResponse =
                await BaseRepository.Client.PostAsync(Api.GetUsernameById + $"?id={mainUserId}", null);

            var mainUsername = await mainUserResponse.Content.ReadAsStringAsync();

            var companions = new List<(int, string)>
            {
                (mainUserId, mainUsername ),
                (companionUserId, companionUsername)
            };

            var items = new List<MessageDto>(messageRecords.Capacity);

            foreach (var message in messageRecords)
            {
                items.Add(Map(message, companions, mainUserId));
            }

            return items;
        }

        private MessageDto Map(MessageRecord message, List<(int, string)> companions, int mainUserId)
        {
            return new MessageDto(mainUserId)            
            {
                Id = message.Id,
                Text = message.Text,
                SenderId = message.SenderId,
                DateTimeOfDelivery = message.DateTimeOfDelivery,
                ReceiverUsername = companions.FirstOrDefault(x=>x.Item1 == message.ReceiverId).Item2,
                SenderUsername = companions.FirstOrDefault(x=>x.Item1 == message.SenderId).Item2
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

        public async Task<int> DeleteMessage(int id)
        {
            var responce = await BaseRepository.Client.PostAsync(Api.DeleteMessage + $"?id={id}", null);
            if (!responce.IsSuccessStatusCode)
                return 0;

            var content = await responce.Content.ReadAsStringAsync();
            if (content == null)
                return 0;

            return JsonConvert.DeserializeObject<int>(content);
        }
    }
}