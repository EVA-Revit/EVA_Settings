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
using System.Reflection;



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
            string nameCirc = "EVA_Имя_цепи";
            string groupNameCirc = "EVA_Группа_имен_цепей";
            string nameLoad = "EVA_Имя_Нагрузки";
            string evaText = "EVA_Текст";
            string evaDouble = "EVA_Число";
            INIManager manager;


            Element el = GetStorageElement();
            //el.DeleteEntity<>
            var paramSettings = el.GetEntity<ParametersNameEntity>();

            if(paramSettings == null)
            {
                paramSettings = new ParametersNameEntity();
                paramSettings.Param_CircName = nameCirc;
                paramSettings.Param_CircuitsNames = groupNameCirc;
                paramSettings.Param_LoadName = nameLoad;
                paramSettings.Param_TextName = evaText;
                paramSettings.Param_DoubleName = evaDouble;
                paramSettings.StorageInProject = false;
            }
            if (paramSettings.Param_CircName == "") paramSettings.Param_CircName = nameCirc;
            if (paramSettings.Param_CircuitsNames == "") paramSettings.Param_CircuitsNames = groupNameCirc;
            if (paramSettings.Param_LoadName == "") paramSettings.Param_LoadName = nameLoad;
            if (paramSettings.Param_TextName == "") paramSettings.Param_TextName = evaText;
            if (paramSettings.Param_DoubleName == "") paramSettings.Param_DoubleName = evaDouble;



            var viewModel = new WPF.MainViewModel(paramSettings);
            string pathApDll = Assembly.GetExecutingAssembly().Location;
            string pathApIni = pathApDll.Replace("dll", "ini");
            //manager = new INIManager("C:\\ProgramData\\Autodesk\\Revit\\Addins\\2020\\EVA_Settings.ini");
            manager = new INIManager(pathApIni);
            //Использовать локальный файл настроек
            if (!paramSettings.StorageInProject)
            {
                

                //Назначение полям текстбоксов значений из файла конфигурации или по умолчанию
                viewModel.ParamCircName = manager.GetPrivateString("Parameters", "CircName");
                viewModel.ParamCircName = viewModel.ParamCircName == "" ? nameCirc : viewModel.ParamCircName;

                viewModel.ParamCircuitsNames = manager.GetPrivateString("Parameters", "CircuinsNames");
                viewModel.ParamCircuitsNames = viewModel.ParamCircuitsNames == "" ? groupNameCirc : viewModel.ParamCircuitsNames;

                viewModel.ParamLoadName = manager.GetPrivateString("Parameters", "LoadName");
                viewModel.ParamLoadName = viewModel.ParamLoadName == "" ? nameLoad : viewModel.ParamLoadName;

                viewModel.ParamTextName = manager.GetPrivateString("Parameters", "Text");
                viewModel.ParamTextName = viewModel.ParamTextName == "" ? evaText : viewModel.ParamTextName;

                viewModel.ParamDoubleName = manager.GetPrivateString("Parameters", "Double");
                viewModel.ParamDoubleName = viewModel.ParamDoubleName == "" ? evaDouble : viewModel.ParamDoubleName;

            }

            
            var view = new WPF.SettingsWindow();
            viewModel.WindowView = view;
            view.DataContext = viewModel;
            //viewModel.IsStorageInFile = true;   
            view.ShowDialog();
            if (view.DialogResult == false) return false;
            using (Transaction newTran = new Transaction(doc, "Запись имен параметров EVA"))
            {
                newTran.Start();
                el.SetEntity(viewModel.Ent);
                newTran.Commit();
            }
            if (viewModel.IsStorageInFile)
            {
                manager.WritePrivateString("Parameters", "CircName", viewModel.ParamCircName);
                manager.WritePrivateString("Parameters", "CircuinsNames", viewModel.ParamCircuitsNames);
                manager.WritePrivateString("Parameters", "LoadName", viewModel.ParamLoadName);
                manager.WritePrivateString("Parameters", "Text", viewModel.ParamTextName);
                manager.WritePrivateString("Parameters", "Double", viewModel.ParamDoubleName);
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
