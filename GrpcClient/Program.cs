using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcServiceDemo;

using System.Xml.Serialization;

namespace GrpcClient {

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            try
            {

                var authenticationClient = new Authentication.AuthenticationClient(channel);
                var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
                {
                    UserName = "admin",
                    Password = "admin"
                });

                Console.WriteLine($"Received Auth Response | Token: {authenticationResponse.AccessToken} | Expires In: {authenticationResponse.ExpiresIn}");

                var calculationClient = new Calculation.CalculationClient(channel);
                var headers = new Metadata();
                headers.Add("Authorization", $"Bearer {authenticationResponse.AccessToken}");


                var sumResult = calculationClient.Add(new InputNumbers { Number1 = 5, Number2 = 10 }, headers);
                Console.WriteLine($"Sum Result: 5+10={sumResult.Result}");

                var subtractResult = calculationClient.Subtract(new InputNumbers { Number1 = 20, Number2 = 5 });
                Console.WriteLine($"Subtract result: 20-5={subtractResult.Result}");


                var multiplyResult = calculationClient.Multiply(new InputNumbers { Number1 = 5, Number2 = 6 }, headers);
                Console.WriteLine($"Multiply Result: 5*6={multiplyResult.Result}");
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Status Code: {ex.StatusCode} | Error: {ex.Message}");
                return;
            }

            await channel.ShutdownAsync();
        }
        public static async Task Main2(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Product.ProductClient(channel);

            var stockDate =DateTime.SpecifyKind( new DateTime(2022, 2, 1),  DateTimeKind.Utc);
            //var response = client.GetFullName(new SampleRequest { Firstname = "Manel", Lastname = "Silva" });
            var response = await client.SaveProcuctAsync(new ProductModel { 
                ProductCode = "Manel", 
                ProductName = "Silva", 
                Price=100, 
                Brand="Ford",
                StockDate=Timestamp.FromDateTime(stockDate)
            });


            var response1 = await client.GetProductsAsync(new Google.Protobuf.WellKnownTypes.Empty());
            foreach (var item in response1.Products)
            {
                Console.WriteLine(item);
            }

            await channel.ShutdownAsync();


            Console.ReadKey();
        }
        public static async Task Main1(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Sample.SampleClient(channel);

            //var response = client.GetFullName(new SampleRequest { Firstname = "Manel", Lastname = "Silva" });
            var response =await  client.GetFullNameAsync(new SampleRequest { Firstname = "Manel", Lastname = "Silva" });

            Console.WriteLine(response);
            await channel.ShutdownAsync();
            Console.ReadKey();
        }
    }

}