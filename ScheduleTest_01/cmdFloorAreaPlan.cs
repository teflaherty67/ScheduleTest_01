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
            //ElementId areaCatID = areaCat.Id;

            ColorFillScheme schemeColorFill = Utils.GetColorFillSchemeByName(curDoc, "Floor");
            //ElementId schemeColorFillId = schemeColorFill.Id;

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

                areaFloor.SetColorFillSchemeId(areaCat.Id, schemeColorFill.Id);

                // area insertion points
                XYZ insStart = new XYZ(0, 0, 0);               

                UV insPoint = new UV(insStart.X, insStart.Y);
                UV offset = new UV(0, 10);

                // area tag inserton points
                XYZ tagInsert = new XYZ(0, 0, 0);
                XYZ tagOffset = new XYZ(0, 10, 0);

                Area areaLiving1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaLiving1.Number = "1";
                areaLiving1.Name = "Living";
                areaLiving1.LookupParameter("Area Category").Set("Total Covered");
                areaLiving1.LookupParameter("Comments").Set("A");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaLiving1), false, TagOrientation.Horizontal, tagInsert);
                AreaTag tagLiving1 = curDoc.Create.NewAreaTag(areaFloor, areaLiving1, insPoint);

                insPoint = insPoint.Add(offset);
                
                Area areaGarage = curDoc.Create.NewArea(areaFloor, insPoint);
                areaGarage.Number = "2";
                areaGarage.Name = "Garage";
                areaGarage.LookupParameter("Area Category").Set("Total Covered");
                areaGarage.LookupParameter("Comments").Set("B");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaGarage), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));
                AreaTag tagGarage = curDoc.Create.NewAreaTag(areaFloor, areaGarage, insPoint);

                insPoint = insPoint.Add(offset);

                Area areaCoveredPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPatio.Number = "3";
                areaCoveredPatio.Name = "Covered Patio";
                areaCoveredPatio.LookupParameter("Area Category").Set("Total Covered");
                areaCoveredPatio.LookupParameter("Comments").Set("C");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaCoveredPatio), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));

                insPoint = insPoint.Add(offset);

                Area areaCoveredPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPorch.Number = "4";
                areaCoveredPorch.Name = "Covered Porch";
                areaCoveredPorch.LookupParameter("Area Category").Set("Total Covered");
                areaCoveredPorch.LookupParameter("Comments").Set("D");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaCoveredPorch), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));

                insPoint = insPoint.Add(offset);

                Area areaPorteCochere = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorteCochere.Number = "5";
                areaPorteCochere.Name = "Porte Cochere";
                areaPorteCochere.LookupParameter("Area Category").Set("Total Covered");
                areaPorteCochere.LookupParameter("Comments").Set("E");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaPorteCochere), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));

                insPoint = insPoint.Add(offset);

                Area areaPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPatio.Number = "6";
                areaPatio.Name = "Patio";
                areaPatio.LookupParameter("Area Category").Set("Total Uncovered");
                areaPatio.LookupParameter("Comments").Set("F");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaPatio), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));

                insPoint = insPoint.Add(offset);

                Area areaPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorch.Number = "7";
                areaPorch.Name = "Porch";
                areaPorch.LookupParameter("Area Category").Set("Total Uncovered");
                areaPorch.LookupParameter("Comments").Set("G");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaPorch), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));

                insPoint = insPoint.Add(offset);

                Area areaOption1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaOption1.Number = "8";
                areaOption1.Name = "Option";
                areaOption1.LookupParameter("Area Category").Set("Options");
                areaOption1.LookupParameter("Comments").Set("H");

                //IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, new Reference(areaOption1), false, TagOrientation.Horizontal, tagInsert = tagInsert.Add(tagOffset));

                insPoint = insPoint.Add(offset);

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


