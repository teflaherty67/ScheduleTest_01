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
                t.Start("Create Exterior Veneer Schedule");

                // check to see if the sheet index exists

                ViewSchedule veneerIndex = Utils.GetScheduleByNameContains(doc, "Exterior Veneer Calculations - Elevation S ");

                if (veneerIndex == null)
                {
                    // duplicate the first schedule found with Sheet Index in the name
                    List<ViewSchedule> listSched = Utils.GetAllScheduleByNameContains(doc, "Exterior Veneer Calculations");

                    ViewSchedule dupSched = listSched.FirstOrDefault();

                    Element viewSched = doc.GetElement(dupSched.Duplicate(ViewDuplicateOption.Duplicate));

                    // rename the duplicated schedule to the new elevation

                    string originalName = viewSched.Name;
                    string[] schedTitle = originalName.Split('-');

                    viewSched.Name = schedTitle[0] + "- Elevation S";

                    // set the design option to the specified elevation designation
                    DesignOption curOption = Utils.getDesignOptionByName(doc, "S");

                    Parameter doParam = viewSched.get_Parameter(BuiltInParameter.VIEWER_OPTION_VISIBILITY);

                    doParam.Set(curOption.Id); //??? the code is getting the right option, but it's not changing anything in the model                     
                }

                t.Commit();
            }

            return Result.Succeeded;
        }       
    }
}
