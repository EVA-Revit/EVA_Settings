using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using System.Diagnostics;
using System.IO;

namespace EVA_S
{
    internal class LoaderFamilys
    {
        public static void LoadFamilys (string varLoadFam, Document doc)
        {
            Debug.Print(varLoadFam);


            string tempFile = Assembly.GetExecutingAssembly().Location;
            tempFile = tempFile.Remove(tempFile.Length - 12) + "Familys/";

            //DirectoryInfo tempFileTest = new DirectoryInfo(@"C:\Users\s.shipovskiy\Desktop\EVA_Familys\");

            using (Transaction tran = new Transaction(doc))
            {
                try
                {
                    if (varLoadFam.Contains("ex"))
                    {
                        tran.Start("Загрузка семейств Ex");
                        LoadFamily(doc, tempFile + "EVA_ex");
                        tran.Commit();
                    }
                    if (varLoadFam.Contains("streams"))
                    {
                        tran.Start("Загрузка семейств Streams");
                        LoadFamily(doc, tempFile + "EVA_streams");
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                    tran.RollBack();
                }
            }
        }

        private static void LoadFamily(Document doc, string tempFile)
        {
            DirectoryInfo tempFileTest = new DirectoryInfo(tempFile);
            FileInfo[] files = tempFileTest.GetFiles("*.rfa");
            Family family = null;
            foreach (FileInfo file in files)
            {
                doc.LoadFamily(file.FullName, out family);
            } 
        }
    }
}
