using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Infrastracture.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CbsAp.Infrastracture.Extensions
{
    public static class RepositoryResources
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IUnitofWork), typeof(UnitofWork))
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IUserManagementRepository), typeof(UserManagementRepository))
                .AddScoped(typeof(IRoleManagementRepository), typeof(RoleManagmentRepository))
                .AddScoped(typeof(INoticeRepository), typeof(NoticeRepository))
                .AddScoped(typeof(IPermissionManagementRepository), typeof(PermissionManagementRepository))
                .AddScoped(typeof(IEntityRepository), typeof(EntityProfileRepository))
                .AddScoped(typeof(IDimensionRepository), typeof(DimensionRepository))
                .AddScoped(typeof(ITaxCodeRepository), typeof(TaxCodeRepository))
                .AddScoped(typeof(IMenuRepository), typeof(MenuRepository))
                .AddScoped(typeof(ISupplierRepository), typeof(SupplierRepository))
                .AddScoped(typeof(IInvRoutingFlowRepository), typeof(InvRoutingFlowRepository))
                .AddScoped(typeof(IInvoiceRepository), typeof(InvoiceRepository))
                .AddScoped(typeof(IPurchaseOrderRepository), typeof(PurchaseOrderRepository))
                .AddScoped(typeof(IAccountRepository), typeof(AccountRepository))
                .AddScoped(typeof(IGoodsReceiptRepository), typeof(GoodsReceiptRepository))

                .AddScoped<ISystemVariableRepository, SystemVariableRepository>();
        }
    }
}