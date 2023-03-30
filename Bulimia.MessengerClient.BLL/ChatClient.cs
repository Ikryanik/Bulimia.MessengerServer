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

        public async Task<List<Chat>> GetChatsUpdates(int id)
        {
            try
            {
                return await _messageRepository.GetChatsUpdates(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Chat>> GetUserChats(int id)
        {
            return await _messageRepository.GetUserChats(id);
        }

        public async Task<List<MessageDto>> GetMessages(int senderId, int receiverId)
        {
            return await _messageRepository.GetMessages(senderId, receiverId);
        }

        public async Task<int> CreateMessage(MessageModel message)
        {
            return await _messageRepository.CreateMessage(message);
        }

        public async Task<int> UpdateMessage(MessageModel message)
        {
            return await _messageRepository.UpdateMessage(message);
        }

        public async Task<int> DeleteMessage(int id)
        {
            return await _messageRepository.DeleteMessage(id);
        }

        public async Task<List<MessageDto>> GetMessagesUpdates(int myId, int companionId)
        {
            try
            {
                return await _messageRepository.GetMessageUpdates(myId, companionId);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}