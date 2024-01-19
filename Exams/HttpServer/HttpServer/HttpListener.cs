using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpServer
{
    public class HttpListener : IHttpListener
    {
        private readonly Dictionary<string, HttpRequest> pendingRequests = new Dictionary<string, HttpRequest>();
        private readonly Queue<HttpRequest> requestQueue = new Queue<HttpRequest>();
        private readonly HashSet<string> priorityRequests = new HashSet<string>();
        private readonly List<HttpRequest> executedRequests = new List<HttpRequest>();

        public void AddRequest(HttpRequest request)
        {
            pendingRequests.Add(request.Id, request);
            requestQueue.Enqueue(request);
        }

        public void AddPriorityRequest(HttpRequest request)
        {
            if (!priorityRequests.Add(request.Id))
            {
                throw new ArgumentException();
            }

            AddRequest(request);
        }

        public bool Contains(string requestId)
        {
            return pendingRequests.ContainsKey(requestId);
        }

        public int Size()
        {
            return pendingRequests.Count;
        }

        public HttpRequest GetRequest(string requestId)
        {
            if (!pendingRequests.TryGetValue(requestId, out var request))
            {
                throw new ArgumentException();
            }

            return request;
        }

        public void CancelRequest(string requestId)
        {
            if (!pendingRequests.ContainsKey(requestId))
            {
                throw new ArgumentException();
            }

            pendingRequests.Remove(requestId);
            priorityRequests.Remove(requestId);
        }

        public HttpRequest Execute()
        {
            if (requestQueue.Count == 0)
            {
                throw new ArgumentException();
            }

            var executedRequest = requestQueue.Dequeue();
            executedRequests.Add(executedRequest);
            pendingRequests.Remove(executedRequest.Id);
            priorityRequests.Remove(executedRequest.Id);

            return executedRequest;
        }

        public HttpRequest RescheduleRequest(string requestId)
        {
            if (!executedRequests.Any(r => r.Id == requestId))
            {
                throw new ArgumentException();
            }

            var executedRequest = executedRequests.First(r => r.Id == requestId);
            AddRequest(executedRequest);

            return executedRequest;
        }

        public IEnumerable<HttpRequest> GetByHost(string host)
        {
            var byHost = pendingRequests.Values.Concat(executedRequests)
                .Where(r => r.Host == host)
                .OrderBy(r => r.Id);

            return byHost.ToList(); 
        }

        public IEnumerable<HttpRequest> GetAllExecutedRequests()
        {
            return executedRequests.ToList(); 
        }
    }
}
