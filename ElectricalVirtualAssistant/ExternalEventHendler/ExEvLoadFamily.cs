using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EVA_S.ExternalEventHendler
{
    class ExEvLoadFamily : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            using (Transaction tr = new Transaction(doc, "Diolog"))
            {
                tr.Start();
                //TaskDialog.Show("Transaction", "ExternalEventHendler");
                System.Windows.MessageBox.Show("TEXT");
                tr.Commit();

            }



            
        }

        public string GetName() => nameof(ExEvLoadFamily);

    }
}
