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

            // fields to add to schedule
            List<string> paramNames = new List<string>() { "Area Category", "Comments", "Name", "Area", "Number" };

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Schedule");

                // get the area schem for the area schedule
                AreaScheme curAreaScheme = Utils.GetAreaSchemeByName(doc, "S Floor");

                // get the built-in areas category id
                ElementId areasCategoryId = new ElementId(BuiltInCategory.OST_Areas);

                // create the area schedule
                ViewSchedule newSched = Utils.CreateAreaSchedule(doc, "Floor Areas - Elevation S", curAreaScheme);

                // code filter schedule filter
                ScheduleFilter filterCode = null;

                // area Category & Comment sorting
                ScheduleSortGroupField catField = null;
                ScheduleSortGroupField comField = null;





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
