namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using static ICSSoft.Services.CurrentUserService;

    /// <summary>
    /// Пример реализации фонового сервиса с DataService и User. НЕ ВКЛЮЧЕН В ГЕНЕРАЦИЮ.
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly IDataService dataService;
        private readonly IUser user;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="dataService">DataService.</param>
        /// <param name="user">Current User.</param>
        public Worker(IDataService dataService, IUser user)
        {
            this.dataService = dataService;
            this.user = user;
        }

        /// <summary>
        /// Асинхронный процесс фонового сервиса.
        /// </summary>
        /// <param name="stoppingToken">Токен, отвечающий за прекращение процесса.</param>
        /// <returns>Асинхронные методы, которые не возвращают значений должны возвращать Task.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string data = LoadExampleDataAsString();
            string message = "Сервис запущен. Загружены тестовые данные " + data;
            LogService.LogInfo(message);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Логика фонового сервиса. Будет срабатывать раз в секунду.
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Загрузка данных из БД.
        /// </summary>
        /// <returns>Возвращает список имен объектов Contacts в виде строки.</returns>
        private string LoadExampleDataAsString()
        {
            try
            {
                View view = new View();
                view.DefineClassType = typeof(Contact);
                view.AddProperty("Name");
                var lcs = LoadingCustomizationStruct.GetSimpleStruct(typeof(Contact), view);
                ObjectStringDataView[] resultList = dataService.LoadStringedObjectView(',', lcs);

                List<string> contactsNames = new List<string>();

                foreach (ObjectStringDataView contactObject in resultList)
                {
                    contactsNames.Add(contactObject.Data);
                }

                string resultMessage = string.Join(", ", contactsNames.ToArray()) + " for User " + user.Login;

                return resultMessage;
            }
            catch (Exception ex)
            {
                string errorMessage = "Contacts names read error: " + ex;
                LogService.LogError(errorMessage);

                return errorMessage;
            }
        }
    }
}