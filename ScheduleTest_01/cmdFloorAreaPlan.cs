#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

#endregion

namespace ScheduleTest_01
{
    [Transaction(TransactionMode.Manual)]
    public class cmdFloorAreaPlan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Your code goes here
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Area Plan");

                AreaScheme schemeFloor = Utils.GetAreaSchemeByName(doc, "S Floor");
                ElementId schemeFloorId = schemeFloor.Id;

                Level curLevel = Utils.GetLevelByName(doc, "Main Level");
                ElementId curLevelId = curLevel.Id;

                View vtFloorAreas = Utils.GetViewTemplateByName(doc, "10-Floor Area");

                ViewPlan areaFloor = ViewPlan.CreateAreaPlan(doc, schemeFloorId, curLevelId);
                areaFloor.Name = "Floor";
                areaFloor.ViewTemplateId = vtFloorAreas.Id;

                t.Commit();
            }
            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
