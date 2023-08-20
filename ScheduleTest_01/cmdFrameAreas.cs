#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace ScheduleTest_01
{
    [Transaction(TransactionMode.Manual)]
    public class cmdFrameAreas : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;
           
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Schedule");

                AreaScheme curAreaScheme = Utils.GetAreaSchemeByName(doc, "S Frame");
                ViewSchedule newSched = Utils.CreateAreaSchedule(doc, "Frame Areas - Elevation S", curAreaScheme);

                List<string> paramNames = new List<string>() { "Area Category", "Name", "Area" };

                List<Parameter> paramList = Utils.GetParametersByName(doc, paramNames, BuiltInCategory.OST_Areas);
                Utils.AddFieldsToSchedule(doc, newSched, paramList);







                t.Commit();



            }
            
            return Result.Succeeded;
        }        
    }
}
