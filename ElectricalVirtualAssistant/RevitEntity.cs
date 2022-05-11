using System.Collections.Generic;
using EVA_S.ExtensibleStorageExtension.Attributes;
using EVA_S.ExtensibleStorageExtension;
using Autodesk.Revit.DB;


namespace EVA_S
{
    [SchemaAttribute("99C5A291-D325-4DAA-B819-016C2BC2A927", "SetParameters", Documentation = "Класс для записи настроек")]
    public class ParametersNameEntity : IRevitEntity
    {
        [FieldAttribute]
        public string Param_CircName { get; set; }

        [FieldAttribute]
        public string Param_CircuitsNames { get; set; }

        [FieldAttribute]
        public string Param_LoadName { get; set; }

        [FieldAttribute]
        public string Param_TextName { get; set; }

        [FieldAttribute]
        public string Param_DoubleName { get; set; }
    }


}
