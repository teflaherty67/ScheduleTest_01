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
using System.Windows.Controls;
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

            // get the category & set category Id
            Category areaCat = curDoc.Settings.Categories.get_Item(BuiltInCategory.OST_Areas);                    

            // Your code goes here
            using (Transaction t = new Transaction(curDoc))
            {
                t.Start("Create Area Plan");

                AreaScheme schemeFloor = Utils.GetAreaSchemeByName(curDoc, "S Floor");
                ElementId schemeFloorId = schemeFloor.Id;

                Level curLevel = Utils.GetLevelByName(curDoc, "First Floor");
                ElementId curLevelId = curLevel.Id;

                AreaTagType tagSymbol = Utils.GetIndependentTagByName(curDoc, "Area Tag");

                View vtFloorAreas = Utils.GetViewTemplateByName(curDoc, "10-Floor Area");

                ViewPlan areaFloor = ViewPlan.CreateAreaPlan(curDoc, schemeFloorId, curLevelId);
                areaFloor.Name = "Floor";
                areaFloor.ViewTemplateId = vtFloorAreas.Id;

                ColorFillScheme schemeColorFill = Utils.GetColorFillSchemeByName(curDoc, "Floor", schemeFloor);
               
                areaFloor.SetColorFillSchemeId(areaCat.Id, schemeColorFill.Id);

                // area insertion points
                XYZ insStart = new XYZ(50, 0, 0);               

                UV insPoint = new UV(insStart.X, insStart.Y);
                UV offset = new UV(0, 8);

                // area tag inserton points
                XYZ tagInsert = new XYZ(50, 0, 0);
                XYZ tagOffset = new XYZ(0, 8, 0);

                Area areaLiving1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaLiving1.Number = "1";
                areaLiving1.Name = "Living";
                areaLiving1.LookupParameter("Area Category").Set("Total Covered");
                areaLiving1.LookupParameter("Comments").Set("A");

                AreaTag livingTag = curDoc.Create.NewAreaTag(areaFloor, areaLiving1, insPoint);
                livingTag.TagHeadPosition = tagInsert;
                livingTag.HasLeader = false;

                insPoint = insPoint.Subtract(offset);
                
                Area areaGarage = curDoc.Create.NewArea(areaFloor, insPoint);
                areaGarage.Number = "2";
                areaGarage.Name = "Garage";
                areaGarage.LookupParameter("Area Category").Set("Total Covered");
                areaGarage.LookupParameter("Comments").Set("B");

                AreaTag garageTag = curDoc.Create.NewAreaTag(areaFloor, areaGarage, insPoint);
                garageTag.TagHeadPosition = tagInsert.Subtract(tagOffset);
                garageTag.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

                Area areaCoveredPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPatio.Number = "3";
                areaCoveredPatio.Name = "Covered Patio";
                areaCoveredPatio.LookupParameter("Area Category").Set("Total Covered");
                areaCoveredPatio.LookupParameter("Comments").Set("C");

                AreaTag tagCoveredPatio = curDoc.Create.NewAreaTag(areaFloor, areaCoveredPatio, insPoint);
                tagCoveredPatio.TagHeadPosition = tagInsert.Subtract(tagOffset);
                tagCoveredPatio.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

                Area areaCoveredPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPorch.Number = "4";
                areaCoveredPorch.Name = "Covered Porch";
                areaCoveredPorch.LookupParameter("Area Category").Set("Total Covered");
                areaCoveredPorch.LookupParameter("Comments").Set("D");

                AreaTag tagCoveredPorch = curDoc.Create.NewAreaTag(areaFloor, areaCoveredPorch, insPoint);
                tagCoveredPorch.TagHeadPosition = tagInsert.Subtract(tagOffset);
                tagCoveredPorch.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

                Area areaPorteCochere = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorteCochere.Number = "5";
                areaPorteCochere.Name = "Porte Cochere";
                areaPorteCochere.LookupParameter("Area Category").Set("Total Covered");
                areaPorteCochere.LookupParameter("Comments").Set("E");

                AreaTag tagPorteCochere = curDoc.Create.NewAreaTag(areaFloor, areaPorteCochere, insPoint);
                tagPorteCochere.TagHeadPosition = tagInsert.Subtract(tagOffset);
                tagPorteCochere.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

                Area areaPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPatio.Number = "6";
                areaPatio.Name = "Patio";
                areaPatio.LookupParameter("Area Category").Set("Total Uncovered");
                areaPatio.LookupParameter("Comments").Set("F");

                AreaTag tagPatio = curDoc.Create.NewAreaTag(areaFloor, areaPatio, insPoint);
                tagPatio.TagHeadPosition = tagInsert.Subtract(tagOffset);
                tagPatio.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

                Area areaPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorch.Number = "7";
                areaPorch.Name = "Porch";
                areaPorch.LookupParameter("Area Category").Set("Total Uncovered");
                areaPorch.LookupParameter("Comments").Set("G");

                AreaTag tagPorch = curDoc.Create.NewAreaTag(areaFloor, areaPorch, insPoint);
                tagPorch.TagHeadPosition = tagInsert.Subtract(tagOffset);
                tagPorch.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

                Area areaOption1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaOption1.Number = "8";
                areaOption1.Name = "Option";
                areaOption1.LookupParameter("Area Category").Set("Options");
                areaOption1.LookupParameter("Comments").Set("H");

                AreaTag tagOption1 = curDoc.Create.NewAreaTag(areaFloor, areaOption1, insPoint);
                tagOption1.TagHeadPosition = tagInsert.Subtract(tagOffset);
                tagOption1.HasLeader = false;

                insPoint = insPoint.Subtract(offset);

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


