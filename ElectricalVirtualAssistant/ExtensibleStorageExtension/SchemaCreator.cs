using System;
using System.Reflection;
using Autodesk.Revit.DB.ExtensibleStorage;
using EVA_S.ExtensibleStorageExtension.Attributes;

namespace EVA_S.ExtensibleStorageExtension
{
    public class SchemaCreator : ISchemaCreator
    {
        private readonly AttributeExtractor<SchemaAttribute> _schemaAttributeExtractor = new AttributeExtractor<SchemaAttribute>();

        private readonly AttributeExtractor<FieldAttribute> _fieldAttributeExtractor = new AttributeExtractor<FieldAttribute>();

        private readonly IFieldFactory _fieldFactory = new FieldFactory();




        //Реализация интерфейса
        public Schema CreateSchema (Type type)
        {
            SchemaAttribute schemaAttribute = _schemaAttributeExtractor.GetAttribute(type); //Получение аттрибутов

            //Создание схемы с использованием аттрибутов свойств
            SchemaBuilder schemaBuilder = new SchemaBuilder(schemaAttribute.GUID);
            schemaBuilder.SetSchemaName(schemaAttribute.SchemaName);

            //Устанавливает другие свойства схемы, если они существуют
            if (schemaAttribute.ApplicationGUID != Guid.Empty)
                schemaBuilder.SetApplicationGUID(schemaAttribute.ApplicationGUID);

            if (!string.IsNullOrEmpty(schemaAttribute.Documentation))
                schemaBuilder.SetDocumentation(schemaAttribute.Documentation);

            if (schemaAttribute.ReadAccesLevel != default(AccessLevel))
                schemaBuilder.SetReadAccessLevel(schemaAttribute.ReadAccesLevel);

            if (schemaAttribute.WriteAccesLevel != default(AccessLevel))
                schemaBuilder.SetReadAccessLevel(schemaAttribute.WriteAccesLevel);

            if (!string.IsNullOrEmpty(schemaAttribute.VendorId))
                schemaAttribute.VendorId = schemaAttribute.VendorId;

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance); // все свойства класса

            //Перебор всех свойств RevitEntity
            foreach (var pi in properties)
            {
                //Получение пользовательского атрибута для открытых свойств
                var propertyAttributes = pi.GetCustomAttributes(typeof(FieldAttribute), true);
                //если свойство не имеет атрибута FieldAttribute, пропустить это свойство
                if (propertyAttributes.Length == 0) continue;

                FieldAttribute fieldAttribute = _fieldAttributeExtractor.GetAttribute(pi);

                FieldBuilder fieldBuilder = _fieldFactory.CreateField(schemaBuilder, pi);

                //Если поле содержится в IRevitEntity, то создать Schema и SubSchema
                if (!string.IsNullOrEmpty(fieldAttribute.Documentation)) fieldBuilder.SetDocumentation(fieldAttribute.Documentation);

                if (fieldBuilder.NeedsUnits()) fieldBuilder.SetUnitType(fieldAttribute.UnitType);

            }

            return schemaBuilder.Finish();
        }

    }
}
