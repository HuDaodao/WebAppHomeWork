using System;
using System.Web;
using System.Web.Mvc;
   //using  Microsoft.SharePoint.Client;

namespace WebAppHomeWork.Filters
{
        public class MemberCheckAttribute : AuthorizeAttribute
        {

            //private RoleType _roleType;
            //public ActionAuthAttribute(RoleType role)
            //{
            //    _roleType = role;
            //}
        protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var isValid = base.AuthorizeCore(httpContext);
                return isValid;
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                base.HandleUnauthorizedRequest(filterContext);
                if (filterContext == null)
                {
                    throw new ArgumentNullException("filterContext");
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("~/Authentication/LogIn");
                }
            }
    }
}