using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace EVA_S
{
    [TransactionAttribute(TransactionMode.Manual)]
    //[RegenerationAttribute(RegenerationOption.Manual)]
    class SettingsComand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Settings.ResultCode(commandData, ref message);
        }
    }
}