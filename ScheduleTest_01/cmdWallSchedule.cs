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
using System.Linq;
using System.Reflection;

#endregion

namespace ScheduleTest_01
{
    [Transaction(TransactionMode.Manual)]
    public class cmdWallSchedule : IExternalCommand
    {

        // built in parameters / fields to add in schedule
        BuiltInParameter[] BiParams = new BuiltInParameter[] { BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM,
        BuiltInParameter.WALL_BASE_CONSTRAINT, BuiltInParameter.HOST_AREA_COMPUTED, BuiltInParameter.CURVE_ELEM_LENGTH};
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;



            Transaction t = new Transaction(doc, "Create Schedule");
            t.Start();

            // get built in wall category id
            ElementId wallCategoryId = new ElementId(BuiltInCategory.OST_Walls);
            ViewSchedule schedule = ViewSchedule.CreateSchedule(doc, wallCategoryId);
            schedule.Name = "Wall Schedule Sample";
            //family and type group sorting
            ScheduleSortGroupField FamilyTypeSorting = null;
            // base contraint filter
            ScheduleFilter BaseConstraintFilter = null;

            // iterate schedulablefileds
            foreach (SchedulableField sf in schedule.Definition.GetSchedulableFields())
            {
                // check field if in params
                if (CheckField(sf))
                {
                    // add field
                    ScheduleField scheduleField = schedule.Definition.AddField(sf);

                    // schedule's filter (to collect base constraint)
                    if (sf.ParameterId == new ElementId(BuiltInParameter.WALL_BASE_CONSTRAINT))
                    {

                        // create schedulefilter by level
                        BaseConstraintFilter = new ScheduleFilter(scheduleField.FieldId, ScheduleFilterType.Equal,
                            GetLevelByName(doc, "Level 1").Id);
                        // add schedule's filter
                        schedule.Definition.AddFilter(BaseConstraintFilter);
                    }

                    // schedule's group sorting (to collect family and type field)
                    if (sf.ParameterId == new ElementId(BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM))
                    {

                        // create group sorting (family and type)
                        FamilyTypeSorting = new ScheduleSortGroupField(scheduleField.FieldId);
                        // add schedule's group sorting field
                        schedule.Definition.AddSortGroupField(FamilyTypeSorting);
                    }

                }
            }
            // set filter
            schedule.Definition.SetFilter(0, BaseConstraintFilter);
            // set sorting/grouping      
            schedule.Definition.SetSortGroupField(0, FamilyTypeSorting);

            // close transaction
            t.Commit();
            // set active view 
            uidoc.ActiveView = schedule;

            //result
            return Result.Succeeded;
        }

        public bool CheckField(SchedulableField vs)
        {
            foreach (BuiltInParameter bip in BiParams)
            {
                if (new ElementId(bip) == vs.ParameterId)
                    return true;
            }
            return false;
        }

        public Element GetLevelByName(Document doc, string name)
        {
            Level level = new FilteredElementCollector(doc).OfClass(typeof(Level))
                .Cast<Level>().Where(x => x.Name == name).FirstOrDefault();

            return level;
        }
    }
}