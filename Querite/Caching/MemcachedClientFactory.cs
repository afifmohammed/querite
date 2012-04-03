using System.Collections.Generic;
using Membase;

namespace querite
{
    public class MembaseClientFactory
    {
        private readonly IDictionary<string, MembaseClient> _clients = new Dictionary<string, MembaseClient>();

        public MembaseClient Build(string bucketName, string password)
        {
            MembaseClient client;
            if(!_clients.TryGetValue(bucketName, out client))
            {
                _clients.Add(bucketName, new MembaseClient(bucketName, password));
            }

            return client;
        }
    }
}