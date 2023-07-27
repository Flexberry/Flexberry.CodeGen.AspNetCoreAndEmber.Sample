namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.STORMNET;

    /// <summary>
    /// Интерфейс для получения данных.
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// Метод возвращает данные.
        /// </summary>
        /// <returns>Данные.</returns>
        DataObject[] GetData();
    }
}
