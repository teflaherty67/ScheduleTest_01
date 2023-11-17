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
    public class cmdFloorArea : IExternalCommand
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

                // get element Id of the parameters
                ElementId catFieldId = Utils.GetProjectParameterId(doc, "Area Category");
                ElementId comFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Areas, BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                ElementId levelFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Areas, BuiltInParameter.ROOM_LEVEL_ID);
                ElementId nameFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Areas, BuiltInParameter.ROOM_NAME);
                ElementId areaFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Areas, BuiltInParameter.ROOM_AREA);
                ElementId numFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Areas, BuiltInParameter.ROOM_NUMBER);

                ScheduleField catField = newSched.Definition.AddField(ScheduleFieldType.Instance, catFieldId);
                catField.IsHidden = true;

                ScheduleField comField = newSched.Definition.AddField(ScheduleFieldType.Instance, comFieldId);
                comField.IsHidden = true;

                ScheduleField levelField = newSched.Definition.AddField(ScheduleFieldType.Instance, levelFieldId);
                levelField.IsHidden = false;
                levelField.ColumnHeading = "Level";
                levelField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                levelField.HorizontalAlignment = ScheduleHorizontalAlignment.Left;

                ScheduleField nameField = newSched.Definition.AddField(ScheduleFieldType.Instance, nameFieldId);
                nameField.IsHidden = true;
                nameField.ColumnHeading = "Name";
                nameField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                nameField.HorizontalAlignment = ScheduleHorizontalAlignment.Left;

                ScheduleField areaField = newSched.Definition.AddField(ScheduleFieldType.Instance, areaFieldId);
                areaField.IsHidden = false;
                areaField.ColumnHeading = "Area";
                areaField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                areaField.HorizontalAlignment = ScheduleHorizontalAlignment.Right;
                areaField.DisplayType = ScheduleFieldDisplayType.Totals;

                ScheduleField numField = newSched.Definition.AddField(ScheduleFieldType.Instance, numFieldId);
                numField.IsHidden = true;

                // create the filters
                ScheduleFilter catFilter = new ScheduleFilter(catField.FieldId, ScheduleFilterType.NotContains, "Options");
                newSched.Definition.AddFilter(catFilter);

                ScheduleFilter areaFilter = new ScheduleFilter(areaField.FieldId, ScheduleFilterType.HasValue);
                newSched.Definition.AddFilter(areaFilter);

                // set the sorting
                ScheduleSortGroupField catSort = new ScheduleSortGroupField(catField.FieldId, ScheduleSortOrder.Ascending);                
                catSort.ShowFooter = true;
                catSort.ShowFooterTitle = true;
                catSort.ShowBlankLine = true;
                newSched.Definition.AddSortGroupField(catSort);

                ScheduleSortGroupField comSort = new ScheduleSortGroupField(comField.FieldId, ScheduleSortOrder.Ascending);
                newSched.Definition.AddSortGroupField(comSort);

                ScheduleSortGroupField nameSort = new ScheduleSortGroupField(nameField.FieldId, ScheduleSortOrder.Ascending);
                nameSort.ShowFooter = true;
                nameSort.ShowHeader = true;
                newSched.Definition.AddSortGroupField(nameSort);

                ScheduleSortGroupField levelSort = new ScheduleSortGroupField(levelField.FieldId, ScheduleSortOrder.Ascending);
                newSched.Definition.AddSortGroupField(levelSort);

                newSched.Definition.IsItemized = false;

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
