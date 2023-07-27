namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.Services;
    using Unity;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using static ICSSoft.Services.CurrentUserService;
    using Microsoft.Extensions.Configuration;

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
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();
            UnityContainer container = ConfigureContainer(config);

            IDataService dataService = container.Resolve<IDataService>();
            IUser user = container.Resolve<IUser>();
            DataLoader dataLoader = new DataLoader(dataService, user);

            IDataLoader loader = container.Resolve<IDataLoader>();

            loader.GetData();
        }

        /// <summary>
        /// Создать и настроить UnityContainer.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Контейнер.</returns>
        private static UnityContainer ConfigureContainer(IConfiguration configuration)
        {
            UnityContainer container = new UnityContainer();

            container.RegisterType<IUser, CurrentUser>();
            container.RegisterType<IDataLoader, DataLoader>();

            RegisterORM(container, configuration);

            return container;
        }

        /// <summary>
        /// Регистрация реализации комопнентов ОРМ.
        /// </summary>
        /// <param name="container">Контейнер для регистрации.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        private static void RegisterORM(IUnityContainer container, IConfiguration configuration)
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
