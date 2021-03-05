/*
 * В этом файле можно настраивать поведение тестов.
 */
namespace NewPlatform.SuperSimpleContactList
{
    using System;
    using System.Collections.Generic;
    using ICSSoft.STORMNET.Business;

    public partial class DataObjectFacts
    {

        /// <summary>
        /// Получить используемый приложением тип сервиса данных.
        /// </summary>
        /// <returns>Тип сервиса данных.</returns>
        private partial Type GetDataServiceType()
        {
            return typeof(PostgresDataService);
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// для которых намеренно не задано DataServiceExpression.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых не задано DataServiceExpression.</returns>
        private partial Dictionary<Type, string[]> GetPropertyWithoutDataServiceExpression()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// для которых намеренно не задано DataServiceExpression.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых не задано DataServiceExpression.</returns>
        private partial Dictionary<Type, string[]> GetPropertyWithoutNotNull()
        {
            return new Dictionary<Type, string[]>();
        }
    }
}
