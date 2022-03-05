using BetaLixT.Logger.Entities;
using BetaLixT.Logger.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace BetaLixT.Logger
{
    internal class LogProcessor : IDisposable
    {
        // private const int _maxQueuedMessages = 2048;

        private readonly ILogRepository _logRepository;
        private readonly BufferBlock<LogMessage> _messageBuffer = new BufferBlock<LogMessage>();
        private readonly Thread _outputThread;

        public LogProcessor(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
            _outputThread = new Thread(() => ProcessLogQueueAsync().Wait())
            {
                IsBackground = true,
                Name = "Log queue processing thread"
            };
            _outputThread.Start();
        }

        public void EnqueueMessage(LogMessage message)
        {
            this._messageBuffer.Post(message);
        }

        public void Dispose()
        {
            this._messageBuffer.Complete();
        }

        private async Task ProcessLogQueueAsync()
        {
            try
            {
                while(await this._messageBuffer.OutputAvailableAsync())
                {
                    if(this._messageBuffer.TryReceiveAll(out var messages))
                    {
                        try
                        {
                            await this._logRepository.AddMessagesAsync(messages);
                        }
                        catch(Exception e)
                        {
                            Console.Error.WriteLine(e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                try
                {
                    this._messageBuffer.Complete();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
    }
}
