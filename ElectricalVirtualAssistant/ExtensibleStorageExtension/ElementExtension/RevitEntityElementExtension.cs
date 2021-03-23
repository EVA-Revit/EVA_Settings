using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using EVA_S.ExtensibleStorageExtension.Attributes;

namespace EVA_S.ExtensibleStorageExtension.ElementExtension
{
    public static class RevitEntityElementExtension
    {
        public static void SetEntity (this Element element, IRevitEntity revitEntity)
        {
            ISchemaCreator schemaCreator = new SchemaCreator();
            IEntityConverter entityConverter = new EntityConverter(schemaCreator);

            Entity entity = entityConverter.Convert(revitEntity);

            element.SetEntity(entity);

        }

        /// <summary>
        /// Получение RevitEntity
        /// </summary>
        /// <typeparam name="TRevitEntity"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static TRevitEntity GetEntity<TRevitEntity>(this Element element) where TRevitEntity : class, IRevitEntity
        {
            Type revitEntityType = typeof(TRevitEntity);

            AttributeExtractor<SchemaAttribute> schemaAttributeExtractor =
                new AttributeExtractor<SchemaAttribute>();

            var schemaAttribute =
                schemaAttributeExtractor.GetAttribute(revitEntityType);

            Schema schema = Schema.Lookup(schemaAttribute.GUID);
            if (schema == null)
                return null;

            var entity =
                element.GetEntity(schema);

            if (entity == null || !entity.IsValid())
                return null;

            ISchemaCreator schemaCreator = new SchemaCreator();
            IEntityConverter entityConverter = new EntityConverter(schemaCreator);

            var revitEntity = entityConverter.Convert<TRevitEntity>(entity);

            return revitEntity;
        }

        /// <summary>
        /// Удаление Entity
        /// </summary>
        /// <typeparam name="TRevitEntity"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool DeleteEntity<TRevitEntity>(this Element element) where TRevitEntity : class, IRevitEntity
        {
            Type revitEntityType = typeof(TRevitEntity);

            AttributeExtractor<SchemaAttribute> schemaAttributeExtractor =
                new AttributeExtractor<SchemaAttribute>();

            var schemaAttribute =
                schemaAttributeExtractor.GetAttribute(revitEntityType);

            Schema schema = Schema.Lookup(schemaAttribute.GUID);
            if (schema == null)
            {
                return false;
            }

            return element.DeleteEntity(schema);
        }
    }










    public static class EntityExtension
    {
        public static void SetWrapper<T>(this Entity entity, Field field, IList<T> value)
        {
            entity.Set(field, value);
        }

        public static void SetWrapper<T>(this Entity entity,
            Field field,
            IList<T> value,
            DisplayUnitType displayUnitType)
        {
            entity.Set(field, value, displayUnitType);
        }

        public static void SetWrapper<TKey, TValue>(this Entity entity,
            Field field,
            IDictionary<TKey, TValue> value)
        {
            entity.Set(field, value);
        }

        public static void SetWrapper<TKey, TValue>(this Entity entity,
            Field field,
            IDictionary<TKey, TValue> value,
            DisplayUnitType displayUnitType)
        {
            entity.Set(field, value, displayUnitType);
        }
    }
}
