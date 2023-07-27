namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.UserDataTypes;
    using System;
    using static ICSSoft.Services.CurrentUserService;
    using ICSSoft.STORMNET;

    /// <summary>
    /// Класс загрузки данных с DataService и User.
    /// </summary>
    public class DataLoader : IDataLoader
    {
        private readonly IDataService dataService;
        private readonly IUser user;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="DataLoader"/>.
        /// </summary>
        /// <param name="dataService">DataService.</param>
        /// <param name="user">Current User.</param>
        public DataLoader(IDataService dataService, IUser user)
        {
            this.dataService = dataService;
            this.user = user;
        }

        /// <summary>
        /// Получает данные.
        /// </summary>
        /// <returns>Данные.</returns>
        public DataObject[] GetData()
        {
            ICSSoft.STORMNET.View view = new ICSSoft.STORMNET.View();

            Type loadTypeContacts = typeof(Contact);
            view.DefineClassType = loadTypeContacts;
            LoadingCustomizationStruct lcs = LoadingCustomizationStruct.GetSimpleStruct(loadTypeContacts, view);
            DataObject[] contacts = this.dataService.LoadObjects(view);

            return contacts;
        }
    }
}
