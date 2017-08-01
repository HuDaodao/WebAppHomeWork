using System;
using System.Web.Mvc;
using System.Collections.Generic;

using BLL;
using Model;
using WebAppHomeWork.Filters;
using WebAppHomeWork.ViewModel;
using System.Linq;

namespace WebAppHomeWork.Controllers
{
    public class EmployeeController : Controller
    {
        [HeaderFooterFilter]
        public ActionResult Index()
        {
            EmployeeListViewModel empListViewModel = new EmployeeListViewModel();
            return View(empListViewModel);
        }

        /// <summary>
        /// 返回分页的employee数据
        /// </summary>
        /// <param name="searchBase">查询条件</param>
        /// <returns></returns>
        public ActionResult DataList(SearchBaseModel searchBase)
        {
            BaseJsonData<EmployeeViewModel> jsonData = new BaseJsonData<EmployeeViewModel>();
            int total = 0;
           
            var uList = GetEmployeeViewModel(searchBase, out total);
           
            jsonData.draw = searchBase.Draw++;
            jsonData.iTotalDisplayRecords = total;
            jsonData.iTotalRecords = total;
            jsonData.aaData = uList;
            return Json(jsonData);
        }

        /// <summary>
        /// 新增修改员工
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        [AdminFilter]
        [HeaderFooterFilter]
        public ActionResult AddEditEmployee(int? id)
        {
            EmployeeViewModel empView = new EmployeeViewModel();
            
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                EmployeeBll empBll = new EmployeeBll();
                DepartBll deptBll = new DepartBll();
                Employee emp = empBll.GetEmployeeById(int.Parse(id.ToString()));
                empView.Id = emp.Id;
                empView.EmployeeName = emp.EmployeeName;
                empView.PassWord = EmployeeBll.UnBase64(emp.PassWord);
                empView.Email = emp.Email;
                empView.Age = emp.Age;
                empView.DepartmentId =emp.DepartmentId??0;
            }
            DropDown(empView.DepartmentId);
            return View(empView);
        }

        /// <summary>
        /// 保存员工信息
        /// </summary>
        /// <param name="emp">员工实体</param>
        /// <returns></returns>
        [AdminFilter]
        [HttpPost]
        [HeaderFooterFilter]
        public ActionResult SaveEmployee(EmployeeViewModel emp)
        {
            EmployeeBll empBll = new EmployeeBll();
            Employee employee = new Employee();
            if (ModelState.IsValid)
            {
                employee.PassWord = emp.PassWord;
                employee.Id = emp.Id;
                employee.Age = emp.Age;
                employee.Email = emp.Email;
                employee.EmployeeName = emp.EmployeeName;
                employee.DepartmentId = emp.DepartmentId;

                try
                {
                    empBll.SaveEmployee(employee);
                }
                catch(Exception e)
                {
                    DropDown(employee.DepartmentId);
                    return View("AddEditEmployee",emp);
                }
                return RedirectToAction("Index");
            }
            else
            {
                DropDown(employee.DepartmentId);
                return View("AddEditEmployee", emp);
            }
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            string  result = "yes";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                EmployeeBll empBll = new EmployeeBll();
                try
                {
                    empBll.Delete(int.Parse(id.ToString()));
                }
                catch(Exception e)
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
            return Json(result);
        }

        /// <summary>
        /// 修改是否为管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IsAdmin(int id)
        {
            string result = "yes";
            if (!string.IsNullOrEmpty(id.ToString()))
            {
                EmployeeBll empBll = new EmployeeBll();
                try
                {
                    empBll.IsAdminChange(int.Parse(id.ToString()));
                }
                catch(Exception e)
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
            return Json(result);
        }

        /// <summary>
        /// 检查姓名是否存在
        /// </summary>
        /// <param name="empName">员工名</param>
        /// <param name="id">员工id</param>
        /// <returns></returns>
        public JsonResult CheckName(string empName,int id)
        {
            EmployeeBll bll=new EmployeeBll();
            var result=bll.CheckName(empName,id);
            return Json(result);
        }

        /// <summary>
        /// 传入权限控制部分视图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddNewLink()
        {
            if (Convert.ToBoolean(Session["IsAdmin"]))
            {
                return PartialView("AddNewLink");
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="total">总数</param>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public List<EmployeeViewModel> GetEmployeeViewModel(SearchBaseModel search,out int total)
        {
            EmployeeBll empBll = new EmployeeBll();
            List<Employee> employeeList = empBll.GetPageEmployees( search.Search,search.PageStart,search.PageSize,out total);
            DepartBll depatBll = new DepartBll();
            List<Department> departList = depatBll.GetAllDeparts();
            //左连接
            var viewTable= from employee in employeeList
                           join depart in departList
                           on employee.DepartmentId equals  depart.Id into empDept
                           from  ed in empDept.DefaultIfEmpty()
                            select new
                            {
                                employee.Id,
                                employee.Email,
                                employee.Age,
                                employee.IsAdmin,
                                employee.EmployeeName,
                                Name=ed!=null? ed.Name:null
                            };
           
            List<EmployeeViewModel> empView = new List<EmployeeViewModel>();
            //将数据库查出的List<Employee>转为List<EmployeeViewModel>视图模型
            foreach (var emp in viewTable)
            {
                EmployeeViewModel empViewModel = new EmployeeViewModel
                {
                    Id = emp.Id,
                    Email = emp.Email,
                    Age = emp.Age,
                    IsAdmin = emp.IsAdmin ? "是" : "否",
                    EmployeeName = emp.EmployeeName,
                    DepartName = emp.Name
                };
                empView.Add(empViewModel);
            }
            if (search.SortCol == 1)
            {
                if (search.SortDir == "asc")
                {
                    empView = empView.OrderBy(list => list.Age).ToList();
                }
                else
                {
                    empView = empView.OrderByDescending(list => list.Age).ToList();
                }
            }
            return empView;
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <param name="departId">部门ID</param>
        public void DropDown(int? departId)
        {
            DepartBll deptBll = new DepartBll();
            ViewData["DepartmentId"] = new SelectList(deptBll.GetAllDeparts(), "Id", "Name",departId);
        }
    }
}