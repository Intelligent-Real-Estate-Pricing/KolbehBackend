using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.NotifHubs
{
    public class ConnectionMappingService : IConnectionMappingService
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

        public void Add(string userId, string connectionId)
        {
            var connections = _connections.GetOrAdd(userId, _ => new HashSet<string>());
            lock (connections)
            {
                connections.Add(connectionId);
            }
        }

        public void Remove(string userId, string connectionId)
        {
            if (_connections.TryGetValue(userId, out var connections))
            {
                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                        _connections.TryRemove(userId, out _);
                }
            }
        }

        public List<string> GetConnections(string userId)
        {
            return _connections.TryGetValue(userId, out var connections)
                ? connections.ToList()
                : new List<string>();
        }
    }

}
