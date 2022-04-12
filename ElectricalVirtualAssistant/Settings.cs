using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.ExtensibleStorage;
using EVA_S.ExtensibleStorageExtension.ElementExtension;
using Autodesk.Revit.ApplicationServices;


namespace EVA_S
{
    class Settings
    {
        public static Document doc { get; set; }
        //private static UIDocument uidoc;
        public static UIDocument uidoc { get; private set; }
        public static UIApplication uiApp { get; private set; }
        public static Application app { get; private set; }

        public static Result ResultCode(ExternalCommandData commandData, ref string message)
        {
            try
            {
                uiApp = commandData.Application;
                uidoc = uiApp.ActiveUIDocument;
                app = uiApp.Application;
                doc = uidoc.Document;
                //Здесь пишется код или метод boolean
                if (!SettingsMetod())
                {
                    return Result.Failed;
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message + ex.StackTrace;
                return Result.Failed;
            }

        }
        /// <summary>
        /// Метод в котором пишется основной код
        /// </summary>
        /// <returns></returns>
        static Boolean SettingsMetod()
        {

            Element el = GetStorageElement();

            var paramSettings = el.GetEntity<ParametersNameEntity>();

            if(paramSettings == null)
            {
                paramSettings = new ParametersNameEntity();
                paramSettings.Param_CircName = "Имя_цепи_EVA";
                paramSettings.Param_CircuitsNames = "Группа_имен_цепей_EVA";
            }
            if (paramSettings.Param_CircName == "") paramSettings.Param_CircName = "Имя_цепи_EVA";
            if (paramSettings.Param_CircuitsNames == "") paramSettings.Param_CircuitsNames = "Группа_имен_цепей_EVA";

            var viewModel = new WPF.MainViewModel(paramSettings);
            var view = new WPF.SettingsWindow();
            viewModel.WindowView = view;
            view.DataContext = viewModel;
            view.ShowDialog();
            if (view.DialogResult == false) return false;
            using (Transaction newTran = new Transaction(doc, "Запись имен параметров EVA"))
            {
                newTran.Start();
                el.SetEntity(viewModel.Ent);
                newTran.Commit();
            }

            if (viewModel.IsLoadSharedParameters) CreateSharedParameters.CreateSharedParameter(doc, app);

            string loadFamilysVariant = "";
            if (viewModel.IsLoadFamelesEVAex) loadFamilysVariant = "ex";
            if (viewModel.IsLoadFamelesEVAstreams) loadFamilysVariant = loadFamilysVariant + "streams";
            if (viewModel.IsLoadFamelesEVAcirc) loadFamilysVariant = loadFamilysVariant + "circ";

            if (loadFamilysVariant != "") LoaderFamilys.LoadFamilys(loadFamilysVariant, doc);

            return true;
        }



        

        public static Element GetStorageElement()
        {
            ProjectInfo pi = doc.ProjectInformation;
            return pi as Element;
        }
    }
}
