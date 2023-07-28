namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using static ICSSoft.Services.CurrentUserService;

    /// <summary>
    /// ������ ���������� �������� ������� � DataService � User. �� ������� � ���������.
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
        /// ����������� ������� �������� �������.
        /// </summary>
        /// <param name="stoppingToken">�����, ���������� �� ����������� ��������.</param>
        /// <returns>����������� ������, ������� �� ���������� �������� ������ ���������� Task.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string data = LoadExampleDataAsString();
            string message = "������ �������. ��������� �������� ������ " + data;
            LogService.LogInfo(message);

            while (!stoppingToken.IsCancellationRequested)
            {
                // ������ �������� �������. ����� ����������� ��� � �������.
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// �������� ������ �� ��.
        /// </summary>
        /// <returns>���������� ������ ���� �������� Contacts � ���� ������.</returns>
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