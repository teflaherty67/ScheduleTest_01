#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ScheduleTest_01;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace ScheduleTest_01
{
    [Transaction(TransactionMode.Manual)]
    public class cmdFloorAreaPlanTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document curDoc = uiapp.ActiveUIDocument.Document;

            // get the category & set category Id
            Category areaCat = curDoc.Settings.Categories.get_Item(BuiltInCategory.OST_Areas);

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

                ColorFillScheme schemeColorFill = Utils.GetColorFillSchemeByName(curDoc, "Floor", schemeFloor);
                areaFloor.SetColorFillSchemeId(areaCat.Id, schemeColorFill.Id);

                XYZ insStart = new XYZ(50, 0, 0);
                UV insPoint = new UV(insStart.X, insStart.Y);
                XYZ tagInsert = new XYZ(50, 0, 0);

                List<clsAreaInfo> areas = new List<clsAreaInfo>
                {
                    new clsAreaInfo("1", "Living", "Total Covered", "A"),
                    new clsAreaInfo("2", "Garage", "Total Covered", "B"),
                    new clsAreaInfo("3", "Covered Patio", "Total Covered", "C"),
                    new clsAreaInfo("4", "Covered Porch", "Total Covered", "D"),
                    new clsAreaInfo("5", "Porte Cochere", "Total Covered", "E"),
                    new clsAreaInfo("6", "Patio", "Total Uncovered", "F"),
                    new clsAreaInfo("7", "Porch", "Total Uncovered", "G"),
                    new clsAreaInfo("8", "Option", "Options", "H")
                };

                foreach (var areaInfo in areas)
                {
                    Utils.CreateAreaWithTag(curDoc, areaFloor, ref insPoint, ref tagInsert, areaInfo);
                }

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