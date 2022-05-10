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
    internal class CreateSharedParameters
    {
        public static void CreateSharedParameter(Document doc, Application app)
        {
            Category _ElectricalFixtures = doc.Settings.Categories.get_Item(BuiltInCategory.OST_ElectricalFixtures);
            Category _ElectricalEquipment = doc.Settings.Categories.get_Item(BuiltInCategory.OST_ElectricalEquipment);
            Category _LightingDevices = doc.Settings.Categories.get_Item(BuiltInCategory.OST_LightingDevices);
            Category _LightingFixtures = doc.Settings.Categories.get_Item(BuiltInCategory.OST_LightingFixtures);
            Category _MechanicalEquipment = doc.Settings.Categories.get_Item(BuiltInCategory.OST_MechanicalEquipment);
            Category _CommunicationDevices = doc.Settings.Categories.get_Item(BuiltInCategory.OST_CommunicationDevices);
            Category _DataDevices = doc.Settings.Categories.get_Item(BuiltInCategory.OST_DataDevices);
            Category _DuctAccessory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_DuctAccessory);
            Category _PipeAccessory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_PipeAccessory);
            Category _PlumbingFixtures = doc.Settings.Categories.get_Item(BuiltInCategory.OST_PlumbingFixtures);
            Category _SecurityDevices = doc.Settings.Categories.get_Item(BuiltInCategory.OST_SecurityDevices);
            Category _TelephoneDevices = doc.Settings.Categories.get_Item(BuiltInCategory.OST_TelephoneDevices);
            Category _NurseCallDevices = doc.Settings.Categories.get_Item(BuiltInCategory.OST_NurseCallDevices);

            CategorySet categorySet = app.Create.NewCategorySet();

            categorySet.Insert(_ElectricalFixtures);
            categorySet.Insert(_ElectricalEquipment);
            categorySet.Insert(_LightingDevices);
            categorySet.Insert(_LightingFixtures);
            categorySet.Insert(_MechanicalEquipment);
            categorySet.Insert(_CommunicationDevices);
            categorySet.Insert(_DataDevices);
            categorySet.Insert(_DuctAccessory);
            categorySet.Insert(_PipeAccessory);
            categorySet.Insert(_PlumbingFixtures);
            categorySet.Insert(_SecurityDevices);
            categorySet.Insert(_TelephoneDevices);
            categorySet.Insert(_NurseCallDevices);

            string originalFile = app.SharedParametersFilename;
            string tempFile = Assembly.GetExecutingAssembly().Location;
            tempFile = tempFile.Remove(tempFile.Length - 12) + "SharedParameters.txt";

            //string tempFileTest = @"C:\Users\s.shipovskiy\Desktop\EVA_SharedParameters.txt";

            //TaskDialog.Show("test", tempFile);

            try
            {
                app.SharedParametersFilename = tempFile;

                DefinitionFile sharedParametersFile = app.OpenSharedParameterFile();

                foreach (DefinitionGroup dg in sharedParametersFile.Groups)
                {
                    if (dg.Name == "EVA")
                    {
                        ExternalDefinition externalDefinitionNameCirc = dg.Definitions.get_Item("EVA_Имя_цепи") as ExternalDefinition;
                        ExternalDefinition externalDefinitionNamesGroupCirc = dg.Definitions.get_Item("EVA_Группа_имен_цепей") as ExternalDefinition;
                        ExternalDefinition externalDefinitionNamesLoad = dg.Definitions.get_Item("EVA_Имя_Нагрузки") as ExternalDefinition;
                        ExternalDefinition externalDefinitionText = dg.Definitions.get_Item("EVA_Текст") as ExternalDefinition;
                        ExternalDefinition externalDefinitionDouble = dg.Definitions.get_Item("EVA_Число") as ExternalDefinition;

                        using (Transaction transaction = new Transaction(doc))
                        {
                            transaction.Start("Add Shared Parameters");

                            InstanceBinding binding = app.Create.NewInstanceBinding(categorySet);
                            doc.ParameterBindings.Insert(externalDefinitionNameCirc, binding, BuiltInParameterGroup.PG_TEXT);
                            doc.ParameterBindings.Insert(externalDefinitionNamesGroupCirc, binding, BuiltInParameterGroup.PG_TEXT);
                            doc.ParameterBindings.Insert(externalDefinitionNamesLoad, binding, BuiltInParameterGroup.PG_TEXT);
                            doc.ParameterBindings.Insert(externalDefinitionText, binding, BuiltInParameterGroup.PG_TEXT);
                            doc.ParameterBindings.Insert(externalDefinitionDouble, binding, BuiltInParameterGroup.PG_TEXT);
                            transaction.Commit();
                        }
                    }
                }
            }
            catch { }
            finally
            {
                app.SharedParametersFilename = originalFile;
            }
        }
    }
}
