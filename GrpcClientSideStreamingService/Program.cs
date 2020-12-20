using System;
using System.Net.Http;
using Grpc.Core;
using System.Threading.Tasks;
using Grpc.Net.Client;
using System.Linq;
using System.Threading;
using Greet;
namespace GrpcClientSideStreamingService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new GreetStreamService.GreetStreamServiceClient(channel);
            GreetMultiRequest[] data ={
                new GreetMultiRequest() { Name="vijay",DOB="03-07-1987"},
            new GreetMultiRequest() { Name="Rajesh",DOB="22-08-1989"},
            new GreetMultiRequest() { Name="Ajay",DOB="23-08-2001"},
                new GreetMultiRequest() { Name="Mohit",DOB="02-01-1999"}
            };
            var responce = client.GetGreetStreamMethod();

            var responseTask = Task.Run(async () =>
            {
                while (await responce.ResponseStream.MoveNext())
                    Console.WriteLine($"{responce.ResponseStream.Current.Message}");
            });

           
            foreach (var item in data)
            {
                Console.WriteLine("Request Hello {0} Dob {1}",item.Name,item.DOB);
                await responce.RequestStream.WriteAsync(new GreetMultiRequest() { Name = item.Name, DOB = item.DOB });
            }

           await responce.RequestStream.CompleteAsync();
           await responseTask;
            channel.ShutdownAsync().Wait();
           
        }
    }
}
