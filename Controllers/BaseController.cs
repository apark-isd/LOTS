using LOTS3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text.Encodings.Web;

namespace FastMail.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRazorViewEngine _razorViewEngine;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        public BaseController(AppDbContext context, IRazorViewEngine razorViewEngine)
        {
            _context = context;
            _razorViewEngine = razorViewEngine;
        }

        protected virtual string RenderPartialViewToString(string viewName, object model)
        {
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            //set model
            ViewData.Model = model;

            //try to get view by the name
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
            if (viewResult.View == null)
            {
                //try to get a view by the path
                viewResult = _razorViewEngine.GetView(null, viewName, false);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} view was not found");
            }
            using (var stringWriter = new StringWriter())
            {
                var viewContext = new ViewContext(actionContext, viewResult.View, ViewData, TempData, stringWriter, new HtmlHelperOptions());

                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        //protected virtual string RenderViewComponentToString(string componentName, object arguments = null)
        //{
        //    if (string.IsNullOrEmpty(componentName))
        //        throw new ArgumentNullException(nameof(componentName));

        //    var context = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

        //    var viewComponentResult = ViewComponent(componentName, arguments);

        //    var viewData = ViewData;
        //    if (viewData == null)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    var tempData = TempData;
        //    if (tempData == null)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    using (var writer = new StringWriter())
        //    {
        //        var viewContext = new ViewContext(context, NullView.Instance, viewData, tempData, writer, new HtmlHelperOptions());

        //        var viewComponentHelper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
        //        (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

        //        var result = viewComponentResult.ViewComponentType == null ?
        //            viewComponentHelper.InvokeAsync(viewComponentResult.ViewComponentName, viewComponentResult.Arguments) :
        //            viewComponentHelper.InvokeAsync(viewComponentResult.ViewComponentType, viewComponentResult.Arguments);

        //        result.Result.WriteTo(writer, HtmlEncoder.Default);
        //        return writer.ToString();
        //    }
        //}

        /// <summary>
        /// NullView implementation
        /// </summary>
    //    internal class NullView : IView
    //    {
    //        public static readonly NullView Instance = new NullView();

    //        public string Path => string.Empty;

    //        public Task RenderAsync(ViewContext context)
    //        {
    //            if (context == null)
    //            {
    //                throw new ArgumentNullException(nameof(context));
    //            }

    //            return Task.CompletedTask;
    //        }
    //    }

    //    protected IList<SelectListItem> GetMailTypes()
    //    {
    //        var mailTypes = _context.MailTypes
    //            .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
    //            .ToList();

    //        return mailTypes;
    //    }

    //    protected IList<SelectListItem> GetDepartments()
    //    {
    //        var departments = _context.Departments
    //            .Where(x => x.IsActive == true)
    //            .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
    //            .OrderBy(x => x.Text).ToList();

    //        return departments;
    //    }

    //    protected IList<SelectListItem> GetDepartmentNumbers()
    //    {
    //        var departments = _context.Departments
    //            .Select(x => new SelectListItem { Text = x.Name, Value = x.Number })
    //            .OrderBy(x => x.Text).ToList();

    //        return departments;
    //    }

    //    protected IList<SelectListItem> GetFacilities(int departmentId = 0)
    //    {
    //        var facilities = from f in _context.Facilities
    //                         join d in _context.Departments on f.DepartmentId equals d.Id
    //                         where d.Id == departmentId && f.IsActive == true
    //                         select new { f.Id, f.LocationAddress, f.Room, f.Floor, f.Division };

    //        var list = new List<SelectListItem>();
    //        foreach (var f in facilities.OrderBy(x => x.LocationAddress))
    //        {
    //            string text = f.LocationAddress.Contains("CA")
    //                ? f.LocationAddress.Substring(0, f.LocationAddress.IndexOf("CA") - 1)
    //                : f.LocationAddress;
    //            if (!string.IsNullOrEmpty(f.Room))
    //                text += $" RM {f.Room}";
    //            if (!string.IsNullOrEmpty(f.Floor))
    //                text += $" FL {f.Floor}";
    //            if (!string.IsNullOrEmpty(f.Division))
    //                text += $" ({f.Division})";

    //            list.Add(new SelectListItem { Text = text, Value = f.Id.ToString() });
    //        }

    //        return list;
    //    }

    //    protected IList<SelectListItem> GetRecipients(int departmentId = 0)
    //    {
    //        var recipients = new List<SelectListItem>();
    //        recipients.Add(new SelectListItem { Text = "* Mail room", Value = "99999" });

    //        var list = _context.Recipients.Where(x => x.DepartmentId == departmentId).ToList();

    //        foreach (var item in list)
    //        {
    //            recipients.Add(new SelectListItem { Text = $"{item.Name}", Value = item.Id.ToString() });
    //        }

    //        return recipients.OrderBy(x => x.Text).ToList();
    //    }
    }

}
