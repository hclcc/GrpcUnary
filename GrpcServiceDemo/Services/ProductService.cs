using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using GrpcServiceDemo;


namespace GrpcServiceDemo.Services
{
    public class ProductService : Product.ProductBase
    {
        public override Task<ProductSaveResponse> SaveProcuct(ProductModel request, ServerCallContext context)
        {
            Console.WriteLine($"{request.ProductCode} {request.ProductName}");

            //Insert to DataBase
            var result = new ProductSaveResponse
            {
                StatusCode = 200,
                IsSuccess = true
            };

            return Task.FromResult(result);
        }
        public override Task<ProductList> GetProducts(Empty request, ServerCallContext context)
        {
            var stockDate = DateTime.SpecifyKind(new DateTime(2022, 2, 1), DateTimeKind.Utc);
            var product1 = new ProductModel
            {
                ProductName = "Sierra",
                ProductCode = "X",
                Price = 10000,
                Brand = "Ford",
                StockDate = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(2022, 2, 1), DateTimeKind.Utc))
            };
            var product2 = new ProductModel
            {
                ProductName = "celica",
                ProductCode = "dd",
                Price = 10000,
                Brand = "Toyota",
                StockDate = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(2021, 2, 1), DateTimeKind.Utc))
            };

            var result = new ProductList();
            result.Products.Add(product1);
            result.Products.Add(product2);

            return Task.FromResult(result);
        }
    }
}
