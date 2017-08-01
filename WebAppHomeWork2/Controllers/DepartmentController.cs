using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using Model;
using WebAppHomeWork.Filters;
using WebAppHomeWork.ViewModel;

namespace WebAppHomeWork.Controllers
{
    public class DepartmentController : Controller
    {
        [HeaderFooterFilter]
        public ActionResult Index()
        {
            return View(new DepatViewModel());
        }

        /// <summary>
        /// 返回部门Json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeList(int id=0)
        {
            DepartBll deprtBll=new DepartBll();
            var departList=deprtBll.GetDepart(id);
            List<JsonTreeModel> departViewList = ReturnToViewModel(departList);
            var a = departViewList.ToJsonString();
            return Json(departViewList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建修改部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreatAndEdit(int id = 0)
        {
            return null;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public JsonResult Save(DepatViewModel depat)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department();
                department.Id = depat.Id;
                department.Note = depat.Note;
                department.Name = depat.Name;
                department.ParentId = depat.ParentId;
                DepartBll db = new DepartBll();
                db.SaveDept(department);
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id=0)
        {
            DepartBll db = new DepartBll();
            var result=db.DeleteDept(id);
            return Json(result?"ok":null);
        }

        /// <summary>
        /// 转为ViewModel
        /// </summary>
        /// <param name="departs"></param>
        /// <returns></returns>
        public List<JsonTreeModel> ReturnToViewModel(List<Department> departs)
        {
            List<JsonTreeModel> departViewList = new List<JsonTreeModel>();
            DepartBll deprtBll=new DepartBll();
            foreach (var depart in departs)
            {
                JsonTreeModel departView = new JsonTreeModel();
                departView.id = depart.Id;
                departView.text = depart.Name;
                departView.parent = depart.ParentId == 0 ? "#" : depart.ParentId.ToString();
                departView.children = deprtBll.GetDepart(depart.Id).Count>0;
                departViewList.Add(departView);
            }
            return departViewList;
        }

        /// <summary>
        ///获取当前部门的可用父级部门
        /// </summary>
        /// <param name="id">当前部门的ID</param>
        /// <returns>JSon数据</returns>
        public JsonResult DropDownParent(string id,int action)
        {
            DepartBll db = new DepartBll();
            List<Department> dropList;
            Department dept = new Department();
            if (string.IsNullOrEmpty(id)||action==0)
            {
                dropList = db.GetAllDeparts();
            }
            else
            {
                dropList = db.GetParentDept(int.Parse(id));
                if (action == 1)
                {
                    dept = db.GetDepartmentById(int.Parse(id));
                }
            }
            var strResult = "{\"DropList\":" + dropList.ToJsonString() + ",\"Dept\":" + dept.ToJsonString() + "}";
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
    }
}