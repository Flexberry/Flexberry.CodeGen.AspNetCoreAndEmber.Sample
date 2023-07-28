namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.Services;
    using Unity;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using static ICSSoft.Services.CurrentUserService;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Пример основного класса консольного приложения.
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

            // Регистрация контейнера.
            UnityContainer container = ConfigureContainer(config);

            // Получение тестовых данных (не входит в генерацию).
            IDataLoader loader = container.Resolve<IDataLoader>();
            loader.GetData();
        }

        /// <summary>
        /// Пример создания и настройки UnityContainer.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Контейнер.</returns>
        private static UnityContainer ConfigureContainer(IConfiguration configuration)
        {
            UnityContainer container = new UnityContainer();

            container.RegisterType<IUser, CurrentUser>();

            // Регистрация DataLoader (не входит в генерацию).
            container.RegisterType<IDataLoader, DataLoader>();

            RegisterORM(container, configuration);

            return container;
        }

        /// <summary>
        /// Пример регистрации реализации комопнентов ОРМ.
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
