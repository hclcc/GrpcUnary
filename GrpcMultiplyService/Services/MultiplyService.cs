using Grpc.Core;

namespace GrpcMultiplyService.Services
{
    public class MultiplyService : Multiply.MultiplyBase
    {

        public override async Task<CalculationResult> Multiply(InputNumbers request, ServerCallContext context)
        {
            await Task.Delay(10000);
            return new CalculationResult { Result = request.Number1 * request.Number2 };
        }
    }
}
