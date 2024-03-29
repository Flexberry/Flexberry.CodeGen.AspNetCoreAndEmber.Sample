namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using Microsoft.AspNetCore.Builder;
    using Unity;
    using Unity.Microsoft.DependencyInjection;
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

            string allowedHosts = configuration["AllowedHosts"];

            if (string.IsNullOrEmpty(allowedHosts))
            {
                throw new System.Configuration.ConfigurationErrorsException("AllowedHosts is not specified in Configuration or enviromnent variables.");
            }

            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins(allowedHosts)
                       .AllowAnyMethod()
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

            container.RegisterType<IUser, CurrentUser>();

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

            container.RegisterSingleton<ISecurityManager, EmptySecurityManager>();
            container.RegisterSingleton<IDataService, PostgresDataService>(
                Inject.Property(nameof(PostgresDataService.CustomizationString), connStr));
        }
    }
}