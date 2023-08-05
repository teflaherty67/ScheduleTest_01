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
    public class Command2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Your code goes here
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfClass(typeof(Level));

            // get area category id
            ElementId catId = new ElementId(BuiltInCategory.OST_Areas);

            // get parameter fields for schedule
            ElementId catFieldId = Utils.GetProjectParameterId(doc, "Area Category");
            ElementId comFieldId = Utils.GetProjectParameterId(doc, "Comments");
            ElementId nameFieldId = Utils.GetProjectParameterId(doc, "Name");
            ElementId areaFieldId = Utils.GetProjectParameterId(doc, "Area");
            ElementId numFieldId = Utils.GetProjectParameterId(doc, "Number");

            List<ElementId> fieldList = new List<ElementId> { catFieldId, comFieldId, nameFieldId, areaFieldId, numFieldId };

            // create the transaction
            using (Transaction t = new Transaction(doc))
            {
                // start the transaction
                t.Start("Create Floor Area Schedule");

                AreaScheme curAreaScheme = Utils.GetAreaSchemeByName(doc, "S Floor");
                ViewSchedule curSched = Utils.CreateAreaSchedule(doc, "Floor Areas - Elevation S", curAreaScheme);

                // add fields to schedule
                foreach (ElementId elementId in fieldList)
                {
                    ScheduleField field = curSched.Definition.AddField(ScheduleFieldType.Instance, elementId);
                }

                ScheduleField catField = curSched.Definition.AddField(ScheduleFieldType.Instance, catFieldId);
                catField.IsHidden = true;

                ScheduleFilter catFilter = new ScheduleFilter(catField.FieldId, ScheduleFilterType.Contains, "Options");
                curSched.Definition.AddFilter(catFilter);
            }


                return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand2";
            string buttonTitle = "Button 2";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 2");

            return myButtonData1.Data;
        }
    }
}
