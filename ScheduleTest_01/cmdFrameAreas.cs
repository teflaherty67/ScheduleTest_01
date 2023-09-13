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
using System.Windows.Input;

#endregion

namespace ScheduleTest_01
{
    [Transaction(TransactionMode.Manual)]
    public class cmdFrameAreas : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;
           
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Schedule");

                AreaScheme curAreaScheme = Utils.GetAreaSchemeByName(doc, "S Frame");
                ViewSchedule newFrameSched = Utils.CreateAreaSchedule(doc, "Frame Areas - Elevation S", curAreaScheme);

                List<string> paramNames = new List<string>() { "Area Category", "Name", "Area" };

                List<Parameter> paramList = Utils.GetParametersByName(doc, paramNames, BuiltInCategory.OST_Areas);
                //Utils.AddFieldsToSchedule(doc, newFrameSched, paramList);

                // get element Id of the parameters
                ElementId catFieldId = Utils.GetProjectParameterId(doc, "Area Category");
                ElementId nameFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Rooms, BuiltInParameter.ROOM_NAME);
                ElementId areaFieldId = Utils.GetBuiltInParameterId(doc, BuiltInCategory.OST_Rooms, BuiltInParameter.ROOM_AREA);

                ScheduleField catField = newFrameSched.Definition.AddField(ScheduleFieldType.Instance, catFieldId);
                catField.IsHidden = true;

                ScheduleField nameField = newFrameSched.Definition.AddField(ScheduleFieldType.Instance, nameFieldId);
                nameField.IsHidden = false;
                nameField.ColumnHeading = "Name";
                nameField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                nameField.HorizontalAlignment = ScheduleHorizontalAlignment.Left;

                ScheduleField areaField = newFrameSched.Definition.AddField(ScheduleFieldType.Instance, areaFieldId);
                areaField.IsHidden = false;
                areaField.ColumnHeading = "Area";
                areaField.HeadingOrientation = ScheduleHeadingOrientation.Horizontal;
                areaField.HorizontalAlignment = ScheduleHorizontalAlignment.Right;
                //areaField.IsCalculatedField = true;  // ??? can this be set to "Calculate totals"

                // create the filters

                ScheduleFilter catFilter = new ScheduleFilter(catField.FieldId, ScheduleFilterType.Contains, "Options");
                newFrameSched.Definition.AddFilter(catFilter);

                ScheduleFilter areaFilter = new ScheduleFilter(areaField.FieldId, ScheduleFilterType.HasValue);
                newFrameSched.Definition.AddFilter(areaFilter);

                // set the sorting

                ScheduleSortGroupField catSort = new ScheduleSortGroupField(catField.FieldId, ScheduleSortOrder.Ascending);
                catSort.ShowHeader = true;
                catSort.ShowBlankLine = true;
                newFrameSched.Definition.AddSortGroupField(catSort);


                t.Commit();



            }
            
            return Result.Succeeded;
        }        
    }
}
