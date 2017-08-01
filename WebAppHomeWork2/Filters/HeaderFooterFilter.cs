using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppHomeWork.ViewModel;

namespace WebAppHomeWork.Filters
{
    public class HeaderFooterFilter:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                BaseViewModel baseView = viewResult.Model as BaseViewModel;
                if (baseView != null)
                {
                    baseView.UserName ="";
                    baseView.FooterData = new FooterViewModel();
                    baseView.FooterData.CompanyName = "Star Park";
                    baseView.FooterData.Year = DateTime.Now.Year.ToString();
                }
            }
        }
    }
}