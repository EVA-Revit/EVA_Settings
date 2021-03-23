using System.Collections.Generic;
using EVA_S.ExtensibleStorageExtension.Attributes;
using EVA_S.ExtensibleStorageExtension;
using Autodesk.Revit.DB;


namespace EVA_S
{
    [SchemaAttribute("98C5A291-D325-4DAA-B819-016C2BC2A938", "ConnectorEntity", Documentation = "Класс для записи в коннектор")]
    public class ParametersNameEntity : IRevitEntity
    {
        [FieldAttribute]
        public string Param_CircName { get; set; }

        [FieldAttribute]
        public string Param_CircuitsNames { get; set; }
    }


    [SchemaAttribute("B6A72ED7-97C4-4B06-A17D-7E0DC1363082", "CircuitEntity", Documentation = "Класс для записи в цепь")]
    public class CircuittEntity : IRevitEntity
    {
        [FieldAttribute]
        public List<ElementId> IdConnectorsInCircuit { get; set; }
    }

}
