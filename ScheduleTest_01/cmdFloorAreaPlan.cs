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
            Document curDoc = uiapp.ActiveUIDocument.Document;

            // Your code goes here
            using (Transaction t = new Transaction(curDoc))
            {
                t.Start("Create Area Plan");

                AreaScheme schemeFloor = Utils.GetAreaSchemeByName(curDoc, "S Floor");
                ElementId schemeFloorId = schemeFloor.Id;

                Level curLevel = Utils.GetLevelByName(curDoc, "Main Level");
                ElementId curLevelId = curLevel.Id;

                View vtFloorAreas = Utils.GetViewTemplateByName(curDoc, "10-Floor Area");

                ViewPlan areaFloor = ViewPlan.CreateAreaPlan(curDoc, schemeFloorId, curLevelId);
                areaFloor.Name = "Floor";
                areaFloor.ViewTemplateId = vtFloorAreas.Id;

                XYZ insStart = new XYZ(0, 0, 0);               

                UV insPoint = new UV(insStart.X, insStart.Y);
                

                Area areaLiving1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaLiving1.Number = "1";
                areaLiving1.Name = "Living";

                Area areaGarage = curDoc.Create.NewArea(areaFloor, insPoint);
                areaGarage.Number = "2";
                areaGarage.Name = "Garage";

                Area areaCoveredPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPatio.Number = "3";
                areaCoveredPatio.Name = "Covered Patio";

                Area areaCoveredPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPorch.Number = "4";
                areaCoveredPorch.Name = "Covered Porch";

                Area areaPorteCochere = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorteCochere.Number = "5";
                areaPorteCochere.Name = "Porte Cochere";

                Area areaPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPatio.Number = "6";
                areaPatio.Name = "Patio";

                Area areaPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorch.Number = "7";
                areaPorch.Name = "Porch";

                Area areaOption1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaOption1.Number = "8";
                areaOption1.Name = "Option";
                areaOption1.LookupParameter("Area Category").Set("Options");

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
