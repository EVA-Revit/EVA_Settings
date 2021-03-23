using System;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace EVA_S.ExtensibleStorageExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaAttribute : Attribute
    {
        //Поля
        private readonly string _schemaName;
        private readonly Guid _guid;

        //Конструктор
        public SchemaAttribute(string guid, string schemaName)
        {
            _schemaName = schemaName;
            _guid = new Guid(guid);
        }


        //Свойства
        public string SchemaName
        {
            get
            {
                return _schemaName;
            }
        }

        public Guid GUID
        {
            get
            {
                return _guid;
            }
        }

        public Guid ApplicationGUID { get; set; }

        public string Documentation { get; set; }

        public AccessLevel ReadAccesLevel { get; set; }

        public AccessLevel WriteAccesLevel { get; set; }

        public string VendorId { get; set; }
    }
}
