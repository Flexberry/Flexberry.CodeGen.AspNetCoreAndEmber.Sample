/*
 * Этот файл генерируется каждый раз при генерации проекта объектов.
 * Не нужно вносить изменения в этот файл вручную.
 * Настроить поведение тестов можно в соседнем файле,
 * который генерируется только при первой генерации.
 */

namespace NewPlatform.SuperSimpleContactList
{
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public partial class DataObjectFacts
    {
        #region Customizations
        private partial Type GetDataServiceType();

        private partial Dictionary<Type, string[]> GetPropertyWithoutDataServiceExpression();

        private partial Dictionary<Type, string[]> GetPropertyWithoutNotNull();

        private partial Dictionary<Type, string[]> GetPropertyWithoutGetterCheck();

        private partial Dictionary<Type, string[]> GetPropertyWithoutSetterCheck();

        private partial IEnumerable<Type> GetTypesWithoutValidViews();
        #endregion

        private IEnumerable<Type> GetStoredDataObjects()
        {
            return Assembly.GetAssembly(typeof(ObjectsMarker))
                .GetExportedTypes()
                .Where(
                    x => x.IsClass
                         && x.IsSubclassOf(typeof(DataObject))
                         && Information.IsStoredType(x))
                .OrderBy(x => x.FullName);
        }

        /// <summary>
        ///     Тест проверяет, что все нехранимые свойства имеют DSE.
        /// </summary>
        [Fact]
        public void TestAllNotStoredPropertiesHaveDataServiceExpression()
        {
            // Arrange.
            var dataServiceType = GetDataServiceType();
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertyWithoutDataServiceExpression();
            var dontCheckPropertyNames = typeof(DataObject).GetProperties()
                .Where(o => !Information.IsStoredProperty(typeof(DataObject), o.Name))
                .Select(o => o.Name);

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var classProperties = cl.GetProperties()
                    .Where(// Не проверяем исключения на имя свойства.
                        o => !dontCheckPropertyNames.Contains(o.Name))
                    .Where(// Не проверяем пары имя свойства + класс.
                        o => !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(o.Name)))
                    .Where(
                        o => !Information.IsStoredProperty(cl, o.Name)
                             && string.IsNullOrEmpty(Information.GetExpressionForProperty(cl, o.Name).GetMostCompatible(dataServiceType)?.ToString())
                             && !o.PropertyType.IsSubclassOf(typeof(DataObject)));
                errors.AddRange(classProperties.Select(prop => $"{cl.FullName}.{prop.Name}"));
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие нехранимые свойства классов не имеют DSE:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что все логические поля имеют атрибут NotNull.
        /// </summary>
        [Fact]
        public void TestAllValueTypeHasNotNullFlag()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertyWithoutNotNull();

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var classProperties = cl.GetProperties();
                foreach (var property in classProperties)
                {
                    var propType = Information.GetPropertyType(cl, property.Name);
                    if (!propType.IsConstructedGenericType
                        && !propType.IsEnum
                        && propType.IsValueType
                        && Information.IsStoredProperty(cl, property.Name)
                        && !Information.GetPropertyNotNull(cl, property.Name)
                        && !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(property.Name)))
                    {
                        errors.Add($@"{cl.FullName}.{property.Name}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие ValueType свойства классов не имеют флага NotNull:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что getter'ы всех свойств не вызывают исключений при пустом объекте.
        /// </summary>
        [Fact]
        public void TestAllGettersAreValid()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertyWithoutGetterCheck();

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var instance = Activator.CreateInstance(cl);
                var classProperties = cl.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.GetMethod != null
                                && x.DeclaringType != typeof(DataObject)
                                && !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(x.Name)))
                    .ToList();
                foreach (var prop in classProperties)
                {
                    try
                    {
                        prop.GetValue(instance, null);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($@"{cl.FullName}.{prop.Name}: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие у следующих свойств getter'ы некорректны:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что setter'ы всех свойств не вызывают исключений при пустом объекте.
        /// </summary>
        [Fact]
        public void TestAllSettersAreValid()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var dontCheckDict = GetPropertyWithoutSetterCheck();

            // Act.
            var errors = new List<string>();
            foreach (var cl in assemblyClasses)
            {
                var instance = Activator.CreateInstance(cl);
                var classProperties = cl.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.SetMethod != null
                                && x.DeclaringType != typeof(DataObject)
                                && !(dontCheckDict.ContainsKey(cl) && dontCheckDict[cl].Contains(x.Name)))
                    .ToList();
                foreach (var prop in classProperties)
                {
                    var val = prop.PropertyType.IsValueType
                        ? Activator.CreateInstance(prop.PropertyType)
                        : null;
                    try
                    {
                        prop.SetValue(instance, val);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($@"{cl.FullName}.{prop.Name}: {ex.InnerException?.Message ?? ex.Message}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}Следующие у следующих свойств setter'ы некорректны:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что атрибут <see cref="BusinessServerAttribute" /> корректен.
        /// </summary>
        [Fact]
        public void TestAllBusinessServerNamesAreValid()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();

            // Act.
            var errors = new List<string>();
            foreach (var type in assemblyClasses)
            {
                try
                {
                    var atrs = type.GetCustomAttributes<BusinessServerAttribute>(false).ToList();
                    if (atrs.Count > 1)
                    {
                        errors.Add($"{type.FullName}: более одного атрибута {nameof(BusinessServerAttribute)}.");
                    }
                    else if (atrs.Any())
                    {
                        var atr = atrs.First();
                        var method = atr.BusinessServerType.GetMethod(
                            $"OnUpdate{type.Name}",
                            BindingFlags.Public | BindingFlags.Instance);
                        if (method == null)
                        {
                            errors.Add($"{type.FullName}: не найден метод \"OnUpdate{type.Name}\" в БСе {atr.BusinessServerType.FullName}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($@"{type.FullName}: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}У следующих классов атрибут {nameof(BusinessServerAttribute)} некорректен:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что у всех хранимых классов DataObject-свойства имеют <see cref="PropertyStorageAttribute"/>.
        /// </summary>
        [Fact]
        public void TestAllDataObjectPropertyHasPropertyStorageAttribute()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var errors = new List<string>();

            // Act.
            foreach (var type in assemblyClasses)
            {
                var classProperties = type.GetProperties()
                    .Where(
                        prop => Information.IsStoredProperty(type, prop.Name)
                             && prop.PropertyType.IsSubclassOf(typeof(DataObject))
                             && !prop.GetCustomAttributes<PropertyStorageAttribute>().Any());
                errors.AddRange(classProperties.Select(prop => $"{type.FullName}.{prop.Name}"));
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}У следующих хранимых классов DataObject-свойства отсутствует атрибут [PropertyStorage]:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что статические представления во вложенном классе Views валидны.
        /// </summary>
        [Fact]
        public void TestStaticViewsAreValid()
        {
            // Arrange.
            var dontCheckClasses = GetTypesWithoutValidViews();
            var assemblyClasses = GetStoredDataObjects();
            var checkedTypes = assemblyClasses.Where(x => !dontCheckClasses.Contains(x)).ToList();

            // Act.
            var errors = new List<string>();
            foreach (var type in checkedTypes)
            {
                var viewsType = type.GetNestedType("Views");
                if (viewsType == null)
                {
                    errors.Add($@"{type.FullName}: не содержит Views-subclass.");
                }
                else
                {
                    var properties = viewsType.GetProperties(BindingFlags.Public | BindingFlags.Static).ToList();
                    foreach (var prop in properties)
                    {
                        try
                        {
                            prop.GetValue(null, null);
                        }
                        catch (Exception ex)
                        {
                            errors.Add($@"{type.FullName}.{prop.Name}: {ex.InnerException?.Message ?? ex.Message}");
                        }
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}У следующих классов Views-subclass некорректны:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что во всех представлениях детейлов присутствует свойство агрегатора.
        /// </summary>
        [Fact]
        public void TestDetailViews()
        {
            // Arrange.
            var assemblyClasses = GetStoredDataObjects();
            var errors = new List<string>();

            // Act.
            foreach (var type in assemblyClasses)
            {
                string agrProp = Information.GetAgregatePropertyName(type);
                if (string.IsNullOrEmpty(agrProp))
                    continue;

                var viewNames = Information.AllViews(type);
                foreach (string viewName in viewNames)
                {
                    var view = Information.GetView(viewName, type);
                    if (view.Properties.All(p => p.Name != agrProp))
                    {
                        errors.Add($"{type.FullName} {viewName}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}В следующих представлениях детейловых классов не найдены свойство агрегатора:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }

        /// <summary>
        ///     Тест проверяет, что для всех нехранимых свойств добавлены необходимые зависимые свойства.
        /// </summary>
        [Fact]
        public void TestNotStoredPropertiesInViews()
        {
            // Arrange.
            var dataServiceType = GetDataServiceType();
            var assemblyClasses = GetStoredDataObjects();
            var errors = new List<string>();

            // Act.
            foreach (var type in assemblyClasses)
            {
                var viewNames = Information.AllViews(type);
                foreach (string viewName in viewNames)
                {
                    // Эталонное представление.
                    var view = Information.GetView(viewName, type);

                    // Представление, в которое будут добавлены зависимые свойства.
                    var viewAppended = Information.GetView(viewName, type);
                    Information.AppendPropertiesFromNotStored(viewAppended, dataServiceType);

                    // Выделим свойства, которые были добавлены методом Information.AppendPropertiesFromNotStored.
                    var extraProps = viewAppended.Properties.Where(p => !view.CheckPropname(p.Name)).ToList();
                    if (extraProps.Any())
                    {
                        errors.Add($"{type.FullName} {viewName}:{Environment.NewLine}{string.Join(Environment.NewLine, extraProps.Select(p => p.Name))}");
                    }
                }
            }

            // Assert.
            Assert.False(
                errors.Any(),
                $"{Environment.NewLine}В следующих представлениях обнаружены ошибки нехранимых свойств:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
        }
    }
}
