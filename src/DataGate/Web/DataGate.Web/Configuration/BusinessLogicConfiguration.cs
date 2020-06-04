﻿namespace DataGate.Web.Configuration
{
    using DataGate.Services.Data;
    using DataGate.Services.Data.Agreements;
    using DataGate.Services.Data.Documents;
    using DataGate.Services.Data.Documents.Contracts;
    using DataGate.Services.Data.Entities;
    using DataGate.Services.Data.Files.Contracts;
    using DataGate.Services.Data.Funds;
    using DataGate.Services.Data.ShareClasses;
    using DataGate.Services.Data.Storage;
    using DataGate.Services.Data.Storage.Contracts;
    using DataGate.Services.Data.SubFunds;
    using DataGate.Services.Data.Timelines;
    using DataGate.Services.Messaging;
    using DataGate.Services.SqlClient;
    using DataGate.Services.SqlClient.Contracts;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class BusinessLogicConfiguration
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration.GetValue<string>("SendGrid:ApiKey")));
            services.AddTransient<ISettingsService, SettingsService>();

            services.AddTransient<ISqlQueryManager, SqlQueryManager>();
            services.AddTransient<IFileSystemService, FileSystemService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IEntitiesDocumentService, EntitiesDocumentService>();
            services.AddTransient<IEntityService, EntityService>();
            services.AddTransient<IEntityDetailsService, EntityDetailsService>();
            services.AddTransient<ITimelineService, TimelineService>();
            services.AddTransient<IAgreementsService, AgreementsService>();

            // Funds
            services.AddTransient<IFundService, FundService>();
            services.AddTransient<IFundsSelectListService, FundsSelectListService>();
            services.AddTransient<IFundStorageService, FundStorageService>();

            // Sub Funds
            services.AddTransient<ISubFundService, SubFundService>();
            //services.AddTransient<IFundsSelectListService, FundsSelectListService>();
            //services.AddTransient<IFundStorageService, FundStorageService>();

            // Share Classes
            services.AddTransient<IShareClassService, ShareClassService>();
            //services.AddTransient<IFundsSelectListService, FundsSelectListService>();
            //services.AddTransient<IFundStorageService, FundStorageService>();

            return services;
        }
    }
}
