using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.AdvanceSearch;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Application.Abstractions.Services.DimensionSetup;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Abstractions.Services.InvoiceInquiry;
using CbsAp.Application.Abstractions.Services.Invoicing;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Application.Abstractions.Services.Supplier;
using CbsAp.Application.Abstractions.Services.TaxCode;
using CbsAp.Application.Services.AdvanceSearch;
using CbsAp.Application.Services.DimensionSetup;
using CbsAp.Application.Services.Entity;
using CbsAp.Application.Services.InvoiceInquiry;
using CbsAp.Application.Services.Invoicing;
using CbsAp.Application.Services.Shared;
using CbsAp.Application.Services.Supplier;
using CbsAp.Application.Services.TaxCodes;
using CbsAp.Application.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace CbsAp.Application.Services
{
    public static class CbsServicesCollection
    {
        public static IServiceCollection AddServiceContainer(this IServiceCollection services)
        {
            services.AddSingleton<IMjmlService, MjmlService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAuthService, UserAuthenticationService>();
            services.AddScoped<IEntityService, EntityService>();
            services.AddScoped<ISupplierService, SupplierService>();

            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IInvRoutingFlowService, InvRoutingFlowService>();

            services.AddScoped<ITaxcodeService, TaxCodeService>();
            services.AddScoped<IInvoiceInquiryService, InvoiceInquiryService>();
            services.AddScoped<IAdvanceSearchService, AdvanceSearchService>();

            services.AddScoped<IDimensionSetupService, DimensionSetupService>();


            return services;
        }
    }
}
