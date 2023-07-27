namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.Services;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using Unity;
    using Unity.Microsoft.DependencyInjection;
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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            UnityContainer container = ConfigureContainer(configuration);

            CreateHost(args, configuration, container).Run();
        }

        /// <summary>
        /// ������� ������������� ����������.
        /// </summary>
        /// <param name="args">��������� �������.</param>
        /// <param name="configuration">������� ������������ ����������.</param>
        /// <param name="container">������� ��������� ����������.</param>
        /// <returns>������������� ����������.</returns>
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
                services.AddHostedService<Worker>();
            })
            .Build();

        /// <summary>
        /// ������� � ��������� UnityContainer.
        /// </summary>
        /// <param name="configuration">������������ ����������.</param>
        /// <returns>���������.</returns>
        private static UnityContainer ConfigureContainer(IConfigurationRoot configuration)
        {
            UnityContainer container = new UnityContainer();

            container.RegisterType<IUser, CurrentUser>();

            RegisterORM(container, configuration);

            return container;
        }

        /// <summary>
        /// ����������� ���������� ����������� ���.
        /// </summary>
        /// <param name="container">��������� ��� �����������.</param>
        /// <param name="configuration">������������ ����������.</param>
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
