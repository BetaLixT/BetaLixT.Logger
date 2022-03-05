using BetaLixT.Logger.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BetaLixT.Logger.Repositories
{
    public interface ILogRepository
    {
        public Task AddMessagesAsync(IList<LogMessage> messages);
    }
}
