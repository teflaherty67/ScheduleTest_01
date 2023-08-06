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
    public class cmdVeneer : IExternalCommand
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
                t.Start("Create Shet Index");

                // check to see if the sheet index exists

                ViewSchedule schedIndex = Utils.GetScheduleByNameContains(doc, "Sheet Index - Elevation S");

                if (schedIndex == null)
                {
                    // duplicate the first schedule found with Sheet Index in the name
                    List<ViewSchedule> listSched = Utils.GetAllScheduleByNameContains(doc, "Sheet Index");

                    ViewSchedule dupSched = listSched.FirstOrDefault();

                    Element viewSched = doc.GetElement(dupSched.Duplicate(ViewDuplicateOption.Duplicate));       

                    // rename the duplicated schedule to the new elevation

                    string originalName = viewSched.Name;
                    string[] schedTitle = originalName.Split('C');

                    string curTitle = schedTitle[0];

                    string lastChar = curTitle.Substring(curTitle.Length - 2);
                    string newLast = "S";

                    viewSched.Name = curTitle.Replace(lastChar, newLast);

                    // set the design option to the specified elevation designation

                    DesignOption curOption = Utils.getDesignOptionByName(doc, newLast); // this code doesn't do anything

                    // set the value of the schedule filter, need to change the filter to Code Filter - Contains - 4

                    // get the Code Filter parameter
                    Parameter curParam = Utils.GetParameterByName(doc, "Code Filter", BuiltInCategory.OST_Sheets);

                    // get the element Id of the parameter
                    ElementId filterCodeId = Utils.GetProjectParameterId(doc, "Code Filter");

                    // create the filter field


                }

                t.Commit();
            }

            return Result.Succeeded;
        }       
    }
}
