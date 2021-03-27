using System.Collections.Generic;
using EVA_S.ExtensibleStorageExtension.Attributes;
using EVA_S.ExtensibleStorageExtension;
using Autodesk.Revit.DB;


namespace EVA_S
{
    [SchemaAttribute("98C5A291-D325-4DAA-B819-016C2BC2A927", "SetParameters", Documentation = "Класс для записи настроек")]
    public class ParametersNameEntity : IRevitEntity
    {
        [FieldAttribute]
        public string Param_CircName { get; set; }

        [FieldAttribute]
        public string Param_CircuitsNames { get; set; }
    }


}
