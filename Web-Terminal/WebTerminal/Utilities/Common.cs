using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using WebTerminal.Models;

namespace WebTerminal.Utilities
{
    public static class Common
    {
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}