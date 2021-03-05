/*
 * В этом файле можно настраивать поведение тестов.
 */
namespace NewPlatform.SuperSimpleContactList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;

    public partial class DataObjectFacts
    {

        /// <summary>
        /// Проверять ли, что во всех объектах данных AcessType=@this.
        /// </summary>
        private bool CheckAccessTypeThis { get; } = false;

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
        /// Получить имена свойств объектов данных с ValueType-типом,
        /// для которых намеренно не задан атрибут <see cref="NotNullAttribute"/>.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, для которых намеренно не задан атрибут <see cref="NotNullAttribute"/>.</returns>
        private partial Dictionary<Type, string[]> GetPropertyWithoutNotNull()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// у которых геттер намеренно генерирует исключение, если объект недозагружен.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, у которых геттер намеренно генерирует исключение, если объект недозагружен.</returns>
        private partial Dictionary<Type, string[]> GetPropertyWithoutGetterCheck()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить имена свойств объектов данных,
        /// у которых сеттер намеренно генерирует исключение, если объект недозагружен.
        /// </summary>
        /// <returns>Словарь {Тип, массив имен свойств}, у которых сеттер намеренно генерирует исключение, если объект недозагружен.</returns>
        private partial Dictionary<Type, string[]> GetPropertyWithoutSetterCheck()
        {
            return new Dictionary<Type, string[]>();
        }

        /// <summary>
        /// Получить классы объектов данных, 
        /// в которых имеются намеренно некорректные представления.
        /// </summary>
        /// <returns>Список классов с намеренно некорректными представлениями.</returns>
        private partial IEnumerable<Type> GetTypesWithoutValidViews()
        {
            return Enumerable.Empty<Type>();
        }

        /// <summary>
        /// Получить классы объектов данных,
        /// в которых не задан AcessType=@this.
        /// </summary>
        /// <returns>Список классов, в которых не задан AcessType=@this.</returns>
        private partial IEnumerable<Type> GetDataObjectsWithoutAccessType()
        {
            if (CheckAccessTypeThis)
            {
                return Enumerable.Empty<Type>();
            }
            else
            {
                return GetStoredDataObjects();
            }
        }
    }
}
