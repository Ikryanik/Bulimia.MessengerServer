using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bulimia.MessengerClient.DAL.Repositories;
using Bulimia.MessengerClient.Domain.Core;

namespace Bulimia.MessengerClient.BLL
{
    public class ChatClient
    {
        private readonly MessageRepository _messageRepository;

        public ChatClient()
        {
            _messageRepository = new MessageRepository();
        }

        public async Task<List<int>> GetUpdates(int myId)
        {
            try
            {
                return await ExecutionService.Execute(() => _messageRepository.GetUpdates(myId));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        public async Task<List<MessageDto>> GetMessagesUpdates(int myId, int companionId)
        {
            try
            {
                return await ExecutionService.Execute(() => _messageRepository.GetMessageUpdates(myId, companionId));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Chat>> GetChatsUpdates(int id)
        {
            try
            {
                return await ExecutionService.Execute(() => _messageRepository.GetChatsUpdates(id));
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Chat>> GetUserChats(int id)
        {
            return await ExecutionService.Execute(() => _messageRepository.GetUserChats(id));
        }

        public async Task<List<MessageDto>> GetMessages(int senderId, int receiverId)
        {
            return await ExecutionService.Execute(() => _messageRepository.GetMessages(senderId, receiverId));
        }

        public async Task<int> CreateMessage(MessageApiModel messageApi)
        {
            return await ExecutionService.Execute(() => _messageRepository.CreateMessage(messageApi));
        }

        public async Task<int> UpdateMessage(MessageApiModel messageApi)
        {
            return await ExecutionService.Execute(() => _messageRepository.UpdateMessage(messageApi));
        }

        public async Task<int> DeleteMessage(int id)
        {
            return await ExecutionService.Execute(() => _messageRepository.DeleteMessage(id));
        }

    }
}