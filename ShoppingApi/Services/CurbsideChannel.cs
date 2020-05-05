using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;



namespace ShoppingApi.Services
{
    public class CurbsideChannel
    {
        private const int MaxMessagesInChannel = 1000;
        private readonly Channel<CurbsideChannelRequest> TheChannel;
        private readonly ILogger<CurbsideChannel> Logger;



        public CurbsideChannel(ILogger<CurbsideChannel> logger)
        {
            Logger = logger;
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleReader = true,
                SingleWriter = false
            };



            TheChannel = Channel.CreateBounded<CurbsideChannelRequest>(options);
        }
        public async Task<bool> AddCurbsideOrder(CurbsideChannelRequest order, CancellationToken ct = default)
        {
            while (await TheChannel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                if (TheChannel.Writer.TryWrite(order))
                {
                    return true;
                }
            }



            return false;
        }



        public IAsyncEnumerable<CurbsideChannelRequest> ReadAllAsync(CancellationToken ct = default)
        {
            return TheChannel.Reader.ReadAllAsync(ct);
        }
    }
}