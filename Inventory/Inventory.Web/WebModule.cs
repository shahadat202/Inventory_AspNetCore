using Autofac;
using Inventory.Application.Services;
using Inventory.Domain.RepositoryContracts;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repositories;
namespace Inventory.Web
{
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManagementService>()
                .As<IProductManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InventoryDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>()
                .As<IProductRepository>()
                .InstancePerLifetimeScope();
        }
    }
}