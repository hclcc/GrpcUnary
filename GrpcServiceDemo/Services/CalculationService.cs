using Grpc.Core;
using Grpc.Net.Client;

using GrpcMultiplyService;

using Microsoft.AspNetCore.Authorization;

namespace GrpcServiceDemo.Services
{
    public class CalculationService: Calculation.CalculationBase
    {
        [Authorize(Roles ="Administrator")]
        public override async Task<CalculationResult> Add(InputNumbers request, ServerCallContext context)
        {
            return new CalculationResult { Result = request.Number1 + request.Number2 };
        }
        [Authorize(Roles = "Administrator,User")]
        public override async Task<CalculationResult> Multiply(InputNumbers request, ServerCallContext context)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:8000");
            var multiplyClient=new Multiply.MultiplyClient(channel);
            var multiplyResponse = await multiplyClient.MultiplyAsync(new GrpcMultiplyService.InputNumbers
                { 
                    Number1 = request.Number1, 
                    Number2 = request.Number2 
                }
            , deadline:context.Deadline);
            await channel.ShutdownAsync();

            return new CalculationResult { Result= multiplyResponse .Result};
        }
        [AllowAnonymous]
        public override Task<CalculationResult> Subtract(InputNumbers request, ServerCallContext context)
        {
            return Task.FromResult(new CalculationResult { Result = request.Number1 - request.Number2 });
        }
    }
}
