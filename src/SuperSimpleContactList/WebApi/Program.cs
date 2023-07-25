namespace NewPlatform.SuperSimpleContactList
{
    using Microsoft.AspNetCore.Builder;
    using Unity;
    using Unity.Microsoft.DependencyInjection;
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using static ICSSoft.Services.CurrentUserService;

    /// <summary>
    /// �������� ����� ����������.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// ����� ����� � ����������.
        /// </summary>
        /// <param name="args">��������� �������.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            UnityContainer container = ConfigureContainer(configuration);
            builder.Host.UseUnityServiceProvider(container);

            builder.Services.AddMvcCore();
            builder.Services.AddHealthChecks().AddNpgSql(configuration["DefConnStr"]);
            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyMethod()
                       .AllowCredentials()
                       .AllowAnyHeader();
            }));

            var app = builder.Build();
            app.MapControllers();
            app.MapHealthChecks("/healthz");
            app.Run();
        }

        /// <summary>
        /// ������� � ��������� UnityContainer.
        /// </summary>
        /// <param name="configuration">������������ ����������.</param>
        /// <returns>���������.</returns>
        private static UnityContainer ConfigureContainer(ConfigurationManager configuration)
        {
            UnityContainer container = new UnityContainer();

            container.RegisterSingleton<IUser, CurrentUser>();

            RegisterORM(container, configuration);

            return container;
        }

        /// <summary>
        /// ����������� ���������� ����������� ���.
        /// </summary>
        /// <param name="container">��������� ��� �����������.</param>
        /// <param name="configuration">������������ ����������.</param>
        private static void RegisterORM(IUnityContainer container, ConfigurationManager configuration)
        {
            string connStr = configuration["DefConnStr"];
            if (string.IsNullOrEmpty(connStr))
            {
                throw new System.Configuration.ConfigurationErrorsException("DefConnStr is not specified in Configuration or enviromnent variables.");
            }

            container.RegisterSingleton<ISecurityManager, EmptySecurityManager>();
            container.RegisterSingleton<IDataService, PostgresDataService>(
                Inject.Property(nameof(PostgresDataService.CustomizationString), connStr));
        }
    }
}