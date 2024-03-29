namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            UnityContainer container = ConfigureContainer(configuration);

            CreateHost(args, configuration, container).Run();
        }

        /// <summary>
        /// Создать инициализатор приложения.
        /// </summary>
        /// <param name="args">Аргументы запуска.</param>
        /// <param name="configuration">Текущая конфигурация приложения.</param>
        /// <param name="container">Текущий контейнер приложения.</param>
        /// <returns>Инициализатор приложения.</returns>
        public static IHost CreateHost(string[] args, IConfigurationRoot configuration, UnityContainer container) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder =>
            {
                builder.Sources.Clear();
                builder.AddConfiguration(configuration);
            })
            .UseUnityServiceProvider(container)
            .ConfigureServices(services =>
            {
                // Добавляем тестовый сервис. НЕ ВКЛЮЧЕНО В ГЕНЕРАЦИЮ.
                services.AddHostedService<Worker>();
            })
            .Build();

        /// <summary>
        /// Создать и настроить UnityContainer.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Контейнер.</returns>
        private static UnityContainer ConfigureContainer(IConfigurationRoot configuration)
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
        private static void RegisterORM(IUnityContainer container, IConfigurationRoot configuration)
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
