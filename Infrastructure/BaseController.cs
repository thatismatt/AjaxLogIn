using System;
using System.Web.Mvc;

namespace AjaxLogIn.Infrastructure
{
    public class BaseController : Controller
    {
        protected JsonResult JsonSuccess(object data)
        {
            return Json(new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonSuccess()
        {
            return JsonSuccess(null);
        }

        protected JsonResult JsonError(Exception exception)
        {
            return Json(new { Success = false, Error = exception.GetType().Name + " " + exception.Message }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JsonError(string error)
        {
            return Json(new { Success = false, Error = error }, JsonRequestBehavior.AllowGet);
        }
    }
}
