using System.Web.Mvc;
using System.Web.Security;
using WebAppHomeWork.ViewModel;

using BLL;
using Model;
namespace WebAppHomeWork.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult LogIn()
        {
            LogInViewModel logModel = new LogInViewModel();
            return View(logModel);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogIn(LogInViewModel emp)
        {
            if (ModelState.IsValid)
            {
                EmployeeBll bll = new EmployeeBll();

                Employee employee = new Employee();
                employee.EmployeeName = emp.UserName;
                employee.PassWord = emp.PassWord;

                UserStatus userStatus = bll.IsValidEmp(employee);
                bool IsAdmin;
                if (userStatus == UserStatus.AuthenticatedAdmin)
                {
                    IsAdmin = true;
                }
                else if (userStatus == UserStatus.AuthenticatedUser)
                {
                    IsAdmin = false;
                }
                else
                {
                    ModelState.AddModelError("CredentialError", "Invalid EmployeeName or PassWord");
                    emp.ErrorMsg = "用户名或密码错误";
                    return View(emp);
                }
                //FormsAuthentication.SetAuthCookie(emp.UserName, false);
                Session["IsAdmin"] = IsAdmin;
                Session["UserName"] = employee.EmployeeName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Login", emp);
                ;
            }

        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            //--star--
            FormsAuthentication.SignOut();
            //--end--
            return RedirectToAction("LogIn");
        }
    }
}


    