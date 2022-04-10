using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace EVA_S
{
    internal class LoaderFamilys
    {
        public static void LoadFamilys ()
        {
            TaskDialog.Show("ChekBox", "LoadFamily");
        }
    }
}
