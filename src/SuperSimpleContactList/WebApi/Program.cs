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
    /// Основной класс приложения.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы запуска.</param>
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
        /// Создать и настроить UnityContainer.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Контейнер.</returns>
        private static UnityContainer ConfigureContainer(ConfigurationManager configuration)
        {
            UnityContainer container = new UnityContainer();

            container.RegisterSingleton<IUser, CurrentUser>();

            RegisterORM(container, configuration);

            return container;
        }

        /// <summary>
        /// Регистрация реализации комопнентов ОРМ.
        /// </summary>
        /// <param name="container">Контейнер для регистрации.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        private static void RegisterORM(IUnityContainer container, ConfigurationManager configuration)
        {
            string connStr = configuration["DefConnStr"];
            if (string.IsNullOrEmpty(connStr))
            {
                throw new System.Configuration.ConfigurationErrorsException("DefConnStr is not specified in Configuration or enviromnent variables.");
            }

            // Register SecurityManager.
            ISecurityManager securityManager = new EmptySecurityManager();
            container.RegisterInstance<ISecurityManager>(securityManager, InstanceLifetime.Singleton);

            // Register DataService.
            IDataService dataService = new PostgresDataService(securityManager)
            {
                CustomizationString = connStr
            };

            container.RegisterInstance<IDataService>(dataService, InstanceLifetime.Singleton);
        }
    }
}