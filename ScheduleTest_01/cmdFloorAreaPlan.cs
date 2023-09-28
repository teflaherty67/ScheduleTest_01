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

                IndependentTag tagSymbol = Utils.GetIndependentTagByName(curDoc, "Area Tag");

                View vtFloorAreas = Utils.GetViewTemplateByName(curDoc, "10-Floor Area");

                ViewPlan areaFloor = ViewPlan.CreateAreaPlan(curDoc, schemeFloorId, curLevelId);
                areaFloor.Name = "Floor";
                areaFloor.ViewTemplateId = vtFloorAreas.Id;

                XYZ insStart = new XYZ(0, 0, 0);               

                UV insPoint = new UV(insStart.X, insStart.Y);
                UV offset = new UV(0, 10);

                Area areaLiving1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaLiving1.Number = "1";
                areaLiving1.Name = "Living";
                areaLiving1.LookupParameter("Area Category").Set("Total Covered");
                areaLiving1.LookupParameter("Comments").Set("A");
                IndependentTag.Create(curDoc, tagSymbol.Id, areaFloor.Id, areaLiving1, false, TagOrientation.Horizontal, insStart);

                insPoint = insPoint.Add(offset);

                Area areaGarage = curDoc.Create.NewArea(areaFloor, insPoint);
                areaGarage.Number = "2";
                areaGarage.Name = "Garage";
                areaGarage.LookupParameter("Area Category").Set("Total Covered");
                areaGarage.LookupParameter("Comments").Set("B");

                insPoint = insPoint.Add(offset);

                Area areaCoveredPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPatio.Number = "3";
                areaCoveredPatio.Name = "Covered Patio";
                areaCoveredPatio.LookupParameter("Area Category").Set("Total Covered");
                areaCoveredPatio.LookupParameter("Comments").Set("C");

                insPoint = insPoint.Add(offset);

                Area areaCoveredPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaCoveredPorch.Number = "4";
                areaCoveredPorch.Name = "Covered Porch";
                areaCoveredPatio.LookupParameter("Area Category").Set("Total Covered");
                areaCoveredPatio.LookupParameter("Comments").Set("D");

                insPoint = insPoint.Add(offset);

                Area areaPorteCochere = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorteCochere.Number = "5";
                areaPorteCochere.Name = "Porte Cochere";
                areaPorteCochere.LookupParameter("Area Category").Set("Total Covered");
                areaPorteCochere.LookupParameter("Comments").Set("E");

                insPoint = insPoint.Add(offset);

                Area areaPatio = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPatio.Number = "6";
                areaPatio.Name = "Patio";
                areaPatio.LookupParameter("Area Category").Set("Total Uncovered");
                areaPatio.LookupParameter("Comments").Set("F");

                insPoint = insPoint.Add(offset);

                Area areaPorch = curDoc.Create.NewArea(areaFloor, insPoint);
                areaPorch.Number = "7";
                areaPorch.Name = "Porch";
                areaPorch.LookupParameter("Area Category").Set("Total Uncovered");
                areaPorch.LookupParameter("Comments").Set("G");

                insPoint = insPoint.Add(offset);

                Area areaOption1 = curDoc.Create.NewArea(areaFloor, insPoint);
                areaOption1.Number = "8";
                areaOption1.Name = "Option";
                areaOption1.LookupParameter("Area Category").Set("Options");
                areaOptions1.LookupParameter("Comments").Set("H");

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
