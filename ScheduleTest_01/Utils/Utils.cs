using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ScheduleTest_01
{
    internal static class Utils
    {
        // ----Schedule methods

        internal static ViewSchedule CreateAreaSchedule(Document doc, string schedName, AreaScheme curAreaScheme)
        {
            ElementId catId = new ElementId(BuiltInCategory.OST_Areas);
            ViewSchedule newSchedule = ViewSchedule.CreateSchedule(doc, catId, curAreaScheme.Id);
            newSchedule.Name = schedName;

            return newSchedule;
        }
        internal static ViewSchedule CreateSchedule(Document doc, BuiltInCategory curCat, string name)
        {
            ElementId catId = new ElementId(curCat);
            ViewSchedule newSchedule = ViewSchedule.CreateSchedule(doc, catId);
            newSchedule.Name = name;

            return newSchedule;
        }
        internal static void AddFieldsToSchedule(Document doc, ViewSchedule newSched, List<Parameter> paramList)
        {
            foreach (Parameter curParam in paramList)
            {
                SchedulableField newField = new SchedulableField(ScheduleFieldType.Instance, curParam.Id);
                newSched.Definition.AddField(newField);
            }
        }

        internal static List<Parameter> GetParametersByName(Document doc, List<string> paramNames, BuiltInCategory cat)
        {
            List<Parameter> returnList = new List<Parameter>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(cat);

            foreach (string curName in paramNames)
            {
                Parameter curParam = collector.FirstElement().LookupParameter(curName);

                if (curParam != null)
                    returnList.Add(curParam);
            }

            return returnList;
        }
        internal static ElementId GetProjectParameterId(Document doc, string name)
        {
            ParameterElement pElem = new FilteredElementCollector(doc)
                .OfClass(typeof(ParameterElement))
                .Cast<ParameterElement>()
                .Where(e => e.Name.Equals(name))
                .FirstOrDefault();

            return pElem?.Id;
        }

        internal static AreaScheme GetAreaSchemeByName(Document doc, string schemeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(AreaScheme));

            foreach (AreaScheme areaScheme in collector)
            {
                if (areaScheme.Name == schemeName)
                {
                    return areaScheme;
                }
            }

            return null;
        }

        // ----Ribbon methods
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel currentPanel = GetRibbonPanelByName(app, tabName, panelName);

            if (currentPanel == null)
                currentPanel = app.CreateRibbonPanel(tabName, panelName);

            return currentPanel;
        }

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        internal static ViewSchedule GetScheduleByNameContains(Document doc, string scheduleString)
        {
            List<ViewSchedule> m_scheduleList = GetAllSchedules(doc);

            foreach (ViewSchedule curSchedule in m_scheduleList)
            {
                if (curSchedule.Name.Contains(scheduleString))
                    return curSchedule;
            }

            return null;
        }

        internal static List<ViewSchedule> GetAllSchedules(Document doc)
        {
            {
                List<ViewSchedule> m_schedList = new List<ViewSchedule>();

                FilteredElementCollector curCollector = new FilteredElementCollector(doc);
                curCollector.OfClass(typeof(ViewSchedule));

                //loop through views and check if schedule - if so then put into schedule list
                foreach (ViewSchedule curView in curCollector)
                {
                    if (curView.ViewType == ViewType.Schedule)
                    {
                        m_schedList.Add((ViewSchedule)curView);
                    }
                }

                return m_schedList;
            }
        }

        internal static List<ViewSchedule> GetAllScheduleByNameContains(Document doc, string schedName)
        {
            List<ViewSchedule> m_scheduleList = GetAllSchedules(doc);

            List<ViewSchedule> m_returnList = new List<ViewSchedule>();

            foreach (ViewSchedule curSchedule in m_scheduleList)
            {
                if (curSchedule.Name.Contains(schedName))
                    m_returnList.Add(curSchedule);
            }

            return m_returnList;
        }

        #region Design Options

        internal static List<DesignOption> getAllDesignOptions(Document doc)
        {
            FilteredElementCollector curCol = new FilteredElementCollector(doc);
            curCol.OfCategory(BuiltInCategory.OST_DesignOptions);

            List<DesignOption> doList = new List<DesignOption>();
            foreach (DesignOption curOpt in curCol)
            {
                doList.Add(curOpt);
            }

            return doList;
        }

        internal static DesignOption getDesignOptionByName(Document doc, string designOpt)
        {
            //get all design options
            List<DesignOption> doList = getAllDesignOptions(doc);

            foreach (DesignOption curOpt in doList)
            {
                if (curOpt.Name == designOpt)
                {
                    return curOpt;
                }
            }

            return null;
        }

        internal static Parameter GetParameterByName(Document doc, string v, BuiltInCategory oST_Sheets)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region View Templates

        public static View GetViewTemplateByName(Document curDoc, string viewTemplateName)
        {
            List<View> viewTemplateList = GetAllViewTemplates(curDoc);

            foreach (View v in viewTemplateList)
            {
                if (v.Name == viewTemplateName)
                {
                    return v;
                }
            }

            return null;
        }

        public static List<View> GetAllViewTemplates(Document curDoc)
        {
            List<View> returnList = new List<View>();
            List<View> viewList = GetAllViews(curDoc);

            //loop through views and check if is view template
            foreach (View v in viewList)
            {
                if (v.IsTemplate == true)
                {
                    //add view template to list
                    returnList.Add(v);
                }
            }

            return returnList;
        }

        #endregion

        internal static ElementId GetBuiltInParameterId(Document doc, BuiltInCategory cat, BuiltInParameter bip)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(cat);

            Parameter curParam = collector.FirstElement().get_Parameter(bip);

            return curParam?.Id;
        }

        internal static Level GetLevelByName(Document doc, string levelWord)
        {
            List<Level> levels = GetAllLevels(doc);          

            foreach (Level curLevel in levels)
            {
                if (curLevel.Name == levelWord)
                    return curLevel;
            }

            return null;
        }

        public static List<Level> GetAllLevels(Document doc)
        {
            FilteredElementCollector colLevels = new FilteredElementCollector(doc);
            colLevels.OfCategory(BuiltInCategory.OST_Levels);

            List<Level> levels = new List<Level>();
            foreach (Element x in colLevels.ToElements())
            {
                if (x.GetType() == typeof(Level))
                {
                    levels.Add((Level)x);
                }
            }

            return levels;
            //order list by elevation
            //m_levels = (From l In m_levels Order By l.Elevation).tolist()
        }

        public static List<View> GetAllViews(Document curDoc)
        {
            FilteredElementCollector m_colviews = new FilteredElementCollector(curDoc);
            m_colviews.OfCategory(BuiltInCategory.OST_Views);

            List<View> m_views = new List<View>();
            foreach (View x in m_colviews.ToElements())
            {
                m_views.Add(x);
            }

            return m_views;
        }

        internal static AreaTagType GetIndependentTagByName(Document curDoc, string tagName)
        {
            FilteredElementCollector m_colTags = new FilteredElementCollector(curDoc)
                .OfCategory(BuiltInCategory.OST_AreaTags)
                .WhereElementIsElementType();

            foreach (AreaTagType curtag in m_colTags)
                if (curtag.Name.Equals(tagName))
                { 
                    return curtag;
                }

            return null;
        }

        internal static ColorFillScheme GetColorFillSchemeByName(Document curDoc, string schemeName, AreaScheme areaScheme)
        {
            ColorFillScheme colorfill = new FilteredElementCollector(curDoc)
                .OfCategory(BuiltInCategory.OST_ColorFillSchema)
                .Cast<ColorFillScheme>()
                .Where(x => x.Name.Equals(schemeName) && x.AreaSchemeId.Equals(areaScheme.Id))
                .First();

            return colorfill;
        }

        internal static void CreateAreaWithTag(Document curDoc, ViewPlan areaFloor, ref UV insPoint, ref XYZ tagInsert, clsAreaInfo areaInfo)
        {
            Area area = curDoc.Create.NewArea(areaFloor, insPoint);
            area.Number = areaInfo.Number;
            area.Name = areaInfo.Name;
            area.LookupParameter("Area Category").Set(areaInfo.Category);
            area.LookupParameter("Comments").Set(areaInfo.Comments);

            AreaTag tag = curDoc.Create.NewAreaTag(areaFloor, area, insPoint);
            tag.TagHeadPosition = tagInsert;
            tag.HasLeader = false;

            UV offset = new UV(0, 8);
            insPoint = insPoint.Subtract(offset);

            XYZ tagOffset = new XYZ(0, 8, 0);
            tagInsert = tagInsert.Subtract(tagOffset);
        }
    }
}
