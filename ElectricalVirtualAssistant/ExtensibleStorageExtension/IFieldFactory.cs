using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autodesk.Revit.DB.ExtensibleStorage;
using EVA_S.ExtensibleStorageExtension.Attributes;

namespace EVA_S.ExtensibleStorageExtension
{
    public interface IFieldFactory
    {
        FieldBuilder CreateField(SchemaBuilder schemaBuilder, PropertyInfo propertyInfo);
    }

    class FieldFactory : IFieldFactory
    {
        public FieldBuilder CreateField(SchemaBuilder schemaBuilder, PropertyInfo propertyInfo)
        {
            IFieldFactory fieldFactory = null;

            var fieldType = propertyInfo.PropertyType;

            //являеться ли тип универсальным, только IList IDictionary

            if (fieldType.IsGenericType)
            {
                foreach (var interfaceType in fieldType.GetInterfaces())
                {
                    if(interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>))
                    {
                        fieldFactory = new ArrayFieldCreator(); //Создатель в схему поля для листа (Массив)
                        break;
                    }
                    else if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                    {
                        fieldFactory = new MapFieldCreator();  //Создатель в схему поля для словаря (Map)
                        break;
                    }

                }

                //Если тип обобщенный и не List и не Dictionary
                if (fieldFactory == null)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(string.Format("Type {0} does not supported.", fieldType));
                    sb.AppendLine("Only IList<T> and IDictionary<TKey, TValue> generic types are supproted");
                    throw new NotSupportedException(sb.ToString());
                }
            }
            else //иначе создать обычное поле
            {
                fieldFactory = new SimpleFieldCreator();
            }
            
            //Вызов общего метода создания
            return fieldFactory.CreateField(schemaBuilder, propertyInfo); 
        }
    }
    /// <summary>
    /// Создание обычного поля
    /// </summary>
    class SimpleFieldCreator : IFieldFactory
    {
        public FieldBuilder CreateField(SchemaBuilder schemaBuilder, PropertyInfo propertyInfo)
        {
            FieldBuilder fieldBuilder;

            var iRevitEntity = propertyInfo.PropertyType.GetInterface("IRevitEntity");
            if (iRevitEntity != null)
            {
                AttributeExtractor<SchemaAttribute> schemaAttributeExtractor = new AttributeExtractor<SchemaAttribute>();
                fieldBuilder = schemaBuilder.AddSimpleField(propertyInfo.Name, typeof(Entity));
                var subSchemaAttribute = schemaAttributeExtractor.GetAttribute(propertyInfo.PropertyType); //Получение данных аттрибута класса схемы
                fieldBuilder.SetSubSchemaGUID(subSchemaAttribute.GUID);
            }
            else
            {
                fieldBuilder = schemaBuilder.AddSimpleField(propertyInfo.Name, propertyInfo.PropertyType);
            }

            return fieldBuilder;
        }
    }



    /// <summary>
    /// Cоздание поля для листа
    /// </summary>
    class ArrayFieldCreator : IFieldFactory
    {
        public FieldBuilder CreateField(SchemaBuilder schemaBuilder, PropertyInfo propertyInfo)
        {
            FieldBuilder fieldBuilder;
            //Проверка является ли обобщенный тип дочерним от IRevitEntity
            var genericType = propertyInfo.PropertyType.GetGenericArguments()[0];
            var iRevitEntity = genericType.GetInterface("IRevitEntity");
            if (iRevitEntity != null)
            {
                fieldBuilder = schemaBuilder.AddArrayField(propertyInfo.Name, typeof(Entity)); //В полях храняться сущности
                AttributeExtractor<SchemaAttribute> schemaAttributeExtractor = new AttributeExtractor<SchemaAttribute>();
                var subSchemaAttribute = schemaAttributeExtractor.GetAttribute(genericType); //Подсхема
                fieldBuilder.SetSubSchemaGUID(subSchemaAttribute.GUID);
            }
            else
            {
                fieldBuilder = schemaBuilder.AddArrayField(propertyInfo.Name, genericType);
            }
            return fieldBuilder;
        }
    }
    /// <summary>
    /// Cоздание поля для словаря
    /// </summary>
    class MapFieldCreator : IFieldFactory
    {
        public FieldBuilder CreateField(SchemaBuilder schemaBuilder, PropertyInfo propertyInfo)
        {
            FieldBuilder fieldBuilder;
            var genericKeyType = propertyInfo.PropertyType.GetGenericArguments()[0]; //Получение типа свойства атрибута для Key
            var genericValueType = propertyInfo.PropertyType.GetGenericArguments()[1]; //Получение типа свойства атрибута для Value

            if(genericValueType.GetInterface("IRevitEntity") != null) //Если класс реализует интерфейс и позволяет записывать подсхемы
            {
                fieldBuilder = schemaBuilder.AddMapField(propertyInfo.Name, genericKeyType, typeof(Entity));

                AttributeExtractor<SchemaAttribute> schemaAttributeExtractor = new AttributeExtractor<SchemaAttribute>();
                var subSchemaAttribute = schemaAttributeExtractor.GetAttribute(genericValueType); //Подсхема
                fieldBuilder.SetSubSchemaGUID(subSchemaAttribute.GUID);
            }
            else
            {
                fieldBuilder = schemaBuilder.AddMapField(propertyInfo.Name, genericKeyType, genericValueType);
            }
            
            var needSubSchemaId = fieldBuilder.NeedsSubSchemaGUID();

            return fieldBuilder;
        }
    }
}
