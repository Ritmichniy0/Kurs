using Autofac.Extensions.DependencyInjection;
using GBLesson3GraphQL.Abstraction;
using GBLesson3GraphQL.GraphServises.Mutation;
using GBLesson3GraphQL.GraphServises.Query;
using GBLesson3GraphQL.Mapping;
using GBLesson3GraphQL.Repo;

namespace GBLesson3GraphQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddSingleton<ProductRepo>()
                            .AddSingleton<ProductGroupRepo>()
                            .AddGraphQLServer().AddQueryType<Query>()
                            .AddMutationType<Mutation>();
            
            builder.Services.AddSingleton<IProductRepo, ProductRepo>();
            builder.Services.AddSingleton<IProductGroupRepo, ProductGroupRepo>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGraphQL();

            app.Run();
        }
    }
}
