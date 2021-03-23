using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace EVA_S.ExtensibleStorageExtension
{
    public interface ISchemaCreator
    {
        Schema CreateSchema(Type type);
    }
}
