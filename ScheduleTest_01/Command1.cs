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
    public class Command1 : IExternalCommand
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
                t.Start("Create Schedule");

                AreaScheme curAreaScheme = Utils.GetAreaSchemeByName(doc, "S Floor");
                ViewSchedule newSched = Utils.CreateAreaSchedule(doc, "Floor Areas - Elevation S", curAreaScheme);

                List<string> paramNames = new List<string>() { "Area Category", "Comments", "Name", "Area", "Number" };

                List<Parameter> paramList = Utils.GetParametersByName(doc, paramNames, BuiltInCategory.OST_Areas);
                Utils.AddFieldsToSchedule(doc, newSched, paramList);

                // get element Id of the parameters
                ElementId catFieldId = Utils.GetProjectParameterId(doc, "Area Category");
                ElementId comFieldId = Utils.GetProjectParameterId(doc, "Comments");
                ElementId nameFieldId = Utils.GetProjectParameterId(doc, "Name");
                ElementId areaFieldId = Utils.GetProjectParameterId(doc, "Area");
                ElementId numFieldId = Utils.GetProjectParameterId(doc, "Number");

                ScheduleField catField = newSched.Definition.AddField(ScheduleFieldType.Instance, catFieldId);
                catField.IsHidden = true;

                ScheduleField comField = newSched.Definition.AddField(ScheduleFieldType.Instance, comFieldId);
                comField.IsHidden = true;

                ScheduleField nameField = newSched.Definition.AddField(ScheduleFieldType.Instance, nameFieldId);
                nameField.IsHidden = false;
                nameField.ColumnHeading = "Name";
                nameField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                nameField.HorizontalAlignment = ScheduleHorizontalAlignment.Left;

                ScheduleField areaField = newSched.Definition.AddField(ScheduleFieldType.Instance, areaFieldId);
                areaField.IsHidden = false;
                areaField.ColumnHeading = "Area";
                areaField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                areaField.HorizontalAlignment = ScheduleHorizontalAlignment.Right;               

                ScheduleField numField = newSched.Definition.AddField(ScheduleFieldType.Instance, numFieldId);
                numField.IsHidden = true;

                // create the filters
                ScheduleFilter catFilter = new ScheduleFilter(catField.FieldId, ScheduleFilterType.Contains, "Options");
                newSched.Definition.AddFilter(catFilter);

                ScheduleFilter areaFilter = new ScheduleFilter(areaField.FieldId, ScheduleFilterType.GreaterThan, "0 SF");
                newSched.Definition.AddFilter(areaFilter);

                // set the sorting
                ScheduleSortGroupField catSort = new ScheduleSortGroupField(catField.FieldId, ScheduleSortOrder.Ascending);
                catSort.ShowFooter = true;
                catSort.ShowFooterCount = true; //??? how to set the footer to be "Title and Totals"
                catSort.ShowBlankLine = true;

                ScheduleSortGroupField comSort = new ScheduleSortGroupField(comField.FieldId, ScheduleSortOrder.Ascending);

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
