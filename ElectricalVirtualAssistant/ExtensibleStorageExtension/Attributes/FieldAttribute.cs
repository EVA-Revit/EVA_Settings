using System;
using Autodesk.Revit.DB;


namespace EVA_S.ExtensibleStorageExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public UnitType UnitType { get; set; }
        public string Documentation { get; set; } 
        //Конструктор по умолчанию
        public FieldAttribute()
        {
            UnitType = UnitType.UT_Undefined;
        }
    }
}
