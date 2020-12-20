using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServerSideStreamingService
{
    public class GreeterService :GreetStreamService.GreetStreamServiceBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task GetGreetStreamMethod(IAsyncStreamReader<GreetMultiRequest> requestStream, IServerStreamWriter<GreetMultiResponce> responseStream, ServerCallContext context)
        {           

            while (await requestStream.MoveNext()) {
               
                string input = string.Format("Hello {0} DOB {1}", requestStream.Current.Name, requestStream.Current.DOB);
                Console.WriteLine("Request Recived  {0}",input);
                await responseStream.WriteAsync(new GreetMultiResponce() { Message = input });
            }
            
        }
    }
}
