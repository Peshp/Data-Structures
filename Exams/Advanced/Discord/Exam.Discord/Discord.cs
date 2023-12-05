using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.Discord
{
    public class Discord : IDiscord
    {
        private HashSet<Message> _messages = new HashSet<Message>();

        public int Count => _messages.Count;

        public bool Contains(Message message)
            => _messages.Contains(message);

        public void DeleteMessage(string messageId)
        {
            var curr = _messages.FirstOrDefault(m => m.Id == messageId);

            if (curr == null)
                throw new ArgumentException();

            _messages.Remove(curr);
        }

        public IEnumerable<Message> GetAllMessagesOrderedByCountOfReactionsThenByTimestampThenByLengthOfContent()
            => _messages.OrderByDescending(m => m.Reactions.Count)
                .ThenBy(m => m.Timestamp)
                .ThenBy(m => m.Content.Length);

        public IEnumerable<Message> GetChannelMessages(string channel)
        {
            var curr = _messages.Where(m => m.Channel == channel);

            if (!curr.Any())
                throw new ArgumentException();

            return curr;
        }

        public Message GetMessage(string messageId)
        {
            var curr = _messages.FirstOrDefault(m => m.Id == messageId);

            if (curr == null)
                throw new ArgumentException();

            return curr;
        }

        public IEnumerable<Message> GetMessageInTimeRange(int lowerBound, int upperBound)
            => _messages.Where(m => m.Timestamp >= lowerBound && m.Timestamp <= upperBound)
                .OrderByDescending(m => _messages.Count(msg => msg.Channel == m.Channel));

        public IEnumerable<Message> GetMessagesByReactions(List<string> reactions)
            => _messages.Where(m => reactions.All(r => m.Reactions.Contains(r)))
                .OrderByDescending(m => m.Reactions.Count)
                .OrderBy(m => m.Timestamp);

        public IEnumerable<Message> GetTop3MostReactedMessages()
         => _messages.OrderByDescending(m => m.Reactions.Count).Take(3);

        public void ReactToMessage(string messageId, string reaction)
        {
            var curr = _messages.FirstOrDefault(m => m.Id == messageId);

            if (curr == null)
                throw new ArgumentException();

            curr.Reactions.Add(reaction);
        }

        public void SendMessage(Message message)
        {
            _messages.Add(message);
        }
    }
}
