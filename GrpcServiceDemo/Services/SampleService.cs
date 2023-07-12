using Grpc.Core;

using GrpcServiceDemo;

namespace GrpcServiceDemo.Services
{
    public class SampleService: Sample.SampleBase
    {
        public override Task<SampleResponse> GetFullName (SampleRequest request, ServerCallContext context)
        {
            var result = $"{request.Firstname} {request.Lastname}";
            return Task.FromResult( new SampleResponse { Fullname = result });
        }

    }
}
