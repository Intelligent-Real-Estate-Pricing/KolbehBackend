using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.NotifHubs
{
    public interface IConnectionMappingService
    {
        void Add(string userId, string connectionId);
        void Remove(string userId, string connectionId);
        List<string> GetConnections(string userId);
    }

}
