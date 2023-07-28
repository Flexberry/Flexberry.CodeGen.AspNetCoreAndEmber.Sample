namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.STORMNET;

    /// <summary>
    /// Пример интерфейса для получения данных.
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// Пример метода возвращения данных.
        /// </summary>
        /// <returns>Данные.</returns>
        DataObject[] GetData();
    }
}
