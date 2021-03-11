namespace NewPlatform.SuperSimpleContactList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class DataObjectFacts
    {
        /// <summary>
        /// Получить классы объектов данных,
        /// которые не требуется вычитывать из БД.
        /// </summary>
        /// <returns>Список классов, которые не требуется вычитывать из БД.</returns>
        private partial IEnumerable<Type> GetNotSelectableTypes()
        {
            return Enumerable.Empty<Type>();
        }
    }
}
