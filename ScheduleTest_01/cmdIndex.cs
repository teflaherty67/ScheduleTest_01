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
using System.Windows.Input;

#endregion

namespace ScheduleTest_01
{
    [Transaction(TransactionMode.Manual)]
    public class cmdIndex : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;            

            // create the transaction
            using (Transaction t = new Transaction(doc))
            {
                // start the transaction
                t.Start("Create Sheet Index");

                // check to see if the sheet index exists

                ViewSchedule schedIndex = Utils.GetScheduleByNameContains(doc, "Sheet Index - Elevation S");

                if (schedIndex == null)
                {
                    // duplicate the first schedule found with Sheet Index in the name
                    List<ViewSchedule> listSched = Utils.GetAllScheduleByNameContains(doc, "Sheet Index");

                    ViewSchedule dupSched = listSched.FirstOrDefault();

                    Element indexSched = doc.GetElement(dupSched.Duplicate(ViewDuplicateOption.Duplicate)); 
                    // ??? is there another way to duplicate a schedule, this method makes it an element
                    // and the code I found to set the filter needs a view schedule element      

                    // rename the duplicated schedule to the new elevation
                    string originalName = indexSched.Name;
                    string[] schedTitle = originalName.Split('C');

                    string curTitle = schedTitle[0];

                    string lastChar = curTitle.Substring(curTitle.Length - 2);
                    string newLast = "S";

                    indexSched.Name = curTitle.Replace(lastChar, newLast);

                    // set the design option to the specified elevation designation

                    DesignOption curOption = Utils.getDesignOptionByName(doc, newLast); // this code doesn't do anything

                    // ??? how to set the value of the schedule filter, need to change the
                    // value of the filter to match the code filter of the new elevation

                    // code I found requires a view schedule, not an element

                    //ScheduleFilter codeFilter = indexSched.Definition.GetFilter(0);
                    
                    //if (codeFilter.IsStringValue)
                    //{
                    //    codeFilter.SetValue("4");
                    //    indexSched.Definition.SetValue(0, codeFilter);
                    //}
                }

                t.Commit();
            }

            return Result.Succeeded;
        }       
    }
}
