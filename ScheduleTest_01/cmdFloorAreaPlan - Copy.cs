#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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
            //ElementId areaCatID = areaCat.Id;

            ColorFillScheme schemeColorFill = Utils.GetColorFillSchemeByName(curDoc, "Floor");
            //ElementId schemeColorFillId = schemeColorFill.Id;

            // Your code goes here
            using (Transaction t = new Transaction(curDoc))
            {
                t.Start("Create Area Plan");

                AreaScheme schemeFloor = Utils.GetAreaSchemeByName(curDoc, "S Floor");
                Level curLevel = Utils.GetLevelByName(curDoc, "First Floor");
                View vtFloorAreas = Utils.GetViewTemplateByName(curDoc, "10-Floor Area");

                ViewPlan areaFloor = ViewPlan.CreateAreaPlan(curDoc, schemeFloor.Id, curLevel.Id);
                areaFloor.Name = "Floor";
                areaFloor.ViewTemplateId = vtFloorAreas.Id;
                areaFloor.SetColorFillSchemeId(areaCat.Id, schemeColorFill.Id);

                UV insPoint = new UV(0, 0);
                UV offset = new UV(0, 10);

                List<(string number, string name, string category, string comments)> areas = new List<(string, string, string, string)>
    {
        ("1", "Living", "Total Covered", "A"),
        ("2", "Garage", "Total Covered", "B"),
        ("3", "Covered Patio", "Total Covered", "C"),
        //... Add the rest of the areas here
    };

                foreach (var areaInfo in areas)
                {
                    CreateAndTagArea(curDoc, areaFloor, insPoint, areaInfo.number, areaInfo.name, areaInfo.category, areaInfo.comments);
                    insPoint = insPoint.Add(offset);
                }

                t.Commit();
            }

            return Result.Succeeded;
        }

        private Area CreateAndTagArea(Document curDoc, ViewPlan areaFloor, UV insPoint, string number, string name, string category, string comments)
        {
            Area area = curDoc.Create.NewArea(areaFloor, insPoint);
            area.Number = number;
            area.Name = name;
            area.LookupParameter("Area Category").Set(category);
            area.LookupParameter("Comments").Set(comments);

            // Assuming you still want to create the AreaTag
            AreaTag tag = curDoc.Create.NewAreaTag(areaFloor, area, insPoint);

            return area;
        }


        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}

//internal static ColorFillScheme GetColorFillSchemeByName(Document curDoc, string schemeName, AreaScheme areaScheme)

//{

//    ColorFillScheme colorfill = new FilteredElementCollector(curDoc)

//        .OfCategory(BuiltInCategory.OST_ColorFillSchema)

//        .Cast<ColorFillScheme>()

//        .Where(x => x.Name.Equals(schemeName) && x.AreaSchemeId.Equals(areaScheme.Id))

//        .First();


//    return colorfill;

//}


